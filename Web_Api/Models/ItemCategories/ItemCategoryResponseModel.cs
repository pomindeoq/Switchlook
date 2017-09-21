using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ItemCategories
{
    public class ItemCategoryResponseModel : IItemCategoryResponseModel
    {
        public int ItemCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
