using System.Collections.Generic;

namespace FIL.Contracts.Models.ASI
{
    public class Identity
    {
        public string Type { get; set; }
        public string No { get; set; }
    }

    public class Nationality
    {
        public string Group { get; set; }
        public string Country { get; set; }
    }

    public class Timeslot
    {
        public int Id { get; set; }
    }

    public class Monument
    {
        public string Code { get; set; }
        public bool Optional { get; set; }
        public bool Main { get; set; }
        public Timeslot Timeslot { get; set; }
    }

    public class Visitor
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public Identity Identity { get; set; }
        public Nationality Nationality { get; set; }
        public Monument Monument { get; set; }
        public decimal Amount { get; set; }
        public string VisitorId { get; set; }
    }

    public class RootObject
    {
        public string TransactionId { get; set; }
        public List<Visitor> Visitors { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
    }
}