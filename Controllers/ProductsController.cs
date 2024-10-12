using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patatzaak.Data;
using Patatzaak.Models;
using Patatzaak.ViewModels;

namespace Patatzaak.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FrituurDb _context;

        public ProductsController(FrituurDb context)
        {
            _context = context;
        }
        public async Task<IActionResult> AddProductToOrder(int? orderNr)
        {
            var products = await _context.Products.ToListAsync();
            if (orderNr != null && orderNr != 0)
            {
                var o = await _context.Orders.Where(o => o.OrderNr == orderNr).FirstOrDefaultAsync();
                var items = await _context.OrderItems
                .Where(oi => oi.OrderNr == o.OrderNr)
                .ToListAsync();

                List<Product>? productsInOrder = new List<Product>();



                foreach (OrderItem orderItem in items)
                {
                    productsInOrder.Add(await _context.Products.Where(p => p.Id == orderItem.ProductId).FirstOrDefaultAsync());
                }

                ProductOrder po = new ProductOrder()
                {
                    Products = products,
                    Items = items,
                    Order = o,
                    ProductsInOrder = productsInOrder
                };

                return View(po);
            }
            else
            {
                var o = new Order()
                {
                    OrderNr = 0,
                    OrderedOn = DateTime.Now,
                    State = "Incomplete"
                };
                if (o == null)
                {
                    return NotFound();
                }
                _context.Add(o);
                await _context.SaveChangesAsync();
                var items = await _context.OrderItems
                .Where(oi => oi.OrderNr == o.OrderNr)
                .ToListAsync();

                List<Product>? productsInOrder = new List<Product>();



                foreach (OrderItem orderItem in items)
                {
                    productsInOrder.Add(await _context.Products.Where(p => p.Id == orderItem.ProductId).FirstOrDefaultAsync());
                }
                ProductOrder po = new ProductOrder()
                {
                    Products = products,
                    Items = items,
                    Order = o,
                    ProductsInOrder = productsInOrder
                };

                return View(po);
            }

        }
        //GET
        public async Task<IActionResult> AddProduct(int? productId, int? orderNr)
        {
            return View();
        }
        //POST
        [HttpPost]
        public async Task<IActionResult> AddProduct([Bind("OrderNr, ProductId,Amount")] OrderItem oi)
        {
            if (OrderItemExists(oi.OrderNr, oi.ProductId) == true)
            {
                var orderItem = await _context.OrderItems.Where(item => item.ProductId == oi.ProductId && item.OrderNr == oi.OrderNr).FirstOrDefaultAsync();
                orderItem.Amount = orderItem.Amount + oi.Amount;

                _context.Update(orderItem);
                await _context.SaveChangesAsync();

                return RedirectToAction("AddProductToOrder", "Products", oi);
            }
            if (ModelState.IsValid)
            {
                _context.Add(oi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddProductToOrder", "Products", oi);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Sale,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Sale,Stock")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        private bool OrderItemExists(int orderNr, int productId)
        {
            return _context.OrderItems.Any(oi => oi.ProductId == productId && oi.OrderNr == orderNr);
        }
    }
}
