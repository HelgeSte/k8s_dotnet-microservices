using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<OrdersProvider> logger;

        public OrdersProvider(OrdersDbContext dbContext, IMapper mapper, ILogger<OrdersProvider> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Order { Id = 1, CustomerId = 1, OrderDate = "19.05.2023", Total = 1 });
                dbContext.Orders.Add(new Order { Id = 2, CustomerId = 2, OrderDate = "19.12.2023", Total = 10 });
                dbContext.Orders.Add(new Order { Id = 3, CustomerId = 1, OrderDate = "09.02.2023", Total = 4 });
                dbContext.Orders.Add(new Order { Id = 4, CustomerId = 3, OrderDate = "19.05.2023", Total = 23 });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id)
        {
            try
            {
                var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    var result = mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync()
        {
            try
            {
                var orders = await dbContext.Orders.ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

      
    }
}
