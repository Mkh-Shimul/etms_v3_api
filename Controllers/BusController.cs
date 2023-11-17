using ETMS_API.Data;
using ETMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETMS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BusController : ControllerBase
	{
		private readonly DataContext _dataContext;

        public BusController(DataContext context)
        {
			_dataContext = context;
        }

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var buses = await _dataContext.Buses.Where(w => w.IsActive).ToListAsync();
			return Ok(buses);
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync(Bus bus)
		{
			bus.IsActive = true;
			bus.CreatedAt = DateTime.UtcNow;
			_dataContext.Buses.Add(bus);
			await _dataContext.SaveChangesAsync();
			return Ok();
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<IActionResult> GetAsync(int id)
		{
			var bus = await _dataContext.Buses.FindAsync(id);
			if (bus == null)
			{
				return NotFound();
			}

			return Ok(bus);
		}


		[Route("{id}")]
		[HttpPut]
		public async Task<IActionResult> PutAsync(Bus bus, int id)
		{
			var existBus = await _dataContext.Buses.FindAsync(id);
			if(existBus == null)
			{
				return NotFound();
			}

			existBus.BusNumber = bus.BusNumber;
			existBus.StartFrom = bus.StartFrom;
			existBus.StopAt = bus.StopAt;
			existBus.StartTime = bus.StartTime;
			existBus.EndTime = bus.EndTime;
			existBus.IsActive = true;
			existBus.UpdatedAt = DateTime.UtcNow;

			_dataContext.Buses.Update(existBus);

			_dataContext.SaveChanges();

			return Ok();
		}


		[Route("{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var bus = await _dataContext.Buses.FindAsync(id);

			if(bus == null)
			{
				return NotFound();
			}

			bus.IsActive = false;
			bus.UpdatedAt = DateTime.UtcNow;
			await _dataContext.SaveChangesAsync();

			return Ok();

		}
    }
}
