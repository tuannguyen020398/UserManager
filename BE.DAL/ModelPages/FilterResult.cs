using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.ModelPages
{
    public class FilterResult<TEntity>: PagedResultBase
    {
        public int TotalRows { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
        public List<TEntity> ItemsData{ set; get; }
    }
}
