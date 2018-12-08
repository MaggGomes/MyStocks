using System.Net;

namespace MyStock.Client
{
    class State
    {
        public HttpStatusCode Status { get; set; }
        public string Response { get; set; }
        public WebRequest Request { get; set; }
    }
}
