using FluentMigrator.Builders.Insert;

namespace FIL.Database.Migration.Data.TestData
{
    public static class VenueTestData
    {
        public static void TestVenueRows(this IInsertExpressionRoot insert)
        {
            /* EXMAPLE
            insert.IntoTable("Venues").WithIdentityInsert().Row(new
            {
                Id = 1,
                Name = "Cool"
            });*/
        }
}
}
