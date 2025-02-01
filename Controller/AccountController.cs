using FinSharkBackEnd.Dtos.Account;
using FinSharkBackEnd.Interfaces;
using FinSharkBackEnd.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinSharkProjeto.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if(user == null){
                return Unauthorized("Usuário não encontrado");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded){
                return Unauthorized("Usuário ou senha inválidos");
            }
            return Ok(new UserDTO{
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
           try{
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = new AppUser{
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded){
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(
                new UserDTO{
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );

           }
           catch(Exception e){
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao cadastrar usuário");
           }
        }
    }
} 