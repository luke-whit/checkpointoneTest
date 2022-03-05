using System;
using System.IO;
using System.Net;

namespace CheckpointTest
{
    public enum httpverb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    internal class RestClient
    {
        public string endpoint { get; set; }
        public httpverb httpmethod { get; set; }

        public RestClient()
        {
            endpoint = string.Empty;
            httpmethod = httpverb.GET;
        }

        public string makeRequest()
        {
            string strResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = httpmethod.ToString();
            request.Headers["x-auth-token"] = "bhvmuh1qm2beh6wdybducwp5esi1tv9";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException(response.StatusCode.ToString());
                }
                
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using(StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponse = reader.ReadToEnd();
                        }
                    }
                }
            }

            return strResponse;
        }
    }
}
