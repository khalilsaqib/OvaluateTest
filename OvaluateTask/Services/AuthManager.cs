using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OvaluateTask.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OvaluateTask.Services
{
    public class AuthManager : IAuthManager
    {
        private UserManager<IdentityUser> userManager;
        private IConfiguration configuration;
        public AuthManager(UserManager<IdentityUser> userManger, IConfiguration configuration)
        {
            this.userManager = userManger;
            this.configuration = configuration;
        }
        public async Task<ResponseModel> RegisterUserAsync(RegisterModel model)
        {
            try
            {
                //get user to check if user already exist
                var user = await userManager.FindByEmailAsync(model.Email);
                //if not exist it returns null user
                if (user == null)
                {

                    //if password doesn't match
                    if (model.Password != model.ConfirmPassword)
                    {

                        return new ResponseModel
                        {
                            Message = "Password doesn not match! :("
                        };
                    }

                    //model not null, password confirm, creat identity usr
                    var IdentityUser = new UserAccount()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        IsActive = true
                    };

                    IdentityResult result = null;


                    result = await userManager.CreateAsync(IdentityUser, model.Password);


                    if (result.Succeeded)
                    {

                        return new ResponseModel()
                        {
                            Message = "User Created Successfully!",
                            IsSuccess = true
                        };
                    }
                    else
                    {
                        return new ResponseModel()
                        {
                            Message = "Error in creating new user",
                            IsSuccess = false,
                            Errors = result.Errors.Select(e => e.Description)
                        };
                    }
                }
                else
                {
                    // if same user already exist then return error message
                    return new ResponseModel()
                    {
                        Message = "User already exist",
                        IsSuccess = false,

                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Message = ex.InnerException.ToString()
                };
            }
        }
        public async Task<ResponseModel> LoginUser(LoginModel model)
        {
            //check user exist
            var user = await userManager.FindByEmailAsync(model.Email);
            //if not exist
            if (user == null)
            {
                return new ResponseModel()
                {
                    Message = "User Doesn't Exist!",
                    IsSuccess = false
                };
            }


            bool result;
            result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new ResponseModel()
                {
                    Message = "Wrong Password!",
                    IsSuccess = false
                };
            }

            
            //setting claims for jwt token
            var claims = new List<Claim>
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                   issuer: configuration["AuthSettings:ValidIssuer"],
                   audience: configuration["AuthSettings:ValidAudience"],
                   claims: claims,
                   expires: DateTime.Now.AddMonths(1),
                   signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new ResponseModel()
            {
                Token = tokenString,
                Message = "Token Created,Successful Login",
                IsSuccess = true,
                ExpireDate = DateTime.Now.AddDays(30)
            };

        }
    }
}
