using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }

        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _PageSize { get; set; } = 6;
        public int pageSize
        {
            get => _PageSize;
            set => _PageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        private string _search;
        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
