using ECommerce.Api.Customers.Interfaces;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using ECommerce.Api.Customers.Db;
using AutoMapper;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dBContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Helge Stegemoen", Address = "Marviksveien 9" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Tifa Lockhart", Address = "7th Heaven" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Rinoa Heartilly", Address = "Edea's House" });

                dbContext.SaveChanges();
            }
            
        }

        public async Task<(bool IsSuccess, 
            IEnumerable<Models.Customer> Customers, 
            string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if(customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            } 
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return(false, null, ex.Message); ;
            }
        }

        public async Task<(bool IsSuccess,
            Models.Customer Customer,
            string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message); ;
            }
        }
    }
}
