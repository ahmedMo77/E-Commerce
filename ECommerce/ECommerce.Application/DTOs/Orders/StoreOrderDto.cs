using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Orders
{
    public class StoreOrderDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = [];
    }
}
