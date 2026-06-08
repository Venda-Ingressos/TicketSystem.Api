//
//  EventsView.swift
//  Tickets
//
//  Created by Kaua on 31/05/26.
//

import SwiftUI

struct EventsView: View {
    @ObservedObject var viewModel: EventListViewModel
    let currentUser: TicketUser

    @State private var showAddSheet = false
    @State private var eventTitle = ""
    @State private var eventDescription = ""
    @State private var eventDate = Date().addingTimeInterval(86400)
    @State private var capacity = 100
    @State private var ticketPrice = "89.90"

    var body: some View {
        NavigationView {
            ZStack {
                AppBackground()

                if viewModel.isLoading {
                    ProgressView("Buscando eventos...")
                } else if let errorMessage = viewModel.errorMessage {
                    EmptyStateCard(title: "Erro", message: errorMessage)
                } else if viewModel.events.isEmpty {
                    VStack {
                        heroSection
                        EmptyStateCard(title: "Sem eventos", message: "Crie o primeiro evento.")
                    }
                    .padding(22)
                } else {
                    ScrollView(showsIndicators: false) {
                        VStack(alignment: .leading, spacing: 26) {
                            heroSection

                            VStack(spacing: 16) {
                                ForEach(viewModel.events) { event in
                                    EventTicketCard(event: event)
                                }
                            }
                        }
                        .padding(.horizontal, 22)
                        .padding(.top, 12)
                        .padding(.bottom, 28)
                    }
                }
            }
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button(action: { showAddSheet = true }) {
                        Image(systemName: "plus")
                            .font(.system(size: 20, weight: .medium))
                    }
                }
            }
            .sheet(isPresented: $showAddSheet) {
                NavigationView {
                    Form {
                        Section("Detalhes do evento") {
                            TextField("Título", text: $eventTitle)
                            TextField("Descrição", text: $eventDescription, axis: .vertical)
                            DatePicker("Data", selection: $eventDate, in: Date()...)
                            Stepper("Capacidade: \(capacity)", value: $capacity, in: 10...10000)
                            TextField("Preço do ingresso", text: $ticketPrice)
                                .keyboardType(.decimalPad)
                        }
                    }
                    .navigationTitle("Novo evento")
                    .navigationBarTitleDisplayMode(.inline)
                    .toolbar {
                        ToolbarItem(placement: .cancellationAction) {
                            Button("Cancelar") { showAddSheet = false }
                        }
                        ToolbarItem(placement: .confirmationAction) {
                            Button("Salvar") {
                                guard let price = normalizedTicketPrice else { return }

                                viewModel.addNewEvent(
                                    title: eventTitle,
                                    description: eventDescription,
                                    date: eventDate,
                                    capacity: capacity,
                                    ticketPrice: price
                                )

                                resetForm()
                                showAddSheet = false
                            }
                            .disabled(!canSaveEvent)
                        }
                    }
                }
            }
        }
    }

    private var heroSection: some View {
        ScreenTitle(title: "Eventos", subtitle: currentUser.name)
    }

    private var canSaveEvent: Bool {
        !eventTitle.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty &&
        !eventDescription.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty &&
        normalizedTicketPrice != nil
    }

    private var normalizedTicketPrice: Decimal? {
        Decimal(string: ticketPrice.replacingOccurrences(of: ",", with: "."))
    }

    private func resetForm() {
        eventTitle = ""
        eventDescription = ""
        eventDate = Date().addingTimeInterval(86400)
        capacity = 100
        ticketPrice = "89.90"
    }
}

#Preview {
    EventsView(
        viewModel: EventListViewModel(),
        currentUser: TicketUser(
            id: UUID(),
            name: "Kaua",
            email: "kaua@gmail.com"
        )
    )
}
