using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Persistance
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configuration = new();

                var devPath = Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/SmartLink.API/appsettings.json");
                if (File.Exists(devPath))
                {
                    configuration.SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/SmartLink.API")));
                }
                else
                {
                    configuration.SetBasePath(Directory.GetCurrentDirectory());
                }

                configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                return configuration.GetConnectionString("DatabaseConnection");
            }
        }
    }
}
