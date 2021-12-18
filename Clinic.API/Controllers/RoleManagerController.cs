using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.RoleManagersDTOs;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Clinic.API.Controllers
{
    [Authorize(Policy = "AdminPanel")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleManagerController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        [HttpGet("ReadAllRoles")]
        public async Task<IActionResult> ReadAllRolesAsync()
        {
            try
            {
                var rolesFromIdentity = await _roleManager.Roles.ToListAsync();

                var rolesForShow = _mapper.Map<List<RoleManagerReadRoleDto>>(rolesFromIdentity);

                return Ok(rolesForShow);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadAllRolesAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpGet("ReadRoleByRoleId/{roleId}")]
        public async Task<IActionResult> ReadRoleByRoleIdAsync(Guid roleId)
        {
            try
            {
                var roleFromIdentity = await _roleManager.FindByIdAsync(roleId.ToString());

                if (roleFromIdentity == null)
                    return BadRequest("Role not found!");

                var roleForShow = _mapper.Map<RoleManagerReadRoleDto>(roleFromIdentity);

                return Ok(roleForShow);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadRoleByRoleIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoleAsync(RoleManagerCreateRoleDto create)
        {
            try
            {
                var newRole = _mapper.Map<Role>(create);

                await _roleManager.CreateAsync(newRole);

                return Ok("Role has been created successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in CreateRoleAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("AddRoleToUser/{userId}&{roleName}")]
        public async Task<IActionResult> AddRoleToUserAsync(Guid userId,string roleName)
        {
            try
            {
                if (_roleManager.FindByNameAsync(roleName).Result == null)
                    return BadRequest("No role has been found with this name!");

                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                    return BadRequest("User not found!");


                await _userManager.AddToRoleAsync(user, roleName);

                return Ok($"{roleName} role has been added to {user.UserName} successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in AddRoleToUserAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPut("UpdateRole/{roleId}")]
        public async Task<IActionResult> UpdateRoleAsync(Guid roleId, RoleManagerUpdateRoleDto update)
        {
            try
            {
                var roleToUpdate = await _roleManager.FindByIdAsync(roleId.ToString());

                if (roleToUpdate == null)
                    return BadRequest("Role not found!");

                var updatedRole = _mapper.Map(update, roleToUpdate);

                await _roleManager.UpdateAsync(updatedRole);

                return Ok("Role has been updated Successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in UpdateRoleAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }



        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(Guid roleId)
        {
            try
            {
                var roleToDelete = await _roleManager.FindByIdAsync(roleId.ToString());

                if (roleToDelete == null)
                    return BadRequest("Role not found!");

                await _roleManager.DeleteAsync(roleToDelete);

                return Ok("Role has been deleted Successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in DeleteRoleAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }
    }
}
