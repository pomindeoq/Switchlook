using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

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

        [Authorize]
        [HttpGet, Route("getAllItems")]
        public async Task<List<Item>> GetItems()
        {
            List<Item> items = await _context.Items.Include(x => x.Category).ToListAsync();
            return items;
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

        [Authorize]
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