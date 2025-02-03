
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Shared
{
    public class Response<T> 
    {
        public List<Error> Errors = new List<Error>();
        public bool IsSuccess => !Errors.Any();
        public T Data { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
