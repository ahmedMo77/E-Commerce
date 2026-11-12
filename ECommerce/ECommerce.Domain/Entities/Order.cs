using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public AppUser Customer { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
        public ICollection<StoreOrder> StoreOrders { get; set; }
            = new List<StoreOrder>();
    }
}
