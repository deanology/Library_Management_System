using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Books
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string ISBN { get; set; }
        [Required] public string PublishYear { get; set; }
        [Required] public double CoverPrice { get; set; }
        [Required] public bool AvailabilityStatus { get; set; }
        public ICollection<CheckOut> CheckOut { get; set; }
        public ICollection<CheckIn> CheckIn { get; set; }
    }
    public class CheckOut
    {
        public int Id { get; set; }
        [Required] public string Fullname { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string NIN { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public int BookId { get; set; }
        public Books Book { get; set; }
    }
    public class CheckIn
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NIN { get; set; }
        public string CheckOutDate { get; set; }
        public string ExpectedReturnDate { get; set; }
        public int PenaltyFee { get; set; }
        public int DaysDefaulted { get; set; }
        public int BookId { get; set; }
        public Books Book { get; set; }
    }
    public class Search
    {
        public string SearchTerm { get; set; }
    }
}
