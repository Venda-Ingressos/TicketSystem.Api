//
//  ProfileRow.swift
//  Tickets
//
//  Created by Kaua on 07/06/26.
//

import SwiftUI

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
    ProfileRow(title: "E-mail", value: "kaua@gmail.com")
        .padding()
}
