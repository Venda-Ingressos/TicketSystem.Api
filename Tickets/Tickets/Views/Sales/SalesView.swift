//
//  SalesView.swift
//  Tickets
//
//  Created by Kaua on 31/05/26.
//

import SwiftUI

struct SalesView: View {
    @ObservedObject var viewModel: SalesViewModel
    @ObservedObject var eventViewModel: EventListViewModel
    let currentUser: TicketUser

    @State private var showBuySheet = false
    @State private var selectedEventId: UUID?
    @State private var quantity = 1

    var body: some View {
        NavigationView {
            ZStack {
                AppBackground()

                if viewModel.isLoading {
                    ProgressView("Carregando vendas...")
                } else if let errorMessage = viewModel.errorMessage {
                    EmptyStateCard(title: "Erro", message: errorMessage)
                } else if viewModel.orders.isEmpty {
                    VStack {
                        ScreenTitle(title: "Compras", subtitle: currentUser.email)
                        EmptyStateCard(title: "Sem compras", message: "As compras vão aparecer aqui.")
                    }
                    .padding(22)
                } else {
                    ScrollView(showsIndicators: false) {
                        VStack(alignment: .leading, spacing: 18) {
                            ScreenTitle(title: "Compras", subtitle: currentUser.email)

                            ForEach(viewModel.orders) { order in
                                SalesCard(
                                    order: order,
                                    eventTitle: title(for: order.eventId)
                                ) {
                                    viewModel.approveTicketPayment(orderId: order.id)
                                }
                            }
                        }
                        .padding(22)
                    }
                }
            }
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button(action: {
                        selectedEventId = eventViewModel.events.first?.id
                        showBuySheet = true
                    }) {
                        Image(systemName: "cart.badge.plus")
                            .font(.system(size: 18, weight: .semibold))
                    }
                    .disabled(eventViewModel.events.isEmpty)
                }
            }
            .sheet(isPresented: $showBuySheet) {
                NavigationView {
                    Form {
                        Section("Compra") {
                            Text(currentUser.name)
                            Text(currentUser.email)
                                .foregroundStyle(.secondary)
                            Stepper("Quantidade: \(quantity)", value: $quantity, in: 1...10)
                        }

                        Section("Selecione o evento") {
                            Picker("Evento", selection: $selectedEventId) {
                                ForEach(eventViewModel.events) { event in
                                    Text(event.title).tag(Optional(event.id))
                                }
                            }
                        }
                    }
                    .navigationTitle("Novo pedido")
                    .navigationBarTitleDisplayMode(.inline)
                    .toolbar {
                        ToolbarItem(placement: .cancellationAction) {
                            Button("Cancelar") { showBuySheet = false }
                        }
                        ToolbarItem(placement: .confirmationAction) {
                            Button("Finalizar") {
                                if let eventId = selectedEventId {
                                    viewModel.checkoutTicket(eventId: eventId, quantity: quantity)
                                    quantity = 1
                                    showBuySheet = false
                                }
                            }
                            .disabled(selectedEventId == nil)
                        }
                    }
                }
            }
        }
    }

    private func title(for eventId: UUID) -> String {
        eventViewModel.events.first(where: { $0.id == eventId })?.title ?? "Evento \(eventId.uuidString.prefix(6))"
    }
}

#Preview {
    SalesView(
        viewModel: SalesViewModel(
            currentUser: TicketUser(
                id: UUID(),
                name: "Kaua",
                email: "kaua@gmail.com"
            )
        ),
        eventViewModel: EventListViewModel(),
        currentUser: TicketUser(
            id: UUID(),
            name: "Kaua",
            email: "kaua@gmail.com"
        )
    )
}
