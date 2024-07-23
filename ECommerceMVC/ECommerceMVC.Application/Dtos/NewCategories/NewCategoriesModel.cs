using ECommerceMVC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos.NewCategories
{
    public class NewCategoriesModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public int IdNewParent { get; set; }

        public string NewParentName { get; set; } = null!;

        public List<DbNew>? DbNews { get; set; } = new List<DbNew>();
    }
}
