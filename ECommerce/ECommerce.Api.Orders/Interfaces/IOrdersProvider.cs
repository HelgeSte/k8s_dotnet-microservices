using ECommerce.Api.Orders.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSuccess, Order Order, string ErrorMessage)> GetOrderAsync(int id);

    }
}
