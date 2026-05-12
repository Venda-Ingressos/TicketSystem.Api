using System;
using TicketSystem.Api.Shared.Entities;
using TicketSystem.Api.Sales.ValueObjects;

namespace TicketSystem.Api.Sales.Entities
{
    public class TicketOrder : EntityBase
    {
        // Referências aos outros Bounded Contexts (apenas os IDs)
        public Guid EventId { get; private set; }
        public Guid UserId { get; private set; }

        public int Quantity { get; private set; }
        public PaymentStatus Status { get; private set; }

        protected TicketOrder() { }

        public TicketOrder(Guid eventId, Guid userId, int quantity)
        {
            if (eventId == Guid.Empty) throw new ArgumentException("O ID do evento é inválido.");
            if (userId == Guid.Empty) throw new ArgumentException("O ID do usuário é inválido.");
            if (quantity <= 0) throw new ArgumentException("A quantidade de ingressos deve ser maior que zero.");

            EventId = eventId;
            UserId = userId;
            Quantity = quantity;

            // Regra de negócio: Toda compra nasce como pendente aguardando pagamento
            Status = PaymentStatus.Pending;
        }
        public void ApprovePayment()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pedidos com status Pendente podem ser aprovados.");

            Status = PaymentStatus.Approved;
            UpdateTimestamp();
        }
        public void RejectPayment()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pedidos com status Pendente podem ser rejeitados.");

            Status = PaymentStatus.Rejected;
            UpdateTimestamp();
        }
    }
}