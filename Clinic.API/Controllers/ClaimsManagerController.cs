using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.ClaimManagersDTOs;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Clinic.API.Controllers
{
    [Authorize(Policy = "AdminPanel")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsManagerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ClaimsManagerController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }




        [HttpPost("AddClaimToUserByUserId/{userId}")]
        public async Task<IActionResult> AddClaimToUserByUserIdAsync(Guid userId, CreateClaimDto claim)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return BadRequest("User not found!");

                var newClaim = new Claim(claim.ClaimType, claim.ClaimValue);

                await _userManager.AddClaimAsync(user, newClaim);

                return Ok("Claim has been added successfully to user.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in AddClaimToUserByUserIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("RemoveClaimFromUserByUserId/{userId}")]
        public async Task<IActionResult> RemoveClaimFromUserByUserIdAsync(Guid userId, DeleteClaimDto claim)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return BadRequest("User not found!");

                var claimForRemove = User.Claims.FirstOrDefault(c => c.Type == claim.ClaimType);

                if (claimForRemove == null)
                    return BadRequest("No claim found!");

                await _userManager.RemoveClaimAsync(user, claimForRemove);

                return Ok("Claim has been deleted successfully from user.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in RemoveClaimFromUserByUserIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }
    }
}
