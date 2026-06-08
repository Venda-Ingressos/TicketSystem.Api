//
//  AccountView.swift
//  Tickets
//
//  Created by Kaua on 06/06/26.
//

import SwiftUI

struct AccountView: View {
    let currentUser: TicketUser
    let onLogout: () -> Void

    var body: some View {
        NavigationView {
            ZStack {
                AppBackground()

                VStack(alignment: .leading, spacing: 18) {
                    ScreenTitle(title: "Perfil", subtitle: "")

                    AppCard {
                        ProfileRow(title: "Nome", value: currentUser.name)
                        ProfileRow(title: "E-mail", value: currentUser.email)
                        ProfileRow(title: "Id", value: currentUser.id.uuidString)
                    }

                    AppPrimaryButton(title: "Sair") {
                        onLogout()
                    }

                    Spacer()
                }
                .padding(22)
            }
            .navigationBarTitleDisplayMode(.inline)
        }
    }
}

#Preview {
    AccountView(
        currentUser: TicketUser(
            id: UUID(),
            name: "Kaua",
            email: "kaua@gmail.com"
        ),
        onLogout: {}
    )
}
