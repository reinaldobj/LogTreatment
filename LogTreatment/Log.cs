namespace LogTreatment
{
    public class Log
    {
        public Log(string provider, string httpMethod, int status, string uriPath, int timeTaken, int responseSize, string cacheStatus)
        {
            HttpMethod = httpMethod;
            StatusCode = status;
            UriPath = uriPath;
            TimeTaken = timeTaken;
            ResponseSize = responseSize;
            CacheStatus = cacheStatus;
            Provider = provider;
        }

        public string Provider  { get; set; }

        public string HttpMethod { get; set; }

        public int StatusCode { get; set; }

        public string UriPath { get; set; }

        public int TimeTaken { get; set; }

        public int ResponseSize { get; set; }

        public string CacheStatus { get; set; }


    }
}
