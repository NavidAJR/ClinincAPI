using System;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Clinic.Core.DTOs.AccountsDTOs;
using Clinic.Core.EmailSenders.Interfaces;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }




        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto register)
        {
            try
            {
                var newUser = _mapper.Map<User>(register);
                var result = await _userManager.CreateAsync(newUser, register.Password);

                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }

                    return BadRequest(errors);
                }

                return Ok("User has been created successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in RegisterAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                    return BadRequest("You're already signed in!");


                var userForLogin = await _userManager.FindByNameAsync(login.UserName);

                if (userForLogin == null)
                    return BadRequest("User not found!");

                var result = _signInManager.PasswordSignInAsync(userForLogin, login.Password, login.IsPersistent, false).Result;

                if (!result.Succeeded)
                    return BadRequest("User not found!");

                return Ok("Successful!");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in LoginAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return Ok("Successful!");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in LogoutAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto forgot)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgot.Email);

                if (user == null)
                    return BadRequest("User not found!");

                var token = HttpUtility.UrlEncode(_userManager.GeneratePasswordResetTokenAsync(user).Result);
                var callbackUrl = Url.Action("ResetPassword", new { Controller = "Account", Action = "ResetPassword", id = user.Id, token = token });
                var body = $"Please click link below to reset your password! </br> <a href = \"{callbackUrl}\" >Reset Password</a>";
                const string title = "Reset Password";

                await _emailSender.Execute(forgot.Email, body, title);

                return Ok("Reset password link has been sent to your email.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ForgotPasswordAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(string id, string token, ResetPasswordDto reset)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = _userManager.ResetPasswordAsync(user, token, reset.Password).Result;

                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }

                    return BadRequest(errors);
                }

                return Ok("Password has been updated successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ResetPasswordAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }
    }
}
