//
//  LoginView.swift
//  Tickets
//
//  Created by Kaua on 06/06/26.
//

import SwiftUI

struct LoginView: View {
    @EnvironmentObject private var session: SessionViewModel

    private var canCreateAccount: Bool {
        session.draftName.trimmingCharacters(in: .whitespacesAndNewlines).count >= 3 &&
        session.draftEmail.trimmingCharacters(in: .whitespacesAndNewlines).contains("@")
    }

    private var canSignIn: Bool {
        session.draftEmail.trimmingCharacters(in: .whitespacesAndNewlines).contains("@")
    }

    var body: some View {
        ZStack {
            AppBackground()

            VStack(spacing: 20) {
                ScreenTitle(title: "Users", subtitle: "Criar ou entrar")

                AppCard {
                    AppTextField(
                        title: "Nome",
                        placeholder: "Seu nome",
                        systemImage: "person.fill",
                        text: Binding(
                            get: { session.draftName },
                            set: { session.updateDraftName($0) }
                        ),
                        textInputAutocapitalization: .words
                    )

                    AppTextField(
                        title: "E-mail",
                        placeholder: "voce@exemplo.com",
                        systemImage: "envelope.fill",
                        text: Binding(
                            get: { session.draftEmail },
                            set: { session.updateDraftEmail($0) }
                        ),
                        keyboardType: .emailAddress,
                        textInputAutocapitalization: .never
                    )

                    if let errorMessage = session.errorMessage {
                        Text(errorMessage)
                            .font(.footnote)
                            .foregroundStyle(.red)
                    }
                }

                VStack(spacing: 12) {
                    AppPrimaryButton(
                        title: session.isLoading ? session.loadingMessage : "Entrar",
                        isDisabled: !canSignIn || session.isLoading
                    ) {
                        session.signIn()
                    }

                    AppSecondaryButton(
                        title: "Criar usuário",
                        isDisabled: !canCreateAccount || session.isLoading
                    ) {
                        session.createAccount()
                    }
                }
            }
            .padding(24)
            .frame(maxWidth: 500)
        }
    }
}

#Preview {
    LoginView()
        .environmentObject(SessionViewModel())
}
