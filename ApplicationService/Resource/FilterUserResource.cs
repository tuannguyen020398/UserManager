using ApplicationService.Model.UserModel;
using BE.DAL.Enums;
using BE.DAL.ModelPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Resource
{
    public class FilterUserResource: PagingParam<UserModelPading>
    {
        public string? Keywork { get; set; }
        public GtStatus ? Count { get; set; }
        public override List<Expression<Func<UserModelPading, bool>>> GetPredicates()
        {
            var filter = base.GetPredicates();
            if (!string.IsNullOrEmpty(Keywork))
            {
                filter.Add(x => x.Name.ToLower().Contains(Keywork.ToLower()));
            }
            if (Count!=null)
            {
                filter.Add(x => x.Gt==Count);
            }

            return filter;
        }
    }
}
