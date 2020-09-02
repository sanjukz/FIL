using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20180911130000_RemoveColumns
{
    [Migration(2018, 09, 11, 13, 0, 0)]
    public class RemoveColumns : BaseMigrator
    {
        public override void Up()
        {
            if (Schema.Table("CorporateRequestDetails").Column("EventTicketDetailId").Exists())
            {
                Delete.ForeignKey("FK_CorporateRequestDetails_EventTicketDetailId_EventTicketDetails_Id")
                .OnTable("CorporateRequestDetails");

                Delete.Column("EventTicketDetailId")
                .FromTable("CorporateRequestDetails");
            }
            if (Schema.Table("InvoiceDetails").Column("EventTicketDetailId").Exists())
            {
                Delete.ForeignKey("FK_InvoiceDetails_EventTicketDetailId_EventTicketDetails_Id")
                  .OnTable("InvoiceDetails");

                Delete.Column("EventTicketDetailId")
                .FromTable("InvoiceDetails");
            }
            if (Schema.Table("CorporateRequests").Column("ZipcodeId").Exists())
            {
                Delete.ForeignKey("FK_CorporateRequests_ZipcodeId_Zipcodes_Id")
                  .OnTable("CorporateRequests");
                
                Delete.Column("ZipcodeId")
                .FromTable("CorporateRequests");
            }

            if (Schema.Table("Sponsors").Column("Address").Exists())
            {
                Delete.Column("Address")
                .FromTable("Sponsors");
            }
        }
    }
}
