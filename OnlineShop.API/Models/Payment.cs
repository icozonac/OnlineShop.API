namespace OnlineShop.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();
        public float TotalAmount { get; set; }
        public float ShipingCharges { get; set; }
        public float AmountReduced { get; set; }
        public float AmountPaid { get; set; }
        public string CreatedAt { get; set; }
    }
}
