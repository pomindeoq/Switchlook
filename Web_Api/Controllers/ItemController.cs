using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Accounts;
using WebApi.Models.ItemCategories;
using WebApi.Models.Items;
using WebApi.Models.Response;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Item")]
    public class ItemController : Controller
    {
        private WebApiDataContext _context;
        private readonly UserManager<Account> _userManager;

        public ItemController(WebApiDataContext context, UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet, Route("getItems")]
        public async Task<IResponse> GetItems()
        {
            ItemsResponse itemsResponse = new ItemsResponse();
            var itemsz = await _context.Items
                .Include(x => x.Category)
                .Include(x => x.OwnerAccount)
                .ToListAsync();

            IEnumerable<IItemResponseModel> item = itemsz.Select(x => new ItemResponseModel
            {
                ItemId = x.Id,
                CategoryName = x.Category.Name,
                OwnerUserName = x.OwnerAccount.UserName,
                PointValue = x.PointValue
            });

            itemsResponse.Items = item;

            return itemsResponse;
        }

        [AllowAnonymous]
        [HttpGet, Route("getItem/id={id}")]
        public async Task<IResponse> GetItem(int id)
        {
            ItemResponse itemResponse = new ItemResponse();
            Item item = await _context.Items
                .Include(x => x.Category)
                .Include(x => x.OwnerAccount)
                .SingleAsync(x => x.Id == id);

            IItemResponseModel itemReturn = new ItemResponseModel()
            {
                ItemId = item.Id,
                CategoryName = item.Category.Name,
                OwnerUserName = item.OwnerAccount.UserName,
                PointValue = item.PointValue
            };

            itemResponse.Item = itemReturn;

            return itemResponse;
        }

        [AllowAnonymous]
        [HttpPost, Route("createItem")]
        public async Task CreateItem([FromBody] CreateItemModel createItemModel)
        {
            Item item = new Item();
            item.OwnerAccount = await _userManager.FindByIdAsync(createItemModel.UserId);
            item.Category = await _context.ItemCategories.SingleAsync(x => x.Id == item.Id);
            item.PointValue = createItemModel.PointValue;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        [HttpPost, Route("createItemCategory")]
        public async Task CreateItemCategory([FromBody] CreateItemCategoryModel itemCategoryModel)
        {
            ItemCategory itemCategory = new ItemCategory();
            itemCategory.Name = itemCategoryModel.Name;
            _context.ItemCategories.Add(itemCategory);
            await _context.SaveChangesAsync();
        }
      
        [Authorize]
        [HttpGet, Route("getItemCategories")]
        public async Task<IResponse> GetItemCategories()
        {
            ItemCategoriesResponse itemCategoriesResponse = new ItemCategoriesResponse();
            var itemCategories = await _context.ItemCategories.ToListAsync();

            IEnumerable<IItemCategoryResponseModel> itemCategoriesReturn = itemCategories.Select(x => new ItemCategoryResponseModel
            {
                ItemCategoryId = x.Id,
                CategoryName = x.Name
            });

            itemCategoriesResponse.ItemCategories = itemCategoriesReturn;

            return itemCategoriesResponse;
        }

        [Authorize]
        [HttpGet, Route("getItemCategory/id={id}")]
        public async Task<IResponse> GetItemCategroy(int id)
        {
            ItemCategoryResponse itemCategoryResponse = new ItemCategoryResponse();
            ItemCategory itemCategory = await _context.ItemCategories.SingleAsync(x => x.Id == id);

            IItemCategoryResponseModel itemCategoryReturn = new ItemCategoryResponseModel()
            {
                ItemCategoryId = itemCategory.Id,
                CategoryName = itemCategory.Name
            };

            itemCategoryResponse.ItemCategory = itemCategoryReturn;

            return itemCategoryResponse;
        }

    }
}