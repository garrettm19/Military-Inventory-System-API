using Microsoft.AspNetCore.Mvc;
using Military_Inventory_System_API.Models;
using Military_Inventory_System_API.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Military_Inventory_System_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminCredentialController : ControllerBase
    {
        private readonly MilitaryInventorySystemContext _inventoryDbContext;

        public AdminCredentialController(MilitaryInventorySystemContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        [HttpPost]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<IActionResult> CreateAsync([FromBody] AdminCredential adminCredential)
        {
            adminCredential.CreatedAt = DateTime.UtcNow;
            adminCredential.UpdatedAt = DateTime.UtcNow;
            _inventoryDbContext.AdminCredentials.Add(adminCredential);
            await _inventoryDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByIdAsync), new { id = adminCredential.ID }, adminCredential);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var adminCredentials = await _inventoryDbContext.AdminCredentials.ToListAsync();
            return Ok(adminCredentials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var adminCredential = await _inventoryDbContext.AdminCredentials.FindAsync(id);
            if (adminCredential == null)
            {
                return NotFound();
            }
            return Ok(adminCredential);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUsernameAsync(string username)
        {
            var adminCredential = await _inventoryDbContext.AdminCredentials.FirstOrDefaultAsync(u => u.Username == username);
            if (adminCredential == null)
            {
                return NotFound();
            }
            return Ok(adminCredential);
        }

        [HttpGet("fullname/{fullname}")]
        public async Task<IActionResult> GetByFullNameAsync(string fullname)
        {
            var adminCredential = await _inventoryDbContext.AdminCredentials.FirstOrDefaultAsync(u => u.FullName == fullname);
            if (adminCredential == null)
            {
                return NotFound();
            }
            return Ok(adminCredential);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonElement jsonElement)
        {
            var adminCredential = await _inventoryDbContext.AdminCredentials.FindAsync(id);
            if (adminCredential == null)
            {
                return NotFound();
            }

            if (jsonElement.TryGetProperty("username", out var usernameProperty))
            {
                adminCredential.Username = usernameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("password", out var passwordProperty))
            {
                adminCredential.Password = passwordProperty.GetString();
            }

            if (jsonElement.TryGetProperty("fullName", out var fullNameProperty))
            {
                adminCredential.FullName = fullNameProperty.GetString();
            }

            adminCredential.UpdatedAt = DateTime.UtcNow;
            _inventoryDbContext.AdminCredentials.Update(adminCredential);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var adminCredential = await _inventoryDbContext.AdminCredentials.FindAsync(id);
            if (adminCredential == null)
            {
                return NotFound();
            }
            _inventoryDbContext.AdminCredentials.Remove(adminCredential);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            _inventoryDbContext.AdminCredentials.RemoveRange(_inventoryDbContext.AdminCredentials);
            await _inventoryDbContext.SaveChangesAsync();
            await _inventoryDbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('AdminCredentials', RESEED, 0)");
            return NoContent();
        }
    }
}