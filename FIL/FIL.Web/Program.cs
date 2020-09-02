using Microsoft.AspNetCore.Hosting;

namespace FIL.Web.Feel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Core.Startup.Program.BuildWebHostBuilder(args).UseStartup<Startup>().Build().Run();
        }
    }
}
