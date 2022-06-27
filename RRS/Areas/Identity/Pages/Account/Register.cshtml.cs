// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RRS.Data;
using RRS.Services;

namespace RRS.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly CustomerService _customerService;
        private readonly ApplicationDbContext _context;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager, CustomerService customerService, ApplicationDbContext context)
            
        {
            _userManager = userManager;
            _userStore = userStore;            
            _signInManager = signInManager;
            _customerService = customerService;
            _context = context;


        }
             
       
        public class InputModel
        {
            [Required,  Display(Name = "First Name")]
            public string FirstName { get; set; }
            
            [Required, Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required, Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            public int RestaurantId { get; set; } = 1;

            [Required, EmailAddress, Display(Name = "Email")]          
            public string Email { get; set; }           
            
            [Required, DataType(DataType.Password)]   
            public string Password { get; set; }

           
            [DataType(DataType.Password),Display(Name = "Confirm password"),Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public async Task OnGetAsync(int? customerId)
        {
   
           if(customerId.HasValue)
           {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId); 
                if(customer != null)
                {
                    Input = new InputModel
                    {
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        PhoneNumber = customer.PhoneNumber,
                        
                    }; 
      
                }
           }
        }

        public async Task<IActionResult> OnPostAsync()
        {
          
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = Input.Email, UserName = Input.Email };                          
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var customer = await _customerService.FindOrCreateCustomerAsync(Input.FirstName, Input.LastName, Input.Email, Input.PhoneNumber, Input.RestaurantId);
                    customer.UserId = user.Id;
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, "Member");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { area = "" });

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

 
    }
}
