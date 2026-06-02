using System;
using TicketSystem.Api.Sales.ValueObjects;
namespace TicketSystem.Api.Sales.Entities
{
    public class TicketOrder
    {
        public Guid Id { get; set; }
        // Referências aos IDs 
        public Guid EventId { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; private set; }
        public PaymentStatus Status { get; private set; }


        protected TicketOrder() { }//??

        public TicketOrder(Guid eventId, Guid userId, int quantity)
        {
            if (eventId == Guid.Empty) throw new ArgumentException("ID evento inválido.");
            if (userId == Guid.Empty) throw new ArgumentException("ID usuário inválido.");
            if (quantity <= 0) throw new ArgumentException("A quantidade de ingressos deve ser maior que zero.");

            EventId = eventId;
            UserId = userId;
            Quantity = quantity;

            // Regra de negócio: Toda compra nasce como pendente aguardando pagamento
            Status = PaymentStatus.Pending;
        }

        // status precisa estar pendente para ser aprovado
        public void ApprovePayment()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pedidos com status Pendente podem ser aprovados.");

            Status = PaymentStatus.Approved;
            // UpdateTimestamp(); <-- Removido!
        }

        // status precisa estar pendente para ser rejeitado
        public void RejectPayment()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Apenas pedidos com status Pendente podem ser rejeitados.");

            Status = PaymentStatus.Rejected;
            // UpdateTimestamp(); <-- Removido!
        }

        // se o pedido já tiver sido rejeitado, não pode ser cancelado novamente
        public void CancelOrder()
        {
            if (Status == PaymentStatus.Rejected)
                throw new Exception("Order is already canceled.");

            Status = PaymentStatus.Rejected;
        }

    }
}