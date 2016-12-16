using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsdToClassT4
{
    public class TemplateDev
    {
        public static string Transform()
        {
            // change this to your schema path!
            string[] schemaPaths = new[] {
                @"C:\temp\T4Sc hemaToClass\T4SchemaToClass\Test\INT037.Movex.FuelTransactions.xsd"
            };





            var xsdPath = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe";

            var runId = Guid.NewGuid();
            var workFolder = Path.Combine(Path.GetTempPath(), runId.ToString());
            Directory.CreateDirectory(workFolder);

            var schemaPathsString = schemaPaths
                .Select(x => "\"" + x + "\"")
                .Aggregate((a, n) => a + " " + n);

            var process = Process.Start(new ProcessStartInfo
            {
                Arguments = string.Format("{0} /c /out:\"{1}\"", schemaPathsString, workFolder),
                CreateNoWindow = true,
                FileName = xsdPath,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            });
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return "WorkFolder: " + workFolder + "\r\n\r\n" + process.StandardError.ReadToEnd();
            }

            var first = true;
            string header;
            string content;
            foreach(var outFile in Directory.EnumerateFiles(workFolder))
            {
                var lines = File.ReadAllLines(outFile);

                if (first)
                {
                    first = false;
                    header = lines.Take(16).Aggregate((a, n) => a + "\r\n" + n);
                }

                content = lines.Skip(16).Aggregate((a, n) => a + "\r\n" + n);
            }

            Directory.Delete(workFolder, true);





            return null;
        }
        private static void WriteLine(string sdfjklf) { }
    }
}
