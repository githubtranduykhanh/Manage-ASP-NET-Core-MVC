using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.DataTable
{
    public class Search
    {
        public string Value { get; set; } = string.Empty;
        public bool Regex { get; set; } 
    }
}
