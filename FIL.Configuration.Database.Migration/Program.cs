using CommandLine;
using FIL.Database.Migration.Core;

namespace FIL.Configuration.Database.Migration
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var parser = Parser.Default.ParseArguments<Options>(args);
            var result = -1;
            parser.WithParsed(options =>
            {
                var runner = new Runner(options);
                result = runner.Run();
            });

            return result;
        }
    }
}