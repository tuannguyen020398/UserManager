using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Common
{
    public class ApiAccountSuccessResult<T> : ApiResult<T>
    {
        /// <summary>return api successResult</summary>
        /// <param name="resultObj">The result object.</param>
        /// <param name="id">The identifier.</param>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public ApiAccountSuccessResult(T resultObj, long id)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
            Id= id;
        }
        //them
        public ApiAccountSuccessResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        public ApiAccountSuccessResult()
        {
            IsSuccessed = true;

        }
    }
}
