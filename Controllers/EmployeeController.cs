using ETMS_API.Data;
using ETMS_API.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
		public async Task<IActionResult> PostAsync([FromForm] IFormFile? file, [FromBody] Employee employee)
		{
			try
			{
				if(file != null && file.Length > 0)
				{
					#region Excel File store directly to the DB
					using (var stream = file.OpenReadStream())
					{
						using (var reader = ExcelReaderFactory.CreateReader(stream))
						{
							do
							{
								bool isHeaderSkipped = false;
								while (reader.Read())
								{
									if (!isHeaderSkipped)
									{
										isHeaderSkipped = true;
										continue;
									}

									var emp = new Employee();
									emp.FirstName = reader.GetValue(1).ToString();
									emp.LastName = reader.GetValue(2).ToString();
									emp.Email = reader.GetValue(3).ToString();
									emp.Designation = Convert.ToInt32(reader.GetValue(4).ToString());
									emp.Gender = Convert.ToInt32(reader.GetValue(5).ToString());

									_dataContext.Add(emp);

									await _dataContext.SaveChangesAsync();

								}
							} while (reader.NextResult());
						}
					}
					#endregion

					#region Excel File Upload and store in the DB
					/*
					var uploadFolder = $"{Directory.GetCurrentDirectory()}\\Uploads\\Excel";
					if(!Directory.Exists(uploadFolder))
					{
						Directory.CreateDirectory(uploadFolder);
					}

					var filePath = Path.Combine(uploadFolder, file.FileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
					{
						using (var reader = ExcelReaderFactory.CreateReader(stream))
						{
							do
							{
								bool isHeaderSkipped = false;
								while (reader.Read())
								{
									if (!isHeaderSkipped)
									{
										isHeaderSkipped = true;
										continue;
									}

									var emp = new Employee();
									emp.FirstName = reader.GetValue(1).ToString();
									emp.LastName = reader.GetValue(2).ToString();
									emp.Email = reader.GetValue(3).ToString();
									emp.Designation = Convert.ToInt32(reader.GetValue(4).ToString());
									emp.Gender = Convert.ToInt32(reader.GetValue(5).ToString());

									_dataContext.Add(emp);

									await _dataContext.SaveChangesAsync();

								}
							} while (reader.NextResult());
						}
					}
					*/
					#endregion
					return Ok("Excel File Data Saved Succesfully");
				} 
				else
				{
					employee.IsActive = true;
					employee.CreatedAt = DateTime.UtcNow;
					_dataContext.Employees.Add(employee);
					await _dataContext.SaveChangesAsync();

					return Ok("Employee Created Successfully");
				}	
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
