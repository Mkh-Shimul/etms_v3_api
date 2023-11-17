using ETMS_API.Data;
using ETMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETMS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployeeController : ControllerBase
	{
		private readonly DataContext _dataContext;

		public EmployeeController(DataContext context)
        {
            _dataContext = context;
        }

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{

			//var userClaims = User.Claims;

			//var username = userClaims.FirstOrDefault(c => c.Type == "UserName")?.Value;
			//var userId = userClaims.FirstOrDefault(c => c.Type == "UserId")?.Value;
			var employee = await _dataContext.Employees.Where(e => e.IsActive).ToListAsync();
			return Ok(employee);
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync(Employee employee)
		{
			try
			{
					employee.IsActive = true;
					employee.CreatedAt = DateTime.UtcNow;
					_dataContext.Employees.Add(employee);
					await _dataContext.SaveChangesAsync();

					return Ok();
			} 
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<IActionResult> GetAsync(int id)
		{
			var emp = await _dataContext.Employees.FindAsync(id);
			if (emp == null)
			{
				return NotFound();
			}

			return Ok(emp);
		}

		[Route("{id}")]
		[HttpPut]
		public async Task<IActionResult> PutAsync(Employee employee, int id)
		{
			var existEmployee = await _dataContext.Employees.FindAsync(id);
			if(existEmployee == null)
			{
				return NotFound();
			}
			existEmployee.FirstName = employee.FirstName;
			existEmployee.LastName = employee.LastName;
			existEmployee.Email = employee.Email;
			existEmployee.Gender = employee.Gender;
			existEmployee.Designation = employee.Designation;
			existEmployee.IsActive = true;
			existEmployee.UpdatedAt = DateTime.UtcNow;
			_dataContext.Employees.Update(existEmployee);
			await _dataContext.SaveChangesAsync();

			return Ok();
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var employee = await _dataContext.Employees.FindAsync(id);

			if(employee == null)
			{
				return NotFound();
			}

			employee.IsActive = false;
			employee.UpdatedAt = DateTime.UtcNow;
			await _dataContext.SaveChangesAsync();
			return Ok(employee);
		}
		
    }
}
