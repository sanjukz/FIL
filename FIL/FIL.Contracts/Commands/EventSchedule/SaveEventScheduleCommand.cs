using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.EventSchedule
{
    public class SaveEventScheduleCommand : BaseCommand
    {
        public List<CreateSubEventModel> SubEventList { get; set; }
        public int DeliveryValue { get; set; }
        public List<CreateFeeTypeModel> FeeTypes { get; set; }
        public Guid userAltId { get; set; }
    }

    public class CreateSubEventModel
    {
        public string Description { get; set; }
        public string name { get; set; }
        public long eventId { get; set; }
        public long id { get; set; }
        public int venueId { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public List<CreateSubEventMatchModel> matches { get; set; }
        public List<CreateSubEventTicketCategoryModel> ticketCategories { get; set; }
    }

    public class CreateFeeTypeModel
    {
        public long id { get; set; }
        public int feeId { get; set; }
        public int valueTypeId { get; set; }
        public int value { get; set; }
        public string displayName { get; set; }
    }

    public class CreateSubEventTicketCategoryModel
    {
        public long id;
        public int ticketCategoryId;
        public int capacity;
        public int price;
        public string ticketCategoryName;
        public int currencyId;
    }

    public class CreateSubEventMatchModel
    {
        public long id;
        public int matchIndex;
        public string description;
        public int teamA;
        public int teamB;
        public string teamAName;
        public string teamBName;
        public int matchNo;
        public int matchDay;
        public string startDateTime;
        public string endDateTime;
    }
}