using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FIL.Web.Kitms.Feel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Core.Startup.Program.BuildWebHostBuilder(args).UseStartup<Startup>().Build().Run();
        }
    }
}
