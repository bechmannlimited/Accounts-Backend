using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts_IOU
{
    public class Response
    {
        public ResponseStatus Status { get; set; }
        public string Message = "";
    }

    public class ResponseDictionary : Dictionary<String, object>
    {

        public ResponseDictionary(Response response)
        {
            this["Response"] = response;
        }

        public ResponseDictionary(ResponseStatus status)
        {
            Response response = new Response
            {
                Status = status
            };
            this["Response"] = response;
        }
    }

    public enum ResponseStatus
    {
        Failed = 0,
        Success = 1
    }


    //

    //public class ResponseObject
    //{
    //    public ResponseStatus Status { get; set; }
    //    public string Message { get; set; }
    //    public DateTime ServerTime { get; set; }
    //    public ResponseDictionary ResponseDictionary {get; set; }
    //    public ResponseObject()
    //    {
    //        Status = ResponseStatus.Failed;
    //        Message = "";
    //        ServerTime = System.DateTime.Now;
    //        ResponseDictionary = new ResponseDictionary(ResponseStatus.Failed);
    //    }
    //}
}