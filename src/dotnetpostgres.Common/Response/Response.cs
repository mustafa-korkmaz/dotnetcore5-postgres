using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetpostgres.Common.Response
{
    /// <summary>
    /// response  base class
    /// </summary>
    public class Response
    {
        public string ErrorCode { get; set; }
        public ResponseType Type { get; set; }
    }
}
