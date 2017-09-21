using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.ItemCategories
{
    public class ItemCategoryResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public IItemCategoryResponseModel ItemCategory { get; set; }
    }
}
