using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_api_examlpe.Dtos.Account;
using web_api_examlpe.Models;

namespace web_api_examlpe.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
            
        [HttpPost("register")]
         public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser
            {
            UserName = registerDto.Username,
            Email = registerDto.Email
            };
        
            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if(createUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if(roleResult.Succeeded)
                {
                return Ok("User created");
                }
                else 
                {
                return StatusCode(500, roleResult.Errors);
                }
            }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {   
                return StatusCode(500, e);
            }
        
        }
    }
}