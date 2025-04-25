namespace E_Commerce.Core.Shared
{
    public static class Constants
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Supervisor = "Supervisor";


        public enum PaymentStatus
        {
            Pending = 1,
            Completed = 2,
            Failed = 3,
            Refunded = 4
        }

        public enum OrderStatus
        {
            Processing = 1,
            Shipped = 2,
            Delivered = 3,
            Cancelled = 4,
            Rejected = 5
        }


    }
}
