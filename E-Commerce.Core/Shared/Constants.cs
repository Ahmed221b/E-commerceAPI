using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Shared
{
    public static class Constants
    {
        public enum Roles 
        { 
            Supervisor = 1,
            Admin = 2,
            Customer = 3,
        }


        public enum PaymentStatus
        {
            Pending = 1,
            Completed = 2,
            Failed = 3,
            Refunded = 4
        }

        public enum OrderStatus
        {
            Pending = 1,
            Processing = 2,
            Shipped = 3,
            Delivered = 4,
            Cancelled = 5
        }


    }
}
