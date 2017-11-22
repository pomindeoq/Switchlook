using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Accounts;
using WebApi.Models.Points;

namespace WebApi.Mangers
{
    public class PointManager
    {
        private readonly WebApiDataContext _context;
        public PointManager(WebApiDataContext context)
        {
            _context = context;
        }

        public async Task<(bool succeeded, PointTransactionModel pointTransaction)> AddToUserAsync(Account account, double pointValue)
        {
            if (pointValue > 0.0)
            {
                PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account == account);
                if (points != null)
                {
                    PointTransactionModel pointTransactionModel = new PointTransactionModel();
                    pointTransactionModel.Account = account;
                    pointTransactionModel.PreviousAmount = points.Value;
                    pointTransactionModel.NewAmount = pointValue + points.Value;
                    pointTransactionModel.DateTime = DateTime.Now;

                    _context.PointTransactionLog.Add(pointTransactionModel);

                    points.Value = points.Value + pointValue;
                    _context.Points.Update(points);

                    await _context.SaveChangesAsync();

                    return (true, pointTransactionModel);
                }
            }
            return (false, null);
        }

        public async Task<(bool succeeded, PointTransactionModel pointTransaction)> RemoveFromUserAsync(Account account,
            double pointValue)
        {
            if (pointValue > 0.0)
            {
                PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account == account);
                if (points != null)
                {
                    if (points.Value != 0.0 || points.Value - pointValue >= 0.0)
                    {
                        PointTransactionModel pointTransactionModel = new PointTransactionModel();
                        pointTransactionModel.Account = account;
                        pointTransactionModel.PreviousAmount = points.Value;
                        pointTransactionModel.NewAmount = pointValue - points.Value;
                        pointTransactionModel.DateTime = DateTime.Now;

                        _context.PointTransactionLog.Add(pointTransactionModel);

                        points.Value = points.Value - pointValue;
                        _context.Points.Update(points);

                        await _context.SaveChangesAsync();

                        return (true, pointTransactionModel);
                    }
                }
            }
            return (false, null);
        }
    }
}
