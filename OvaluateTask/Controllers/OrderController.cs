using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OvaluateTask.Data;
using OvaluateTask.Models;

namespace OvaluateTask.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<OrderController> logger;
        public OrderController(ApplicationDbContext context, ILogger<OrderController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("User Get List of Orders");
            var response =await context.Order.Include(o => o.Customer).ToListAsync();
            return View(response);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            logger.LogInformation("Get order Details Method Called");
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            logger.LogInformation("Create new order Method called");
            ViewData["Customer"] =await context.Customer.ToListAsync();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            logger.LogInformation("Create new order Method called");
            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid();
                context.Add(order);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(context.Customer, "Id", "Id", order.CustomerId);
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            logger.LogInformation("Edit order method called");
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Customer"] = await context.Customer.ToListAsync();
            return View(order);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Order order)
        {
            logger.LogInformation("Edit Order method called");
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(order);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(context.Customer, "Id", "Id", order.CustomerId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            logger.LogInformation("Delete Order method called");
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await context.Order.FindAsync(id);
            context.Order.Remove(order);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(Guid id)
        {
            return context.Order.Any(e => e.Id == id);
        }
    }
}
