using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gLibrary.DAL;
using gLibrary.Models;
using gLibrary.ViewModels;

namespace gMenu.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Order(int rid = 0, string tid = null )
        {
            //if (tid == null || rid == 0)
            if (tid == null)
            {            
                return View("Error");
            } 
            else
            {
                //int id = Convert.ToInt32(Encryption.Decrypt(tid, gMnts.TripleDesKeyPrefix));
                int id = Int32.Parse(tid);
                //dishes for this restaurant
                var dishes = _unitOfWork.GetRepository<Dish>().Get(d => d.RestaurantId == rid, includeProperties: "Category").OrderBy(d => d.CategoryId);

                IEnumerable<DishViewModel> dishViewModels = null;
                OrderViewModel orderViewModel;

                //check order from order table get unpaid orders order by order date time
                Order order = _unitOfWork.GetRepository<Order>().Get(o => o.TableId == id).OrderByDescending(o => o.OrderDateTime).FirstOrDefault();

                if (order != null && order.Status == OrderStatus.Ordered)
                {
                    //load order with order details
                    var orderDetails = _unitOfWork.GetRepository<OrderDetail>().Get(d => d.OrderId == order.Id);

                    dishViewModels = from d in dishes
                                     join o in orderDetails on d.Id equals o.DishId
                                     into a
                                     from b in a.DefaultIfEmpty(new OrderDetail())
                                     //existing order
                                     let o = (b.Id != 0)
                                     select new DishViewModel { Dish = d, Quantity = o ? b.Quantity : 1, Ticked = o };

                    orderViewModel = new OrderViewModel() { OrderId = order.Id, RestaurantId = rid, TableId = id, Dishes = dishViewModels.ToList() };

                    //return View("Edit", "_EditLayout", orderViewModel);
                    return View(orderViewModel);
                }

                //check cookie order for this table
                HttpCookie orderCookie = Request.Cookies[gMnts.Cookie_SelectedDishs];

                //load order from cookie if one exists
                if (orderCookie != null)
                {
                    string[] idArray = orderCookie.Value.ToString().Split('&')[0].Split(',');
                    string[] quantityArray = orderCookie.Value.ToString().Split('&')[1].Split(',');

                    dishViewModels = from d in dishes
                                     let i = Array.IndexOf(idArray, d.Id.ToString())
                                     select new DishViewModel { Dish = d, Quantity = i != -1 ? Convert.ToInt32(quantityArray[i]) : 1, Ticked = i != -1 };
                }
                //new order
                else
                {
                    dishViewModels = from d in dishes
                                     select new DishViewModel { Dish = d, Quantity = 1, Ticked = false };
                }

                orderViewModel = new OrderViewModel() { RestaurantId = rid, TableId = id, Dishes = dishViewModels.ToList() };

                return View(orderViewModel);
            }
        }

        //
        // POST: /Order/Create

        [HttpPost]
        public ActionResult Order(OrderViewModel orderViewModel)
        {
            try
            {
                //create the order
                var order = new Order()
                {
                    RestaurantId = orderViewModel.RestaurantId,
                    TableId = orderViewModel.TableId,
                };

                _unitOfWork.GetRepository<Order>().Insert(order);
                _unitOfWork.Save();

                var orderDetails = from d in orderViewModel.Dishes.Where(x => x.Ticked)
                                   select new OrderDetail { DishId = d.Dish.Id, Quantity = d.Quantity, Price = d.Dish.Price, OrderId = order.Id };

                //insert orderdetail
                foreach (OrderDetail o in orderDetails)
                {
                    _unitOfWork.GetRepository<OrderDetail>().Insert(o);
                }

                //save orderdetail
                _unitOfWork.Save();

                //remove cookie for this order
                if (Request.Cookies[gMnts.Cookie_SelectedDishs] != null)
                {
                    HttpCookie orderCookie = new HttpCookie(gMnts.Cookie_SelectedDishs);
                    orderCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(orderCookie);
                }

                return RedirectToAction("OrderDetails", new { oid = order.Id, rid = order.RestaurantId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View(orderViewModel);
        }

        //
        // POST: /Order/Update

        [HttpPost]
        public ActionResult Edit(OrderViewModel orderViewModel)
        {
            try
            {
                int orderId = orderViewModel.OrderId;
                //read all order details for this order
                var orderDetails = _unitOfWork.GetRepository<OrderDetail>().Get(o => o.OrderId == orderId);
                var orderDetailDishIds = orderDetails.Select(o => o.Dish.Id).ToArray();

                int[] selectedDishesArray = orderViewModel.Dishes.Where(o => o.Ticked).Select(o => o.Dish.Id).ToArray();

                if (selectedDishesArray.Count() > 0)
                {
                    //add new dishes  (
                    var dishesToAdd = selectedDishesArray.Except(orderDetailDishIds);

                    OrderDetail orderDetail;

                    foreach (var d in dishesToAdd)
                    {
                        orderDetail = new OrderDetail() { DishId = d, Quantity = orderViewModel.Dishes.Single(s => s.Dish.Id == d).Quantity, Price = orderViewModel.Dishes.Single(s => s.Dish.Id == d).Dish.Price, OrderId = orderId };
                        _unitOfWork.GetRepository<OrderDetail>().Insert(orderDetail);
                    }

                    //remove unwanted dishes
                    var dishesToDelete = orderDetailDishIds.Except(selectedDishesArray);

                    foreach (var d in dishesToDelete)
                    {
                        var orderDetailToDelete = orderDetails.SingleOrDefault(w => w.Dish.Id == d);
                        _unitOfWork.GetRepository<OrderDetail>().Delete(orderDetailToDelete);
                    }

                    //order modification
                    var orderDetailsToUpdate = orderDetailDishIds.Where(o => selectedDishesArray.Contains(o));

                    foreach (int id in orderDetailsToUpdate)
                    {
                        var thisOrderDetail = orderDetails.Single(o => o.DishId == id);
                        int thisNewQuantity = orderViewModel.Dishes.Single(s => s.Dish.Id == id).Quantity;

                        if (thisOrderDetail.Quantity != thisNewQuantity)
                        {
                            thisOrderDetail.Quantity = thisNewQuantity;
                            _unitOfWork.GetRepository<OrderDetail>().Update(thisOrderDetail);
                        }

                    }
                }
                else //no dish selected, cancel order?
                {
                    //remove order details
                    foreach (OrderDetail o in orderDetails)
                    {
                        _unitOfWork.GetRepository<OrderDetail>().Delete(o);
                    }
                    _unitOfWork.Save();

                    //remove order
                    _unitOfWork.GetRepository<Order>().Delete(orderId);
                    _unitOfWork.Save();
                }

                //save orderdetail
                _unitOfWork.Save();

                return RedirectToAction("OrderDetails", new { oid = orderId, rid = orderViewModel.RestaurantId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View(orderViewModel);
        }

        public ActionResult OrderDetails(int rid = 0, int oid = 0)
        {
            var orderListModel = new OrderListModel()
            {
                Order = _unitOfWork.GetRepository<Order>().GetByID(oid),
                OrderDetails = _unitOfWork.GetRepository<OrderDetail>().Get(o => o.OrderId == oid).ToList()
            };

            return View(orderListModel);
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int rid, int did)
        {
            var dish = _unitOfWork.GetRepository<Dish>().GetByID(did);

            return View(dish);
        }

        //public PartialViewResult RestaurantProfile(int rid = 0)
        //{
        //    Restaurant restaurant = _unitOfWork.GetRepository<Restaurant>().GetByID(rid);

        //    if (restaurant == null)
        //    {
        //        restaurant = new Restaurant()
        //        {
        //            Bilingual = new Bilingual()
        //            {
        //                Name = "gMenu",
        //                AlienName = "gMenu"
        //            },
        //            AddressId = 1
        //        };
        //    }

        //    return PartialView("_restaurantProfile", restaurant);
        //}

        public PartialViewResult PlaceOrder(int id)
        {
            var order = _unitOfWork.GetRepository<Order>().GetByID(id);

            return PartialView();
        }

        public ActionResult Contact()
        {
            return View("Contact", "_ContactLayout");
        }
    }
}
