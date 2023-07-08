using Microsoft.AspNetCore.Mvc;
using Military_Inventory_System_API.Models;
using Military_Inventory_System_API.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Military_Inventory_System_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MilitaryInventorySystemContext _inventoryDbContext;

        public UserController(MilitaryInventorySystemContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        [HttpPost]
        [ActionName(nameof(GetBySSNumberAsync))]
        public async Task<IActionResult> CreateAsync([FromBody] User user)
        {
            _inventoryDbContext.Users.Add(user);
            await _inventoryDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBySSNumberAsync), new { ssNumber = user.SSN }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _inventoryDbContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{ssNumber}")]
        public async Task<IActionResult> GetBySSNumberAsync(string ssNumber)
        {
            var user = await _inventoryDbContext.Users.FindAsync(ssNumber);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var user = await _inventoryDbContext.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("rank/{rank}")]
        public async Task<IActionResult> GetByRankAsync(string rank)
        {
            var user = await _inventoryDbContext.Users.FirstOrDefaultAsync(u => u.Rank == rank);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{ssnNumber}")]
        public async Task<IActionResult> UpdateAsync(string ssnNumber, [FromBody] JsonElement jsonElement)
        {
            var user = await _inventoryDbContext.Users.FindAsync(ssnNumber);
            if (user == null)
            {
                return NotFound();
            }

            if (jsonElement.TryGetProperty("name", out var nameProperty))
            {
                user.Name = nameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("dob", out var dobProperty))
            {
                user.DOB = dobProperty.GetDateTime();
            }

            if (jsonElement.TryGetProperty("rank", out var rankProperty))
            {
                user.Rank = rankProperty.GetString();
            }

            _inventoryDbContext.Users.Update(user);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{ssnNumber}")]
        public async Task<IActionResult> DeleteAsync(string ssnNumber)
        {
            var user = await _inventoryDbContext.Users.FindAsync(ssnNumber);
            if (user == null)
            {
                return NotFound();
            }
            _inventoryDbContext.Users.Remove(user);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            _inventoryDbContext.Users.RemoveRange(_inventoryDbContext.Users);
            await _inventoryDbContext.SaveChangesAsync();
            await _inventoryDbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('users', RESEED, 0)");
            return NoContent();
        }
    }
}
