using Azure.Core;
using ETMS_API.Data;
using ETMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ETMS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		public static User user = new User();
		private readonly IConfiguration _config;
        private readonly DataContext _context;
        public UserController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			try
			{
				var userList = await _context.Users.Where(u => u.IsActive).ToListAsync();
				return Ok(userList);

			}
			catch
			{
				return BadRequest();
			}
		}

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO request)
        {
			try
			{
				CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

				user.FullName = request.FullName;
				user.Email = request.Email;
				user.UserName = request.UserName;
				user.PasswordHash = passwordHash;
				user.PasswordSalt = passwordSalt;
				user.PasswordInPlainText = request.Password;
				user.UserRoleId = request.UserRoleId;
				user.IsActive = true;
				user.CreatedAt = DateTime.UtcNow;

				_context.Add(user);
				await _context.SaveChangesAsync();
				return Ok(user);
			}
			catch
			{
				return BadRequest();
			}
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
