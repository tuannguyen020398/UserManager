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
    /// <summary>filter danh sách theo keywork và thuộc tính</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class FilterUserResource: PagingParam<UserModelPading>
    {
        public string? Keywork { get; set; }
        public SexStatus ? Count { get; set; }
        public DateTime? StartDob { get; set; }
        public DateTime? EndDob{ get; set; }
        public override List<Expression<Func<UserModelPading, bool>>> GetPredicates()
        {

            var filter = base.GetPredicates();

            // filter theo keywwork
            if (!string.IsNullOrEmpty(Keywork))
            {
                filter.Add(x => x.Name!.ToLower().Contains(Keywork.ToLower()) || x.Email!.ToLower().Contains(Keywork.ToLower()));                
            }
            //
            //filter theo giới tính
            if (Count != null)
            {
                filter.Add(x => x.Sex == Count);
            }
            //filter date dob
            if (StartDob.HasValue)
            {
                filter.Add(x => x.Dob >= StartDob.Value);
            }
            if (EndDob.HasValue)
            {
                filter.Add(x => x.Dob <= EndDob.Value);
            }
            filter.Add(x => x.Status == 0);

            return filter;
        }
    }
}
