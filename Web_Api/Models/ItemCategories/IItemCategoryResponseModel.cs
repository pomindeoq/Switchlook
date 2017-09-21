using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ItemCategories
{
    public interface IItemCategoryResponseModel
    {
        int ItemCategoryId { get; set; }
        string CategoryName { get; set; }
    }
}
