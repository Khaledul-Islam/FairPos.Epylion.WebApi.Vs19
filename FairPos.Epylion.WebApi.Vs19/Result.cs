using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19
{
    public class Result
    {
        public object Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
