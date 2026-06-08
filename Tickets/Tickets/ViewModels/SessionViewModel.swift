//
//  SessionViewModel.swift
//  Tickets
//
//  Created by Kaua on 06/06/26.
//

import Foundation
import Combine

final class SessionViewModel: ObservableObject {
    @Published private(set) var currentUser: TicketUser?
    @Published var draftName: String
    @Published var draftEmail: String
    @Published var isLoading = false
    @Published var loadingMessage = ""
    @Published var errorMessage: String?

    private let apiService = ApiService.shared
    private let defaults = UserDefaults.standard

    private enum StorageKeys {
        static let currentUser = "tickets.currentUser"
        static let draftName = "tickets.draftName"
        static let draftEmail = "tickets.draftEmail"
    }

    init() {
        draftName = defaults.string(forKey: StorageKeys.draftName) ?? ""
        draftEmail = defaults.string(forKey: StorageKeys.draftEmail) ?? ""

        if
            let data = defaults.data(forKey: StorageKeys.currentUser),
            let user = try? JSONDecoder().decode(TicketUser.self, from: data)
        {
            currentUser = user
        } else {
            currentUser = nil
        }
    }

    var isAuthenticated: Bool {
        currentUser != nil
    }

    func updateDraftName(_ value: String) {
        draftName = value
        defaults.set(value, forKey: StorageKeys.draftName)
        clearError()
    }

    func updateDraftEmail(_ value: String) {
        draftEmail = value
        defaults.set(value, forKey: StorageKeys.draftEmail)
        clearError()
    }

    func signIn() {
        let normalizedEmail = draftEmail.trimmingCharacters(in: .whitespacesAndNewlines).lowercased()

        guard !normalizedEmail.isEmpty, normalizedEmail.contains("@") else {
            errorMessage = "Digite um e-mail válido para entrar."
            return
        }

        updateDraftEmail(normalizedEmail)
        setLoading(true, message: "Entrando...")

        apiService.fetchUser(byEmail: normalizedEmail) { [weak self] result in
            DispatchQueue.main.async {
                guard let self else { return }
                self.setLoading(false)

                switch result {
                case .success(let user):
                    self.persist(user: user)
                case .failure(let error):
                    self.errorMessage = error.localizedDescription
                }
            }
        }
    }

    func createAccount() {
        let normalizedName = draftName.trimmingCharacters(in: .whitespacesAndNewlines)
        let normalizedEmail = draftEmail.trimmingCharacters(in: .whitespacesAndNewlines).lowercased()

        guard normalizedName.count >= 3 else {
            errorMessage = "O nome precisa ter pelo menos 3 caracteres."
            return
        }

        guard !normalizedEmail.isEmpty, normalizedEmail.contains("@") else {
            errorMessage = "Digite um e-mail válido para criar sua conta."
            return
        }

        updateDraftName(normalizedName)
        updateDraftEmail(normalizedEmail)
        setLoading(true, message: "Criando seu acesso...")

        let request = CreateUserRequest(name: normalizedName, email: normalizedEmail)
        apiService.createUser(request: request) { [weak self] result in
            DispatchQueue.main.async {
                guard let self else { return }
                self.setLoading(false)

                switch result {
                case .success(let response):
                    let user = TicketUser(id: response.id, name: normalizedName, email: normalizedEmail)
                    self.persist(user: user)
                case .failure(let error):
                    self.errorMessage = error.localizedDescription
                }
            }
        }
    }

    func logout() {
        currentUser = nil
        defaults.removeObject(forKey: StorageKeys.currentUser)
        clearError()
    }

    func clearError() {
        errorMessage = nil
    }

    private func persist(user: TicketUser) {
        currentUser = user

        if let data = try? JSONEncoder().encode(user) {
            defaults.set(data, forKey: StorageKeys.currentUser)
        }

        clearError()
    }

    private func setLoading(_ isLoading: Bool, message: String = "") {
        self.isLoading = isLoading
        loadingMessage = message
    }
}
