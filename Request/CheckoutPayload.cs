using System;
namespace Library_Management_System.Request
{
    public class CheckoutPayload
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NIN { get; set; }
        public string BookISBN { get; set; }
    }
}
