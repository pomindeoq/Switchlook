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
using WebApi.Models.Points;
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
        public async Task<IActionResult> CreateItem([FromBody] CreateItemModel createItemModel)
        {
            Item item = new Item();
            item.OwnerAccount = await _userManager.FindByIdAsync(createItemModel.UserId);
            item.Category = await _context.ItemCategories.SingleAsync(x => x.Id == createItemModel.CategoryId);
            item.PointValue = createItemModel.PointValue;

            AddPoints addPoints = new AddPoints(_context);
            await addPoints.ToUserAsync(item.OwnerAccount, item.PointValue);

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            dynamic response = new
            {
                itemId = item.Id
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost, Route("createItems")]
        public async Task<IResponse> CreateItems([FromBody] CreateItemsModel createItemsModels)
        {
            List<int> itemIds = new List<int>();
            foreach (double pointValue in createItemsModels.PointValues)
            {
                Item item = new Item();
                item.OwnerAccount = await _userManager.FindByIdAsync(createItemsModels.UserId);
                item.Category = await _context.ItemCategories.SingleAsync(x => x.Id == createItemsModels.CategoryId);
                item.PointValue = pointValue;

                AddPoints addPoints = new AddPoints(_context);
                await addPoints.ToUserAsync(item.OwnerAccount, item.PointValue);

                _context.Items.Add(item);
                itemIds.Add(item.Id);
            }

            
            await _context.SaveChangesAsync();

            CreateItemsResponse createItemsResponse = new CreateItemsResponse
            {
                ItemIds = itemIds
            };

            return createItemsResponse;
        }



        [AllowAnonymous]
        [HttpPost, Route("createItemCategory")]
        public async Task CreateItemCategory([FromBody] CreateItemCategoryModel itemCategoryModel)
        {
            ItemCategory itemCategory = new ItemCategory();
            itemCategory.Name = itemCategoryModel.Name;
            _context.ItemCategories.Add(itemCategory);
            await _context.SaveChangesAsync();
        }
      
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost, Route("exchangeItem")]
        public async Task<IResponse> ExchangeItem([FromBody] ItemExchangeViewModel itemExchangeViewModel)
        {
            ItemExchangeResponse itemExchangeResponse = new ItemExchangeResponse();
            List<string> errors = new List<string>();

            Item item = await _context.Items.Include(x=>x.Category).Include(x=>x.OwnerAccount).FirstOrDefaultAsync(x => x.Id == itemExchangeViewModel.ItemId);
            if (item != null)
            {
                Account newOwnerAccount =
                    await _userManager.FindByIdAsync(itemExchangeViewModel.NewOwnerAccountId);
                if (newOwnerAccount != null)
                {
                    PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account == newOwnerAccount);
                    if (points.Value >= itemExchangeViewModel.PointValue)
                    {
                        AddPoints addPoints = new AddPoints(_context);
                        await addPoints.ToUserAsync(newOwnerAccount, -itemExchangeViewModel.PointValue);

                        Account oldOwnerAccount = item.OwnerAccount;
                        item.OwnerAccount = newOwnerAccount;

                        _context.Items.Update(item);
                        await _context.SaveChangesAsync();

                        ItemExchangeModel itemExchangeModel = new ItemExchangeModel
                        {
                            Item = item,
                            OldOwnerAccount = oldOwnerAccount,
                            NewOwnerAccount = item.OwnerAccount,
                            PointValue = itemExchangeViewModel.PointValue
                        };

                        ItemExchange itemExchange = new ItemExchange(_context);
                        itemExchange.Set(itemExchangeModel);
                        itemExchangeResponse = await itemExchange.CommitAsync();
                    }
                    else
                    {
                        errors.Add($"Not enough points for user :{newOwnerAccount.UserName}");
                    }  
                }
                else
                {
                    errors.Add("User not found.");
                }
            }
            else
            {
                errors.Add("Item not found.");
            }
            itemExchangeResponse.Errors = errors;
            return itemExchangeResponse;
        }

        [AllowAnonymous]
        [HttpGet, Route("deleteItem/id={id}")]
        public async Task DeleteItem(int id)
        {
            List<string> errors = new List<string>();
            var item = _context.Items.Find(id);
            if (item != null)
            {
              _context.Items.Remove(item);
              await  _context.SaveChangesAsync();            
            }
            else
            {
                errors.Add("Item not found");             
            }
        }

    }
}