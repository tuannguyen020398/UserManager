using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.ModelPages
{
    public class PagingParam<Entity>
    {
        public string SortExpression { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Skip { get; set; }

        public int NotSkip { get; set; }

        public virtual List<Expression<Func<Entity, bool>>> GetPredicates()
        {
            return new List<Expression<Func<Entity, bool>>>();
        }
    }
}
