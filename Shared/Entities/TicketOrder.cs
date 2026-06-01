using System;

namespace TicketSystem.Api.Shared.Entities
{
    public class TicketOrder : EntityBase
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public int Quantity { get; set; }

        // Salvamos como int no banco. O Repositório vai traduzir 
        // de/para o Enum PaymentStatus na hora de conversar com o UseCase.
        public int Status { get; set; }
    }
}