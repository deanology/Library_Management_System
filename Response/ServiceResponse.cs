using System;
namespace Library_Management_System.Response
{
    public class ServiceResponse
    {
        public ServiceResponse(string responseCode, string responseDescription)
        {
            ResponseCode = responseCode;
            ResponseDescription = responseDescription;
        }

        public ServiceResponse(string responseCode)
        {
            ResponseCode = responseCode;
        }

        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
