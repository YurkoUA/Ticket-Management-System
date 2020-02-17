using System;
using System.IO;
using Newtonsoft.Json;

namespace TicketManagementSystem.AutoTest.Util
{
    public static class OptionsHelper
    {
        public static TestOptions GetTestOptions()
        {
            var path = Environment.CurrentDirectory + "\\Configuration\\test-options.json";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("test-options.json is not found.");
            }

            var content = File.ReadAllText(path);
            var options = JsonConvert.DeserializeObject<TestOptions>(content);
            return options;
        }
    }
}
