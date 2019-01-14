using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LogTreatment
{
    public class Mapping
    {
        private string Url {get;set;}
        
        public Mapping(string url)
        {
            Url = url;
        }

        public string MapMinhaCdnToAgora()
        {
            ValidateUrl();

            string logsCdn = DownloadLogsCdn();

            IList<Log> logs = GenerateLogs(logsCdn);

            string logsAgora = GenerateLogsAgora(logs);

            string fileName = Url.Split("/").Last();

            SaveLogs(logsAgora, fileName);

            return fileName;
        }

        private void ValidateUrl()
        {
            if (String.IsNullOrEmpty(Url))
                throw new Exception("The url is empty");

            bool uriIsValid = Uri.TryCreate(Url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!uriIsValid)
                throw new Exception("The url is invalid");
        }

        private void SaveLogs(string logs, string fileName)
        {
            StreamWriter file = new StreamWriter($".\\{fileName}");
            file.Write(logs);
            file.Flush();
            file.Close();
        }

        private string GenerateLogsAgora(IList<Log> logs)
        {
            StringBuilder logAgora = new StringBuilder();
            logAgora.AppendLine($"#Version: 1.0");
            logAgora.AppendLine($"#Date: {DateTime.Now}");
            logAgora.AppendLine($"#Fields: provider http-method status-code uri-path time-taken response - size cache - status");

            foreach(Log log in logs)
            {
                string logLine;
                logLine = $"\"{log.Provider}\" {log.HttpMethod} {log.StatusCode} { log.UriPath } {log.TimeTaken} {log.ResponseSize} {log.CacheStatus}";
                logAgora.AppendLine(logLine);

            }

            return logAgora.ToString();
        }

        private IList<Log> GenerateLogs(string logsCdn)
        {
            IList<Log> logs = new List<Log>();

            string[] logsCdnArray = logsCdn.Split(Environment.NewLine);
            for (int i = 0; i < logsCdnArray.Length - 1; i++)
            {
                string logCdn = logsCdnArray[i];
                string[] logFields = logCdn.Split("|");

                string provider = "MINHA CDN";
                int status = int.Parse(logFields[1]);
                int timeTaken = (int)Math.Round(double.Parse(logFields[4], CultureInfo.InvariantCulture));
                int responseSize = int.Parse(logFields[0]);
                string cacheStatus = logFields[2];

                string[] logHttpFields = logFields[3].Split(" ");
                string httpMethod = logHttpFields[0].Replace("\"", "");
                string uriPath = logHttpFields[1];

                Log log = new Log(provider, httpMethod, status, uriPath, timeTaken, responseSize, cacheStatus);
                logs.Add(log);
            }

            return logs;
        }

        private string DownloadLogsCdn()
        {
            string logsCdn;
            using (WebClient wc = new WebClient())
            {
                logsCdn = wc.DownloadString(Url);
            }

            if (String.IsNullOrEmpty(logsCdn))
                throw new Exception("The file log is empty");

            return logsCdn;
        }
    }
}
