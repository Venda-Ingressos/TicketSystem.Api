//
//  AppUI.swift
//  Tickets
//
//  Created by Kaua on 07/06/26.
//

import SwiftUI

struct AppBackground: View {
    var body: some View {
        LinearGradient(
            colors: [
                Color(red: 0.97, green: 0.98, blue: 1.0),
                Color.white
            ],
            startPoint: .topLeading,
            endPoint: .bottomTrailing
        )
        .ignoresSafeArea()
    }
}

struct ScreenTitle: View {
    let title: String
    let subtitle: String

    var body: some View {
        VStack(alignment: .leading, spacing: 4) {
            Text(title)
                .font(.system(size: 34, weight: .bold, design: .rounded))

            if !subtitle.isEmpty {
                Text(subtitle)
                    .font(.system(size: 15, weight: .regular, design: .rounded))
                    .foregroundStyle(.secondary)
            }
        }
        .frame(maxWidth: .infinity, alignment: .leading)
    }
}

struct AppCard<Content: View>: View {
    let content: Content

    init(@ViewBuilder content: () -> Content) {
        self.content = content()
    }

    var body: some View {
        VStack(alignment: .leading, spacing: 12) {
            content
        }
        .padding()
        .frame(maxWidth: .infinity, alignment: .leading)
        .background(Color.white)
        .clipShape(RoundedRectangle(cornerRadius: 16))
        .shadow(color: .black.opacity(0.06), radius: 10, y: 4)
    }
}

struct AppTextField: View {
    let title: String
    let placeholder: String
    let systemImage: String
    @Binding var text: String
    var keyboardType: UIKeyboardType = .default
    var textInputAutocapitalization: TextInputAutocapitalization = .sentences

    var body: some View {
        VStack(alignment: .leading, spacing: 8) {
            Text(title)
                .font(.caption)
                .foregroundStyle(.secondary)

            HStack(spacing: 10) {
                Image(systemName: systemImage)
                    .foregroundStyle(.secondary)

                TextField(placeholder, text: $text)
                    .keyboardType(keyboardType)
                    .textInputAutocapitalization(textInputAutocapitalization)
                    .autocorrectionDisabled(true)
            }
            .padding(12)
            .background(Color(.systemGray6))
            .clipShape(RoundedRectangle(cornerRadius: 12))
        }
    }
}

struct AppPrimaryButton: View {
    let title: String
    var isDisabled: Bool = false
    let action: () -> Void

    var body: some View {
        Button(title, action: action)
            .font(.headline)
            .foregroundStyle(.white)
            .frame(maxWidth: .infinity)
            .padding(.vertical, 14)
            .background(isDisabled ? Color.gray : Color.black)
            .clipShape(RoundedRectangle(cornerRadius: 14))
            .disabled(isDisabled)
    }
}

struct AppSecondaryButton: View {
    let title: String
    var isDisabled: Bool = false
    let action: () -> Void

    var body: some View {
        Button(title, action: action)
            .font(.headline)
            .foregroundStyle(.black)
            .frame(maxWidth: .infinity)
            .padding(.vertical, 14)
            .background(Color(.systemGray6))
            .clipShape(RoundedRectangle(cornerRadius: 14))
            .disabled(isDisabled)
            .opacity(isDisabled ? 0.6 : 1)
    }
}

struct EmptyStateCard: View {
    let title: String
    let message: String

    var body: some View {
        AppCard {
            Text(title)
                .font(.headline)

            Text(message)
                .foregroundStyle(.secondary)
        }
    }
}

struct ProfileRow: View {
    let title: String
    let value: String

    var body: some View {
        VStack(alignment: .leading, spacing: 4) {
            Text(title)
                .font(.caption)
                .foregroundStyle(.secondary)

            Text(value)
                .font(.body)
        }
    }
}

#Preview {
    ZStack {
        AppBackground()
        AppCard {
            ScreenTitle(title: "Preview", subtitle: "Componentes")
            AppTextField(
                title: "Nome",
                placeholder: "Seu nome",
                systemImage: "person.fill",
                text: .constant("Kaua")
            )
            AppPrimaryButton(title: "Salvar") {}
            AppSecondaryButton(title: "Cancelar") {}
            EmptyStateCard(title: "Vazio", message: "Nada por aqui.")
            ProfileRow(title: "E-mail", value: "kaua@gmail.com")
        }
        .padding()
    }
}
