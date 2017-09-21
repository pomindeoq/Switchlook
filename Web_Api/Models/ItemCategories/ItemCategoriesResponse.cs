using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.ItemCategories
{
    public class ItemCategoriesResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<IItemCategoryResponseModel> ItemCategories { get; set; }
    }
}
