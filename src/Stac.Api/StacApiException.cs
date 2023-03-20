using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac.Api
{
    public partial class StacApiException : System.Exception
    {

        public int StatusCode { get; protected set; }

        public string Response { get; protected set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public StacApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public StacApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }

        public override string Message
        {
            get
            {
                return base.Message + "\n\nStatus: " + StatusCode + "\nResponse: \n" + ((Response == null) ? "(null)" : Response.Substring(0, Response.Length >= 1024 ? 1024 : Response.Length));
            }
        }
    }

    public partial class StacApiException<TResult> : StacApiException
    {

        public StacApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
        : base(message, statusCode, response, headers, innerException)

        {
            Response = JsonConvert.SerializeObject(result);
        }
    }
}
