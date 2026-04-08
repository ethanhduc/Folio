using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) //since we have complex data type
                return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());
            
            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password is incorrect!");

            return Ok(
                new NewUserDTO
                {
                    UserName = loginDTO.Username,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try 
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // user object created from resgisterDTO
                var appUser = new AppUser
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                };

                // IdentityResult object returns result of creating user
                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDTO
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    } else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}