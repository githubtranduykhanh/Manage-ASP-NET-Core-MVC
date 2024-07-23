using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos
{
    public class ResponseService<T>
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public T? data { get; set; } = default; // Sử dụng new T() để khởi tạo đối tượng T
        public object[] errors { get; set; } = new object[] { };
        public IEnumerable<string> EnumErrors { get; set; } = new string[] { };
    }

    public class ResponseService
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; } = new object { };
        public object[] errors { get; set; } = new object[] { };
        public IEnumerable<string> EnumErrors { get; set; } = new string[] { };
    }
}
