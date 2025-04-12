
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Shared
{
    /// <summary>
    /// A generic class to return a structured response from endpoints
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResponse<T> 
    {
        public List<Error> Errors { get; set; } = new List<Error>();
        public bool IsSuccess => !Errors.Any();
        public T? Data { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
