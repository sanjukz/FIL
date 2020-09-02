using CommandLine;
using FIL.Database.Migration.Core;

namespace FIL.Database.Migration
{
    class Program
    {
        static int Main(string[] args)
        {
            var parser = Parser.Default.ParseArguments<Options>(args);
            var result = -1;
            parser.WithParsed(options =>
            {
                options.GenerateEnums = true;
                var runner = new Runner(options);
                result = runner.Run();
            });
            
            return result;
        }
    }
}
