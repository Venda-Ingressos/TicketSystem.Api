//
//  SalesViewModel.swift
//  Tickets
//
//  Created by Kaua on 31/05/26.
//

import Foundation
import Combine

final class SalesViewModel: ObservableObject {
    @Published var orders: [OrderResponse] = []
    @Published var isLoading = false
    @Published var errorMessage: String?

    private let apiService = ApiService.shared
    private let currentUser: TicketUser

    init(currentUser: TicketUser) {
        self.currentUser = currentUser
    }

    func loadOrders() {
        isLoading = true
        errorMessage = nil

        apiService.fetchOrders(for: currentUser.id) { [weak self] result in
            guard let self else { return }

            DispatchQueue.main.async {
                self.isLoading = false

                switch result {
                case .success(let fetchedOrders):
                    self.orders = fetchedOrders
                case .failure(let error):
                    self.errorMessage = error.localizedDescription
                }
            }
        }
    }

    func checkoutTicket(eventId: UUID, quantity: Int) {
        let request = CreateOrderRequest(eventId: eventId, userId: currentUser.id, quantity: quantity)

        apiService.createOrder(request: request) { [weak self] result in
            guard let self else { return }

            DispatchQueue.main.async {
                switch result {
                case .success:
                    self.loadOrders()
                case .failure(let error):
                    self.errorMessage = "Erro ao criar pedido: \(error.localizedDescription)"
                }
            }
        }
    }

    func approveTicketPayment(orderId: UUID) {
        apiService.approveOrder(id: orderId) { [weak self] result in
            guard let self else { return }

            DispatchQueue.main.async {
                switch result {
                case .success:
                    self.loadOrders()
                case .failure(let error):
                    self.errorMessage = "Erro ao aprovar pagamento: \(error.localizedDescription)"
                }
            }
        }
    }
}
