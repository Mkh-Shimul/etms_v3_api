using Azure.Core;
using ETMS_API.Data;
using ETMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ETMS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
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
				var userList = await _context.Users
					.Where(u => u.IsActive)
					.Join(
						_context.UserRoles,
						u => u.UserRoleId,
						ur => ur.Id,
						(u, ur) => new { Id = u.Id, FullName = u.FullName, Email = u.Email, UserName = u.UserName, RoleName = ur.RoleName }
					)
					.ToListAsync();
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
				user.IsActive = request.IsActive;
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

		[Route("{id}")]
		[HttpGet]
		public async Task<IActionResult> GetAsync(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpGet("GetAllUserRoles")]
		public async Task<IActionResult> GetAllUserRoles()
		{
			try
			{
				var userRoleList = await _context.UserRoles
					.Where(ur => ur.IsActive)
					.Select(ur => new {Id = ur.Id,  RoleName = ur.RoleName})
					.ToListAsync();
				return Ok(userRoleList);
			}
			catch (Exception ex)
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
