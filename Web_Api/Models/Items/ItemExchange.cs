using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;
using WebApi.Models.Response;

namespace WebApi.Models.Items
{
    public class ItemExchange
    {
        private readonly WebApiDataContext _context;
        public ItemExchange(WebApiDataContext context)
        {
            _context = context;
        }

        public ItemExchangeModel ItemExchangeModel { get; set; }

        public void Set(ItemExchangeModel itemExchangeModel)
        {
            ItemExchangeModel = itemExchangeModel;
        }

        public async Task<ItemExchangeResponse> CommitAsync()
        {
            ItemExchangeResponse itemExchangeResponse = new ItemExchangeResponse();
            List<string> errors = new List<string>();
            if (ItemExchangeModel != null)
            {
                _context.ItemExchangeLog.Add(ItemExchangeModel);
                await _context.SaveChangesAsync();
                itemExchangeResponse.Succeeded = true;
            }
            else
            {
                errors.Add("ItemExchangeModel not set.");
            }
            itemExchangeResponse.Errors = errors;
            return itemExchangeResponse;
        }
    }
}
