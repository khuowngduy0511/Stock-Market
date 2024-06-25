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
using web_api_examlpe.Service;
using web_api_examlpe.Interfaces;

namespace web_api_examlpe.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _TokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _TokenService = tokenService;
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
                    return Ok(
                        new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _TokenService.CreateToken(appUser)
                        }
                    );
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