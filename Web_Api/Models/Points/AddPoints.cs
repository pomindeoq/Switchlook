using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class AddPoints
    {
        private readonly WebApiDataContext _context;
        public AddPoints(WebApiDataContext context)
        {
            _context = context;
        }

        public async Task ToUserAsync(Account account, double pointValue)
        {
            PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account == account);
            if (points != null)
            {
                points.Value = points.Value + pointValue;
                _context.Points.Update(points);
            }
        }
    }
}
