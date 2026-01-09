using System;
using System.Collections.Generic;

namespace Application.Common.Models
{
    public class PageResultResponseDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? ResultsPerPage { get; set; }
        public int? TotalPages
        {
            get
            {
                if (ResultsPerPage == null || ResultsPerPage == 0) return null;

                return (TotalCount + ResultsPerPage - 1) / ResultsPerPage!.Value;
            }
        }
    }
}
