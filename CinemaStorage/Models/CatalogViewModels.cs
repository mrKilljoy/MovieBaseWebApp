using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaStorage.Models
{
    public class CatalogViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public int RecordsPerPage { get; set; }

        public Movie[] MoviesList { get; set; }
    }
}