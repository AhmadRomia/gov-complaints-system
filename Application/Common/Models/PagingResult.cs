using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class PagingResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int Count { get; set; }
    }
}
