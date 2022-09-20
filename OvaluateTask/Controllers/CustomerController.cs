using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OvaluateTask.Data;
using OvaluateTask.Models;

namespace OvaluateTask.Controllers
{
    
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext context;
        private SignInManager<IdentityUser> signInManager;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, ILogger<CustomerController> logger)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("User Get List of Customers");
            return View(await context.Customer.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            logger.LogInformation("Get Customer Details Method Called");
            if (id == null)
            {
                return NotFound();
            }

            var customer = await context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            logger.LogInformation("Create new Customer Method called");
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                context.Add(customer);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            logger.LogInformation("Edit Cutomer method called");
            if (id == null)
            {
                return NotFound();
            }

            var customer = await context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,Customer customer)
        {
            logger.LogInformation("Edit Cutomer method called");
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(customer);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            logger.LogInformation("Delete Cutomer method called");
            if (id == null)
            {
                return NotFound();
            }

            var customer = await context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await context.Customer.FindAsync(id);
            context.Customer.Remove(customer);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(Guid id)
        {
            return context.Customer.Any(e => e.Id == id);
        }
    }
}
