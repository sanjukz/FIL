using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ASI
{
    public class Payment
    {
        public string Id { get; set; }
        public string Provider { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string Gateway { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    public class IdentityData
    {
        public string Type { get; set; }
        public string No { get; set; }
    }

    public class NationalityData
    {
        public string Group { get; set; }
        public string Country { get; set; }
    }

    public class TimeslotData
    {
        public int Id { get; set; }
    }

    public class MonumentData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Optional { get; set; }
        public TimeslotData Timeslot { get; set; }
    }

    public class Ticket
    {
        public string VisitorId { get; set; }
        public string Date { get; set; }
        public string No { get; set; }
        public string QrCode { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public IdentityData Identity { get; set; }
        public NationalityData Nationality { get; set; }
        public MonumentData Monument { get; set; }
        public bool IsAdult { get; set; }
        public decimal Amount { get; set; }
    }

    public class ASIData
    {
        public string TransactionId { get; set; }
        public string Hash { get; set; }
        public Payment Payment { get; set; }
        public List<Ticket> Tickets { get; set; }
    }

    public class ASIResponseFormData
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public string TransactionId { get; set; }
        public ASIData Data { get; set; }
    }
}