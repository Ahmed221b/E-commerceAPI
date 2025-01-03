namespace E_Commerce.Models
{
    public class Customer : ApplicationUser
    {
        public string Address { get; set; }
        public DateOnly DataOfBirth { get; set; }
    }
}
