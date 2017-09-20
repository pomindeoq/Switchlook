using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Models.Response.Item;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Item")]
    public class ItemController : Controller
    {
        private WebApiDataContext _context;

        public ItemController(WebApiDataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet, Route("getItems")]
        public async Task<IResponse> GetItems()
        {
            ItemsResponse itemsResponse = new ItemsResponse();
            var itemsz = await _context.Items
                .Include(x => x.Category)
                .Include(x => x.OwnerAccount).ToListAsync();

            IEnumerable<dynamic> item = itemsz.Select(x => new
            {
                ItemId = x.Id,
                CategoryName = x.Category.Name,
                OwnerUserName = x.OwnerAccount.UserName,
                PointValue = x.PointValue


            });

            itemsResponse.Items = item;

            return itemsResponse;
        }

        [Authorize]
        [HttpGet, Route("getItem/id={id}")]
        public async Task<Item> GetItem(int id)
        {
            Item item = await _context.Items.SingleAsync(x => x.Id == id);
            return item;
        }

        [Authorize]
        [HttpPost, Route("createItem")]
        public async Task CreateItem([FromBody] Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        [HttpPost, Route("createItemCategory")]
        public async Task CreateItemCategory([FromBody] ItemCategory itemCategory)
        {
            _context.ItemCategories.Add(itemCategory);
            await _context.SaveChangesAsync();
        }

        [AllowAnonymous]
        [HttpGet, Route("getAllItemCategories")]
        public async Task<List<ItemCategory>> GetAllItemCategories()
        {
            List<ItemCategory> itemCategories = await _context.ItemCategories.ToListAsync();
            return itemCategories;
        }

        [Authorize]
        [HttpGet, Route("getItemCategory/id={id}")]
        public async Task<ItemCategory> GetItemCategroy(int id)
        {
            ItemCategory result = await _context.ItemCategories.SingleAsync(x => x.Id == id);
            return result;
        }

    }
}