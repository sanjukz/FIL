namespace FIL.Contracts.Utils
{
    public class Constant
    {
        public class ASIConstant
        {
            public static string CountryName = "India"; // COUNTRY NOT COMING FROM API SO USED CONSTANT....
            public static string CurrencyCode = "INR";
            public static string Category = "Monuments"; // ALL ITEMS BELONGS TO MONUMENTS....
        }

        public class HohoConstant
        {
            public static int EventCategoryId = 85;
        }

        public class TiqetsConstant
        {
            public static int CurrencyCode = 15;       // ALl Products are retuned in currency Euro
            public static string CategoryId = "1";
            public static int DefaultEventCategoryId = 17;  // Use the default if api don't return
        }

        public class Zoom
        {
            public class Message
            {
                public const string Invalid = "Invalid link, Please try again with the link mailed to you";
                public const string Active = "Your current session is active, Please close that to continue";
                public const string MeetingEnd = "Sorry, your session is expired";
                public const string MeetingNotStarted = "Sorry, you can join only before 10 minutes of scheduled time";
            }
        }

        public class SqlStatement
        {
            public class ScheduleDetail
            {
                public const string InsertCommand = "Insert Into ScheduleDetails (EventScheduleId,StartDateTime,EndDateTime,IsEnabled,CreatedUtc,CreatedBy)" +
               " Values (@EventScheduleId, @StartDateTime,@EndDateTime,@IsEnabled,@CreatedUtc,@CreatedBy);";

                public const string UpdateCommand = "Update ScheduleDetails set StartDateTime=@StartDateTime,EndDateTime=@EndDateTime,IsEnabled=@IsEnabled where id = @Id;";
                public const string DeleteCommand = "Delete ScheduleDetails where id = @Id;";
            }
        }

        public class TestEmail
        {
            public static string TestEmails = "" +
                "pratibha.dhage@zoonga.com,rupalihegade77@gmail.com,pratibha@feelaplace.com,@test.com,rupali.hegade@zoonga.com" +
                ",sanjiv.chavan@zoonga.com,pratibha@feelitlive.com";
        }
    }
}