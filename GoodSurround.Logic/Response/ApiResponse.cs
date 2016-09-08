using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.Logic.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        public ApiResponse(string errorMessage)
        {
            Ok = false;
            ErrorMessage = errorMessage;
        }

        public ApiResponse(T data)
        {
            Ok = true;
            Data = data;
        }

        public ApiResponse(bool ok, string errorMessage)
        {
            Ok = ok;
            ErrorMessage = errorMessage;
        }

        public ApiResponse(bool ok, string errorMessage, T data)
        {
            Ok = ok;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
