// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using TicketSystem.Api.Sales.Interfaces;
// using TicketSystem.Api.Sales.ValueObjects;
// using TicketSystem.Api.Shared.Data;

// using DomainTicketOrder = TicketSystem.Api.Sales.Entities.TicketOrder;
// using SharedTicketOrder = TicketSystem.Api.Shared.Entities.TicketOrder;

// namespace TicketSystem.Api.Sales.Repositories
// {
//     public class TicketOrderRepository : ITicketOrderRepository
//     {
//         private readonly TicketContext _context;

//         public TicketOrderRepository(TicketContext context)
//         {
//             _context = context;
//         }

//         public async Task<DomainTicketOrder> GetByIdAsync(Guid id)
//         {
//             var sharedOrder = await _context.TicketOrders.FindAsync(id);
//             return sharedOrder == null ? null : MapToDomain(sharedOrder);
//         }

//         public async Task<IEnumerable<DomainTicketOrder>> GetByUserIdAsync(Guid userId)
//         {
//             var sharedOrders = await _context.TicketOrders
//                 .Where(order => order.UserId == userId)
//                 .ToListAsync();

//             return sharedOrders.Select(MapToDomain);
//         }

//         public async Task AddAsync(DomainTicketOrder order)
//         {
//             var sharedOrder = new SharedTicketOrder
//             {
//                 EventId = order.EventId,
//                 UserId = order.UserId,
//                 Quantity = order.Quantity,
//                 Status = (int)order.Status
//             };

//             await _context.TicketOrders.AddAsync(sharedOrder);
//             await _context.SaveChangesAsync();
//             order.Id = sharedOrder.Id;
//         }

//         public async Task UpdateAsync(DomainTicketOrder order)
//         {
//             var sharedOrder = await _context.TicketOrders.FindAsync(order.Id);
//             if (sharedOrder == null)
//             {
//                 return;
//             }

//             sharedOrder.EventId = order.EventId;
//             sharedOrder.UserId = order.UserId;
//             sharedOrder.Quantity = order.Quantity;
//             sharedOrder.Status = (int)order.Status;

//             _context.TicketOrders.Update(sharedOrder);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<int> GetTotalTicketsSoldForEventAsync(Guid eventId)
//         {
//             return await _context.TicketOrders
//                 .Where(order => order.EventId == eventId && order.Status == (int)PaymentStatus.Approved)
//                 .SumAsync(order => order.Quantity);
//         }

//         private static DomainTicketOrder MapToDomain(SharedTicketOrder sharedOrder)
//         {
//             var domainOrder = new DomainTicketOrder(sharedOrder.EventId, sharedOrder.UserId, sharedOrder.Quantity)
//             {
//                 Id = sharedOrder.Id
//             };

//             if (sharedOrder.Status == (int)PaymentStatus.Approved)
//             {
//                 domainOrder.ApprovePayment();
//             }
//             else if (sharedOrder.Status == (int)PaymentStatus.Rejected)
//             {
//                 domainOrder.RejectPayment();
//             }

//             return domainOrder;
//         }
//     }
// }
