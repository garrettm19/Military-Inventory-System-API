using Microsoft.AspNetCore.Mvc;
using Military_Inventory_System_API.Models;
using Military_Inventory_System_API.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Military_Inventory_System_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeaponManagementUserController : ControllerBase
    {
        private readonly MilitaryInventorySystemContext _inventoryDbContext;

        public WeaponManagementUserController(MilitaryInventorySystemContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        [HttpPost]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<IActionResult> CreateAsync([FromBody] WeaponManagementUser weaponManagementUser)
        {
            weaponManagementUser.CreatedAt = DateTime.UtcNow;
            weaponManagementUser.UpdatedAt = DateTime.UtcNow;
            _inventoryDbContext.WeaponManagementUsers.Add(weaponManagementUser);
            await _inventoryDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByIdAsync), new { id = weaponManagementUser.ID }, weaponManagementUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var weaponManagementUsers = await _inventoryDbContext.WeaponManagementUsers.ToListAsync();
            return Ok(weaponManagementUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var weaponManagementUser = await _inventoryDbContext.WeaponManagementUsers.FindAsync(id);
            if (weaponManagementUser == null)
            {
                return NotFound();
            }
            return Ok(weaponManagementUser);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUsernameAsync(string username)
        {
            var weaponManagementUser = await _inventoryDbContext.WeaponManagementUsers.FirstOrDefaultAsync(u => u.Username == username);
            if (weaponManagementUser == null)
            {
                return NotFound();
            }
            return Ok(weaponManagementUser);
        }

        [HttpGet("fullname/{fullname}")]
        public async Task<IActionResult> GetByFullnameAsync(string fullname)
        {
            var weaponManagementUser = await _inventoryDbContext.WeaponManagementUsers.FirstOrDefaultAsync(u => u.FullName == fullname);
            if (weaponManagementUser == null)
            {
                return NotFound();
            }
            return Ok(weaponManagementUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonElement jsonElement)
        {
            var weaponManagementUser = await _inventoryDbContext.WeaponManagementUsers.FindAsync(id);
            if (weaponManagementUser == null)
            {
                return NotFound();
            }

            if (jsonElement.TryGetProperty("username", out var usernameProperty))
            {
                weaponManagementUser.Username = usernameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("password", out var passwordProperty))
            {
                weaponManagementUser.Password = passwordProperty.GetString();
            }

            if (jsonElement.TryGetProperty("fullname", out var fullNameProperty))
            {
                weaponManagementUser.FullName = fullNameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("role", out var roleProperty))
            {
                weaponManagementUser.Role = roleProperty.GetString();
            }

            weaponManagementUser.UpdatedAt = DateTime.UtcNow;
            _inventoryDbContext.WeaponManagementUsers.Update(weaponManagementUser);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var weaponManagementUser = await _inventoryDbContext.WeaponManagementUsers.FindAsync(id);
            if (weaponManagementUser == null)
            {
                return NotFound();
            }
            _inventoryDbContext.WeaponManagementUsers.Remove(weaponManagementUser);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            _inventoryDbContext.WeaponManagementUsers.RemoveRange(_inventoryDbContext.WeaponManagementUsers);
            await _inventoryDbContext.SaveChangesAsync();
            await _inventoryDbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('WeaponManagementUsers', RESEED, 0)");
            return NoContent();
        }
    }
}