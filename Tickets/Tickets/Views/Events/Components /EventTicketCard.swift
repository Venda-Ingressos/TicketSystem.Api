//
//  EventTicketCard.swift
//  Tickets
//
//  Created by Kaua on 07/06/26.
//

import SwiftUI

struct EventTicketCard: View {
    let event: EventResponse

    var body: some View {
        AppCard {
            Text(event.title)
                .font(.headline)

            Text(event.description)
                .foregroundStyle(.secondary)

            Text("Data: \(event.date.formatted(date: .abbreviated, time: .shortened))")
            Text("Capacidade: \(event.totalCapacity)")
            Text("Ingresso: \(event.ticketPrice.formatted(.currency(code: "BRL")))")
            Text("Id: \(event.id.uuidString)")
                .font(.caption)
                .foregroundStyle(.secondary)
        }
    }
}

#Preview {
    ZStack {
        AppBackground()
        EventTicketCard(
            event: EventResponse(
                id: UUID(),
                title: "Show no Centro",
                description: "Evento de teste",
                date: .now.addingTimeInterval(86400),
                totalCapacity: 300,
                ticketPrice: 99.90
            )
        )
        .padding()
    }
}
