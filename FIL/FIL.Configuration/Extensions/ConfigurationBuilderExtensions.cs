using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Configuration.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Dictionary<string, string> GetConfig(IConfiguration configuration)
        {
            return
                configuration.AsEnumerable().Where(c => c.Key.Contains("iis:env") && c.Value != null)
                    .Select(pair => pair.Value.Split(new[] { '=' }, 2))
                    .ToDictionary(keypair => keypair[0], keypair => keypair[1]);
        }
    }
}