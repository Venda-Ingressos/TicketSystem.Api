//
//  SalesCard.swift
//  Tickets
//
//  Created by Kaua on 07/06/26.
//

import SwiftUI

struct SalesCard: View {
    let order: OrderResponse
    let eventTitle: String
    let onApprove: () -> Void

    var body: some View {
        AppCard {
            Text(eventTitle)
                .font(.headline)

            Text("Quantidade: \(order.quantity)")
            Text("Status: \(order.status.title)")
            Text("Id: \(order.id.uuidString)")
                .font(.caption)
                .foregroundStyle(.secondary)

            if order.status == .pending {
                AppPrimaryButton(title: "Aprovar") {
                    onApprove()
                }
            }
        }
    }
}

#Preview {
    ZStack {
        AppBackground()
        SalesCard(
            order: OrderResponse(
                id: UUID(),
                eventId: UUID(),
                userId: UUID(),
                quantity: 2,
                status: .pending
            ),
            eventTitle: "Show no Centro",
            onApprove: {}
        )
        .padding()
    }
}
