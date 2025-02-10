using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Shared
{
    /// <summary>
    /// A generic class to return a response from a service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
    
        public ServiceResult(T data)
        {
            IsSuccess = true;
            Data = data;
            StatusCode = 200;
        }
        public ServiceResult(string message,int statusCode)
        {
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
        }
    }
}
