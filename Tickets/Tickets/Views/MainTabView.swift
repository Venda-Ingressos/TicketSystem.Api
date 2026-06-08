//
//  MainTabView.swift
//  Tickets
//
//  Created by Kaua on 31/05/26.
//

import SwiftUI

struct MainTabView: View {
    let currentUser: TicketUser
    let onLogout: () -> Void

    @StateObject private var eventVM = EventListViewModel()
    @StateObject private var salesVM: SalesViewModel

    init(currentUser: TicketUser, onLogout: @escaping () -> Void) {
        self.currentUser = currentUser
        self.onLogout = onLogout
        _salesVM = StateObject(wrappedValue: SalesViewModel(currentUser: currentUser))
    }

    var body: some View {
        TabView {
            EventsView(viewModel: eventVM, currentUser: currentUser)
                .tabItem {
                    Label("Eventos", systemImage: "ticket")
                }

            SalesView(viewModel: salesVM, eventViewModel: eventVM, currentUser: currentUser)
                .tabItem {
                    Label("Compras", systemImage: "cart")
                }

            AccountView(currentUser: currentUser, onLogout: onLogout)
                .tabItem {
                    Label("Perfil", systemImage: "person.crop.circle")
                }
        }
        .onAppear {
            eventVM.loadEvents()
            salesVM.loadOrders()
        }
    }
}

#Preview {
    MainTabView(
        currentUser: TicketUser(
            id: UUID(),
            name: "Kaua",
            email: "kaua@gmail.com"
        ),
        onLogout: {}
    )
}
