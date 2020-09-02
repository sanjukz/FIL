using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.Kidzania.BookCreateOption
{
    public class BookOption
    {
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime VisitDate { get; set; }
        public long TransactionId { get; set; }
        public int NoOfTickets { get; set; }
        public List<VisitorType> VisitorTypes { get; set; }
        public string Ip { get; set; }
    }

    public class VisitorType
    {
        public string VisitorTypeId { get; set; }
        public string VisitorTypeDesc { get; set; }
        public string VisitorTypeAges { get; set; }
        public string VisitorTypePrice { get; set; }
        public string VisitorGender { get; set; }
        public string VisitorAge { get; set; }
        public string VisitorName { get; set; }
        public string VisitorDOB { get; set; }
    }
}