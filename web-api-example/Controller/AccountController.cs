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
using Microsoft.AspNetCore.Identity.Data;

namespace web_api_examlpe.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if(user == null)   return Unauthorized("Invalid username!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded) return Unauthorized("USername not found and/or password incorrect");
            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
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
                            Token = _tokenService.CreateToken(appUser)
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