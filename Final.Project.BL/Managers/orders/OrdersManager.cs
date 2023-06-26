﻿using Final.Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Project.BL;

public class OrdersManager : IOrdersManager
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region Add Order

    public void AddNewOrder(string userId, int addressId)
    {
        //1-Add new order in order table
        Order newOrder = new Order
        {
            OrderStatus = OrderStatus.Pending,
            OrderDate = DateTime.Now,
            UserId = userId,
            //AddressId = addressId
        };
        _unitOfWork.OrderRepo.Add(newOrder);
        _unitOfWork.Savechanges();
        //2-we need the orderId of the new order to use it in orderdetails table
        //so lets get that 
        int LastOrderId = _unitOfWork.OrderRepo.GetLastUserOrder(userId);

        //3-transfer products from cart to order 
        IEnumerable<UserProductsCart> productsFromCart = _unitOfWork.UserProdutsCartRepo.GetAllProductsByUserId(userId);

        //4-we need  productId,OrderId,Quantity for each row in orderDetails table
        //so lets extract this data from products we got from cart and insert them
        // to userProductsDetails Table

        var orderProducts = productsFromCart.Select(p => new OrderProductDetails
        {
            OrderId = LastOrderId,
            ProductId = p.ProductId,
            Quantity = p.Quantity
        });

        _unitOfWork.OrdersDetailsRepo.AddRange(orderProducts);

        //5-Make The UserCart Empty

        _unitOfWork.UserProdutsCartRepo.DeleteAllProductsFromUserCart(userId);
        _unitOfWork.Savechanges();
    }

    #endregion

    #region Get all Orders

    public IEnumerable<OrderReadDto> GetAllOrders()
    {
        var orderFromDb = _unitOfWork.OrderRepo.GetAll();
        var orderReadDto = orderFromDb
            .Select(o => new OrderReadDto
            {
                Id = o.Id,
                OrderStatus = o.OrderStatus,
                OrderDate = o.OrderDate,
                DeliverdDate = o.DeliverdDate,
                UserName = (o.User.FName + " " + o.User.LName),
            });

        return orderReadDto;
    }

    #endregion

    #region Get Order Details

    public OrderDetailsDto GetOrderDetails(int OrderId)
    {
        Order order = _unitOfWork.OrderRepo.GetOrderWithProducts(OrderId);

        OrderDetailsDto orderDetails = new OrderDetailsDto
        {
            Id = order.Id,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate,
            DeliverdDate = order.DeliverdDate,
            UserName = (order.User.FName + " " + order.User.LName),
            ProductsInOrder = order.OrdersProductDetails.Select(op => new ProductsInOrder
            {
                Quantity = op.Quantity,
                ProductName = op.Product.Name,
                ProductDescription = op.Product.Description,
                ProductPrice = op.Product.Price,
                ProductImage = op.Product.Image,
                ProductModel = op.Product.Model,
            })

        };

        return orderDetails;
    }

    #endregion

    #region Update Order

    public bool UpdateOrder(OrderEditDto orderEdit)
    {
        var order = _unitOfWork.OrderRepo.GetById(orderEdit.Id);
        if (order is null)
        {
            return false;
        }

        order.OrderStatus = orderEdit.OrderStatus;
        order.OrderDate = orderEdit.OrderDate;
        order.DeliverdDate = orderEdit.DeliverdDate;
        order.UserId = orderEdit.UserId;

        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Delete Order

    public bool DeleteOrder(int Id)
    {
        var order = _unitOfWork.OrderRepo.GetById(Id);
        if (order is null)
        {
            return false;
        }

        _unitOfWork.OrderRepo.Delete(order);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion
}
