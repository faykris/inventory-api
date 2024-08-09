using InventoryAPI.Data;
using InventoryAPI.Models.Dtos;
using InventoryAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public EmployeeController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployee registerEmployee)
        {
            bool userExists = await _dbContext.Employees.AnyAsync(e => e.Email == registerEmployee.Email);
            if (userExists)
                return BadRequest("El correo ya esta registrado con otro usuario");

            registerEmployee.Password = BCrypt.Net.BCrypt.HashPassword(registerEmployee.Password);

            Employee employee = new Employee
            {
                Name = registerEmployee.Name,
                Email = registerEmployee.Email,
                Password = registerEmployee.Password,
                Phone = registerEmployee.Phone,
                Role = 2,
                Status = 1,
                Created = DateTime.Now,
            };

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginEmployee([FromBody] Login login)
        {
            var user = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return Unauthorized("Correo y/o contraseña incorrectos");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role == 1 ? "Admin" : "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

    }
}
