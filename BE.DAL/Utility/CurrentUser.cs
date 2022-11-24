using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Utility
{
    public static class CurrentUser
    {
        public static long Id
        {
            get
            {
                if (HttpHelper.HttpContext == null)
                {
                    return 0L;
                }

                return ConvertUtility.ConvertToLong(HttpHelper.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            }
        }
        public static DateTime UtcNow
        {
            get
            {
                DateTime utcNow = DateTime.UtcNow;
                return DateTime.SpecifyKind(utcNow, DateTimeKind.Unspecified);
            }
        }

        public static DateTime UtcNowWithKind
        {
            get
            {
                DateTime utcNow = DateTime.UtcNow;
                return DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
            }
        }

        public static DateTime Now
        {
            get
            {
                DateTime now = DateTime.Now;
                return DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
            }
        }

    }
}
