using ETMS_API.Data;
using ETMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ETMS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		public static User user = new User();

		private readonly IConfiguration _config;
		private readonly DataContext _context;

        public AuthenticationController(IConfiguration config, DataContext context)
        {
			_config = config;
			_context = context;
		}

		[HttpPost("register")]
		public async Task<ActionResult<User>> Register(UserDTO request)
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

				_context.Add(user);
				await _context.SaveChangesAsync();
				return Ok(user);
			} catch
			{
				return BadRequest();
			}
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> Login(UserDTO request)
		{
			try
			{
				var userinfo = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
				if (userinfo == null)
				{
					return BadRequest("User Not Found");
				}

				if (!VerifyPasswordHash(request.Password, userinfo.PasswordHash, userinfo.PasswordSalt))
				{
					return Unauthorized("Wrong Password. Please try Again");
				}

				string token = CreateToken(userinfo);

				return Ok(new { Token = token, UserInfo = userinfo });

			} catch
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

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

				return computedHash.SequenceEqual(passwordHash);
			}
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim("UserId", user.Id.ToString()),
				new Claim("UserName", user.UserName),
				new Claim("UserRole", user.UserRoleId.ToString())
			};

			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_config.GetSection("AppSettings:Token").Value!));

			var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
