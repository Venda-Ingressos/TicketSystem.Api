//
//  ApiService.swift
//  Tickets
//
//  Created by Kaua on 31/05/26.
//

import Foundation
import Alamofire

enum ApiServiceError: LocalizedError {
    case invalidEmail
    case notFound(String)
    case server(String)
    case emptyResponse

    var errorDescription: String? {
        switch self {
        case .invalidEmail:
            return "Não foi possível montar a rota desse e-mail."
        case .notFound(let message):
            return message
        case .server(let message):
            return message
        case .emptyResponse:
            return "A API respondeu sem dados."
        }
    }
}

private struct ApiErrorResponse: Decodable {
    let error: String?
    let message: String?
}

final class ApiService {
    static let shared = ApiService()

    private let baseURL = {
        #if targetEnvironment(simulator)
        return "http://localhost:5123/api"
        #else
        return "http://192.168.3.95:5123/api"
        #endif
    }()

    private let jsonDecoder: JSONDecoder = {
        let decoder = JSONDecoder()

        let formatterWithFractionalSeconds = ISO8601DateFormatter()
        formatterWithFractionalSeconds.formatOptions = [.withInternetDateTime, .withFractionalSeconds]

        let formatterWithoutFractionalSeconds = ISO8601DateFormatter()
        formatterWithoutFractionalSeconds.formatOptions = [.withInternetDateTime]

        let dotNetFormatterWithFractionalSeconds = DateFormatter()
        dotNetFormatterWithFractionalSeconds.locale = Locale(identifier: "en_US_POSIX")
        dotNetFormatterWithFractionalSeconds.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSSSSS"

        let dotNetFormatterWithoutFractionalSeconds = DateFormatter()
        dotNetFormatterWithoutFractionalSeconds.locale = Locale(identifier: "en_US_POSIX")
        dotNetFormatterWithoutFractionalSeconds.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"

        decoder.dateDecodingStrategy = .custom { decoder in
            let container = try decoder.singleValueContainer()
            let value = try container.decode(String.self)

            if let date =
                formatterWithFractionalSeconds.date(from: value) ??
                formatterWithoutFractionalSeconds.date(from: value) ??
                dotNetFormatterWithFractionalSeconds.date(from: value) ??
                dotNetFormatterWithoutFractionalSeconds.date(from: value)
            {
                return date
            }

            throw DecodingError.dataCorruptedError(in: container, debugDescription: "Data invalida: \(value)")
        }

        return decoder
    }()

    private let jsonEncoder: JSONEncoder = {
        let encoder = JSONEncoder()
        encoder.dateEncodingStrategy = .iso8601
        return encoder
    }()

    private init() {}

    func fetchEvents(completion: @escaping (Result<[EventResponse], Error>) -> Void) {
        AF.request("\(baseURL)/Event", method: .get)
            .validate()
            .responseDecodable(of: [EventResponse].self, decoder: jsonDecoder) { response in
                switch response.result {
                case .success(let events):
                    completion(.success(events))
                case .failure:
                    completion(.failure(self.resolveError(from: response)))
                }
            }
    }

    func createEvent(request: CreateEventRequest, completion: @escaping (Result<Void, Error>) -> Void) {
        AF.request(
            "\(baseURL)/Event",
            method: .post,
            parameters: request,
            encoder: JSONParameterEncoder(encoder: jsonEncoder)
        )
        .validate()
        .responseData { response in
            switch response.result {
            case .success:
                completion(.success(()))
            case .failure:
                completion(.failure(self.resolveError(from: response)))
            }
        }
    }

    func fetchOrders(for userId: UUID, completion: @escaping (Result<[OrderResponse], Error>) -> Void) {
        AF.request("\(baseURL)/Sale/user/\(userId.uuidString.lowercased())", method: .get)
            .validate()
            .responseDecodable(of: [OrderResponse].self, decoder: jsonDecoder) { response in
                switch response.result {
                case .success(let orders):
                    completion(.success(orders))
                case .failure:
                    completion(.failure(self.resolveError(from: response)))
                }
            }
    }

    func createOrder(request: CreateOrderRequest, completion: @escaping (Result<Void, Error>) -> Void) {
        AF.request(
            "\(baseURL)/Sale",
            method: .post,
            parameters: request,
            encoder: JSONParameterEncoder(encoder: jsonEncoder)
        )
        .validate()
        .responseData { response in
            switch response.result {
            case .success:
                completion(.success(()))
            case .failure:
                completion(.failure(self.resolveError(from: response)))
            }
        }
    }

    func approveOrder(id: UUID, completion: @escaping (Result<Void, Error>) -> Void) {
        AF.request("\(baseURL)/Sale/\(id.uuidString.lowercased())/approve", method: .put)
            .validate()
            .responseData { response in
                switch response.result {
                case .success:
                    completion(.success(()))
                case .failure:
                    completion(.failure(self.resolveError(from: response)))
                }
            }
    }

    func fetchUser(byEmail email: String, completion: @escaping (Result<TicketUser, Error>) -> Void) {
        let allowedCharacters = CharacterSet.urlPathAllowed.subtracting(CharacterSet(charactersIn: "/"))

        guard let encodedEmail = email.addingPercentEncoding(withAllowedCharacters: allowedCharacters) else {
            completion(.failure(ApiServiceError.invalidEmail))
            return
        }

        AF.request("\(baseURL)/User/email/\(encodedEmail)", method: .get)
            .validate()
            .responseDecodable(of: TicketUser.self, decoder: jsonDecoder) { response in
                switch response.result {
                case .success(let user):
                    completion(.success(user))
                case .failure:
                    completion(.failure(self.resolveError(from: response)))
                }
            }
    }

    func createUser(request: CreateUserRequest, completion: @escaping (Result<CreateUserResponse, Error>) -> Void) {
        AF.request(
            "\(baseURL)/User",
            method: .post,
            parameters: request,
            encoder: JSONParameterEncoder(encoder: jsonEncoder)
        )
        .validate()
        .responseDecodable(of: CreateUserResponse.self, decoder: jsonDecoder) { response in
            switch response.result {
            case .success(let createdUser):
                completion(.success(createdUser))
            case .failure:
                completion(.failure(self.resolveError(from: response)))
            }
        }
    }

    private func resolveError<T>(from response: AFDataResponse<T>) -> Error {
        let statusCode = response.response?.statusCode

        if let data = response.data, let apiMessage = decodeMessage(from: data) {
            if statusCode == 404 {
                return ApiServiceError.notFound(apiMessage)
            }

            return ApiServiceError.server(apiMessage)
        }

        if statusCode == 404 {
            return ApiServiceError.notFound("Não encontramos esse registro na API.")
        }

        return response.error ?? ApiServiceError.emptyResponse
    }

    private func decodeMessage(from data: Data) -> String? {
        if let parsed = try? JSONDecoder().decode(ApiErrorResponse.self, from: data) {
            return parsed.error ?? parsed.message
        }

        return String(data: data, encoding: .utf8)
    }
}
