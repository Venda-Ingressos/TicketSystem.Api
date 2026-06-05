using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Api.Sales.Interfaces;
using TicketSystem.Api.Sales.ValueObjects;
using TicketSystem.Api.Shared.Data;

using DomainOrder = TicketSystem.Api.Sales.Entities.TicketOrder;
using SharedOrder = TicketSystem.Api.Shared.Entities.TicketOrder;

namespace TicketSystem.Api.Sales.Repositories
{
    public class TicketOrderRepository : ITicketOrderRepository
    {
        private readonly TicketContext _context;
        public TicketOrderRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task Add(DomainOrder order)
        {
            var sharedOrder = new SharedOrder
            {
                Id = order.Id == Guid.Empty ? Guid.NewGuid() : order.Id,
                EventId = order.EventId,
                UserId = order.UserId,
                Quantity = order.Quantity,
                Status = (int)order.Status
            };

            _context.TicketOrders.Add(sharedOrder);
            await _context.SaveChangesAsync();
            order.Id = sharedOrder.Id;
        }

        public async Task<DomainOrder> GetById(Guid id)
        {
            var sharedOrder = await _context.TicketOrders.FindAsync(id);
            if (sharedOrder == null) return null;

            return MapToDomain(sharedOrder);
        }

        public async Task<IEnumerable<DomainOrder>> GetByUserId(Guid userId)
        {
            var sharedOrders = await _context.TicketOrders
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return sharedOrders.Select(MapToDomain);
        }

        public async Task Update(DomainOrder order)
        {
            var sharedOrder = await _context.TicketOrders.FindAsync(order.Id);

            if (sharedOrder != null)
            {
                sharedOrder.EventId = order.EventId;
                sharedOrder.UserId = order.UserId;
                sharedOrder.Quantity = order.Quantity;
                sharedOrder.Status = (int)order.Status;

                _context.TicketOrders.Update(sharedOrder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalTicketsSoldForEvent(Guid eventId)
        {
            return await _context.TicketOrders
                .Where(o => o.EventId == eventId && o.Status == (int)PaymentStatus.Approved)
                .SumAsync(o => o.Quantity);
        }

        private static DomainOrder MapToDomain(SharedOrder sharedOrder)
        {
            var order = new DomainOrder(sharedOrder.EventId, sharedOrder.UserId, sharedOrder.Quantity)
            {
                Id = sharedOrder.Id
            };

            if (sharedOrder.Status == (int)PaymentStatus.Approved)
            {
                order.ApprovePayment();
            }
            else if (sharedOrder.Status == (int)PaymentStatus.Rejected)
            {
                order.RejectPayment();
            }

            return order;
        }
    }
}
