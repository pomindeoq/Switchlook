using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Points> Points { get; set; }
    }
}
