using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.VkModels
{
    public class Error
    {
        public int error_code { get; set; }
        public string error_msg { get; set; }

        public Param[] request_params { get; set; }
    }

    public class Param
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
