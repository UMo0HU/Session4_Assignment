using Assignment_2.Data;
using Assignment_2.Models;
using Assignment_2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            this._context = context;
        }
        OrdersCustomersViewModel ordersCustomersViewModel = new OrdersCustomersViewModel();
        OrderOderItemProductViewModel orderOderItemProductViewModel = new OrderOderItemProductViewModel();
        public IActionResult Index()
        {
            ordersCustomersViewModel.orders = _context.Orders.ToList();
            ordersCustomersViewModel.customers = _context.Customers.ToList();
            return View(ordersCustomersViewModel);
        }

        public IActionResult CreateForm()
        {
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            return View();
        }

        public IActionResult CreateOrder(Order order, List<OrderItem> orderItems)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    decimal total = 0;
                    var newOrder = new Order
                    {
                        CustomerId = order.CustomerId,
                        OrderDate = DateTime.Now,
                    };

                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();

                    foreach (var orderItem in orderItems)
                    {
                        orderItem.Product = _context.Products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                        var newOrderItem = new OrderItem
                        {
                            ProductId = orderItem.ProductId,
                            Quantity = orderItem.Quantity,
                            Price = (decimal)(orderItem.Product.Price * orderItem.Quantity),
                            OrderId = newOrder.Id
                        };

                        total += newOrderItem.Price;
                        _context.OrderItems.Add(newOrderItem);
                    }

                    _context.SaveChanges();

                    var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == newOrder.Id);
                    orderToUpdate.TotalAmount = total;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the order: " + ex.Message);
                }
            }
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            return View("CreateForm");
        }

        public IActionResult EditForm(int id)
        {
            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            ViewData["OrderItems"] = order.OrderItems.ToList();
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            return View(order);
        }

        public IActionResult EditOrder(int id, List<OrderItem> orderItems)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    decimal total = 0;
                    var orderToUpdate = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);

                    foreach (var orderItem in orderItems)
                    {
                        orderItem.Product = _context.Products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                        var orderItemToUpdate = orderToUpdate.OrderItems.FirstOrDefault(oi => oi.Id == orderItem.Id && oi.ProductId == orderItem.ProductId);
                        orderItemToUpdate.Quantity = orderItem.Quantity;
                        orderItemToUpdate.Price = (decimal)(orderItem.Product.Price * orderItem.Quantity);
                        total += orderItemToUpdate.Price;

                        _context.OrderItems.Update(orderItemToUpdate);
                    }

                    orderToUpdate.TotalAmount = total;
                    _context.Orders.Update(orderToUpdate);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the order: " + ex.Message);
                }
            }
            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            ViewData["OrderItems"] = order.OrderItems.ToList();
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            return View("EditForm", order);
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);

            orderOderItemProductViewModel.Order = order;
            orderOderItemProductViewModel.orderItems = order.OrderItems.ToList();
            orderOderItemProductViewModel.products = order.OrderItems.Select(oi => oi.Product).ToList();

            return View(orderOderItemProductViewModel);
        }
    }
}
