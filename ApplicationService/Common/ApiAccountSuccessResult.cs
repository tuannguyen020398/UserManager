using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Common
{
    public class ApiAccountSuccessResult<T> : ApiResult<T>
    {
        public ApiAccountSuccessResult(T resultObj, Guid id)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
            Id = id;
        }

        public ApiAccountSuccessResult()
        {
            IsSuccessed = true;

        }
    }
}
