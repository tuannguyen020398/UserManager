using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Utility
{
    public static class HttpHelper
    {
        private static IHttpContextAccessor _accessor;
        private static readonly HttpClient httpClient=new HttpClient();
        public static HttpContext HttpContext => _accessor?.HttpContext;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }

    }
}
