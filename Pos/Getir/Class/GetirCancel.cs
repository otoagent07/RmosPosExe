using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Getir.Class
{
    public class GetirCancel
    {
        public class Root
        {
            public string id { get; set; }
            public string message { get; set; }
        }

        public class PostCancelRequest
        {
            public string cancelNote { get; set; }
            public string cancelReasonId { get; set; }
        } 
        public class PostCancelResponse
        {
            public string microservice { get; set; }
            public int code { get; set; }
            public string error { get; set; }
            public string message { get; set; }
        }
    }
}
