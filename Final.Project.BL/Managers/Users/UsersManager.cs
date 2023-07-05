﻿using Final.Project.DAL;
using System.Collections.Generic;

namespace Final.Project.BL;

public class UsersManager : IUsersManager
{
    private readonly IUnitOfWork _unitOfWork;
    

    public UsersManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region delete user's account
    public bool delete(string id)
    {
        User? user = _unitOfWork.UserRepo.GetById(id);

        if (user is null) { return false; }

        _unitOfWork.UserRepo.Delete(user);
        _unitOfWork.Savechanges();

        return true;
    }
    #endregion

   
    public bool Edit(UserUpdateDto updateDto, User user)
    {
        user.FName = updateDto.FName;

        user.LName = updateDto.LName;


        _unitOfWork.Savechanges();
        return true;

    }

    #region UserOrderDetails
    public UserOrderDetailsDto? GetUserOrderDetailsDto(int id)
    {

        List<OrderProductDetails> OrderProductDetails = _unitOfWork.UserRepo.GetUsersOrderDetails(id).ToList();
        if (OrderProductDetails == null)
        {
            return null;
        }

        IEnumerable<UserOrderProductsDetailsDto> products = OrderProductDetails.Select(p => new UserOrderProductsDetailsDto
        {
            product_Id = p.ProductId,
            Image = p.Product.Image,
            Price = p.Product.Price,
            Quantity = p.Quantity,
            title = p.Product.Name,
          
        });

        UserOrderAddressDetailsDto orderAddress = new UserOrderAddressDetailsDto
        {
            Id = OrderProductDetails[0].Order.UserAddress?.Id,
            City = OrderProductDetails[0].Order.UserAddress?.City,
            Street = OrderProductDetails[0].Order.UserAddress?.Street,
            Phone = OrderProductDetails[0].Order.UserAddress?.Phone,
            DefaultAddress = OrderProductDetails[0].Order.UserAddress?.DefaultAddress,

        };

        UserOrderDetailsDto orderDetails = new UserOrderDetailsDto
        {
            OrderProducts = products,
            OrderAddress = orderAddress
        };


        return orderDetails;

    }
    #endregion

    #region UserOrder
    public IEnumerable<UserOrderDto> GetUserOrdersDto(string id)
    {
        IEnumerable<Order>? ordersFromDB = _unitOfWork.UserRepo.GetUserOrders(id);
        if (ordersFromDB == null) { return null; }
        IEnumerable<UserOrderDto> ordersDto= ordersFromDB.Select(order=> new UserOrderDto
        {
            Id = order.Id,
            OrderStatus = order.OrderStatus,
            DeliverdDate = order.DeliverdDate,
            Products = order.OrdersProductDetails.Select(ip => new UserProductDto
            {
                Image = ip.Product.Image,
                title = ip.Product.Name,
            }

            )

        });
        return ordersDto.ToList();
    }

    #endregion

    #region View user Profile
    
    public UserReadDto GetUserReadDto(User user)
    {

        return new UserReadDto
        {
            Fname = user.FName,
            Lname = user.LName,
            Email = user.Email
        };

    }

    #endregion
}
