using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Accounts;
using WebApi.Models.ItemCategories;
using WebApi.Models.Items;
using WebApi.Models.Points;

namespace WebApi.Models
{
    public class WebApiDataContext : IdentityDbContext<Account>
    {
        public WebApiDataContext(DbContextOptions<WebApiDataContext> opt)
            : base(opt)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; } 
        public DbSet<PointsModel> Points { get; set; } 
        public DbSet<ItemExchangeModel> ItemExchangeLog { get; set; }
    }
}
