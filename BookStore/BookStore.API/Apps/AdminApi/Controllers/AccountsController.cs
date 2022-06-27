using AutoMapper;
using BookStore.Api.Apps.AdminApi.DTOs.AccountDTOs;
using BookStore.Core.Entities;
using BookStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Api.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AccountsController(DataContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration,IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
      public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            AppUser admin = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName==loginDTO.UserName && x.IsAdmin);
            if(admin==null)
            {
                return NotFound();  
            }
            List<Claim> claims = new List<Claim>()
           {
               new Claim(ClaimTypes.NameIdentifier, admin.Id),
               new Claim(ClaimTypes.Name,admin.UserName),
               new Claim("FullName",admin.Fullname),
               new Claim("IsAdmin",admin.IsAdmin.ToString())
           };
            var adminRoles = _userManager.GetRolesAsync(admin).Result;
            var roleClaims = adminRoles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);
            string keyStr = _configuration.GetSection("JWT:secretKey").Value;
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyStr));
            SigningCredentials creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken
                (
                claims: claims,
                signingCredentials: creds,
                expires: System.DateTime.UtcNow.AddHours(12),
                issuer: _configuration.GetSection("JWT:issuer").Value,
                audience: _configuration.GetSection("JWT:audience").Value
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            HttpContext.Response.Cookies.Append("UserName", admin.UserName);
            HttpContext.Response.Cookies.Append("Token", tokenStr);

            var result = await _signInManager.PasswordSignInAsync(admin, loginDTO.Password, false, false);
            if (!result.Succeeded)
            {
                return StatusCode(404, "Ad və ya şifrə yanlışdır !");
            }
            return Ok(new {token=tokenStr});
        }
        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Append("UserName","");
            return StatusCode(201, "Ugurla hesabdan cixildi");
        }
        [HttpPut]
        public async Task<IActionResult> Update(AccountPostDTO accountPost)
        {
            var token=HttpContext.Request.Cookies["Token"];
            
            if(HttpContext.Request.Cookies["UserName"]=="")
            {
                return StatusCode(401, "Daxil olmamisiz");
            }
            string UserName = HttpContext.Request.Cookies["UserName"];
            AppUser member = await _userManager.FindByNameAsync(UserName);
            if(member==null)
            {
                return BadRequest();
            }
            member.Fullname = accountPost.FullName;
            member.PhoneNumber = accountPost.PhoneNumber;
            member.Email = accountPost.Email;
            _context.SaveChanges();
            AccountGetDTO getDTO = _mapper.Map<AccountGetDTO>(member);
            return Ok(getDTO);

        }
        
       
    }
}
