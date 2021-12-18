using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.UserManagersDTOs;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Clinic.API.Controllers
{
    [Authorize(Policy = "AdminPanel")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserManagerRepository _userManagerRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManagerIdentity;

        public UserManagerController(IUserManagerRepository userManagerRepository, IMapper mapper, UserManager<User> userManagerIdentity)
        {
            _userManagerRepository = userManagerRepository;
            _mapper = mapper;
            _userManagerIdentity = userManagerIdentity;
        }




        [HttpGet("ReadAllUsers")]
        public async Task<IActionResult> ReadAllUsersAsync()
        {
            try
            {
                var users = _mapper.Map<IEnumerable<UserManagerReadUserDto>>(await _userManagerRepository.ReadAllUsersAsync());
                return Ok(users);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadAllUsersAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpGet("ReadUserById/{userId}")]
        public async Task<IActionResult> ReadUserByIdAsync(Guid userId)
        {
            try
            {
                var user = _mapper.Map<UserManagerReadUserDto>(await _userManagerRepository.ReadUserByUserIdAsync(userId));
                return Ok(user);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadUserByIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync(UserManagerCreateUserDto create)
        {
            try
            {
                var newUser = _mapper.Map<User>(create);
                var result = await _userManagerIdentity.CreateAsync(newUser, create.Password);

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
                Log.Logger.Error("This exception caught in CreateUserAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUserAsync(Guid userId,UserManagerUpdateUserDto update)
        {
            try
            {
                var userToUpdate = await _userManagerIdentity.FindByIdAsync(userId.ToString());

                if (userToUpdate == null)
                    return BadRequest("User not found!");

                var updatedUser = _mapper.Map(update, userToUpdate);
                var result = _userManagerIdentity.UpdateAsync(updatedUser).Result;

                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }

                    return BadRequest(errors);
                }

                return Ok("User has been updated successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in UpdateUserAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPatch("PartialUserUpdate/{userId}")]
        public async Task<IActionResult> PartialUserUpdateAsync(Guid userId, JsonPatchDocument<UserManagerUpdateUserDto> patchDoc)
        {
            try
            {
                var userToUpdate = await _userManagerIdentity.FindByIdAsync(userId.ToString());

                if (userToUpdate == null)
                    return BadRequest("User not found!");

                #region Validations

                var userToPatch = _mapper.Map<UserManagerUpdateUserDto>(userToUpdate);
                patchDoc.ApplyTo(userToPatch, ModelState);
                if (!TryValidateModel(userToPatch))
                    return ValidationProblem(ModelState);

                #endregion


                var updatedUser = _mapper.Map(userToPatch, userToUpdate);

                await _userManagerIdentity.UpdateAsync(updatedUser);

                return Ok("User has been updated successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in PartialUserUpdateAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                var userToDelete = await _userManagerIdentity.FindByIdAsync(userId.ToString());

                if (userToDelete == null)
                    return BadRequest("User not found!");


                await _userManagerRepository.SoftDeleteUserByUserAsync(userToDelete);

                return Ok("User has been deleted successfully.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in DeleteUserAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }
    }
}
