using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Database.EFCore.Response
{
    public class ResultPage
    {
        public ResultPage() { }
        public ResultPage(int totalRows, int totalPages, int currentPage, int pageSize, object data)
        {
            TotalRows = totalRows;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Data = data;
        }
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public object Data { get; set; }
    }
}
