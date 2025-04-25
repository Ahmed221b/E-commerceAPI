using E_Commerce.Core.DTO.Order;
using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Services
{
    public static class InvoiceGenerator
    {
        public static string CreateInvoiceMessageBody(GetOrderDTO order)
        {
            var itemsDetails = string.Join("", order.OrderItems.Select(item =>
                $"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.Price:C}</td></tr>"));

            return $@"
        <html>
            <head>
                <style>
                    table {{
                        width: 100%;
                        border-collapse: collapse;
                    }}
                    th, td {{
                        border: 1px solid black;
                        padding: 8px;
                        text-align: left;
                    }}
                    th {{
                        background-color: #f2f2f2;
                    }}
                </style>
            </head>
            <body>
                <p>Hi,</p>
                <p>Thank you for your order! Here is your invoice for Order #{order.OrderId}.</p>
                <table>
                    <thead>
                        <tr>
                            <th>Product Name</th>
                            <th>Quantity</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {itemsDetails}
                    </tbody>
                </table>
                <p><strong>Total:</strong> {order.TotalPrice:C}</p>
                <p>Thanks for your business!</p>
            </body>
        </html>
    ";
        }
    }
}
