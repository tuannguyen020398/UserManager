using ApplicationService.Model.UserModel;
using BE.DAL.ModelPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Resource
{
    public class FilterResource : PagingParam<UserModelPading>
    {
        public override List<Expression<Func<UserModelPading, bool>>> GetPredicates()
        {
            var filter = base.GetPredicates();
            //filter.Add(c => c.Status == 7);
            return filter;
        }
    }
}
