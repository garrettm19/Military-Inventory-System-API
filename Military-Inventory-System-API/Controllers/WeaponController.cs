using Microsoft.AspNetCore.Mvc;
using Military_Inventory_System_API.Models;
using Military_Inventory_System_API.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Military_Inventory_System_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly MilitaryInventorySystemContext _inventoryDbContext;

        public WeaponController(MilitaryInventorySystemContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        [HttpPost]
        [ActionName(nameof(GetBySerialNumberAsync))]
        public async Task<IActionResult> CreateAsync([FromBody] Weapon weapon)
        {
            if (!string.IsNullOrEmpty(weapon.UserSSN))
            {
                // Find the user based on the provided SSN
                weapon.User = await _inventoryDbContext.Users.FirstOrDefaultAsync(u => u.SSN == weapon.UserSSN);
            }

            if (weapon.User == null)
            {
                // No user with the provided SSN was found, set User property to null or indicate no user
                weapon.User = null; // Or any other indication, e.g., weapon.User = new User { SSNumber = "No User" };
            }

            _inventoryDbContext.Weapons.Add(weapon);
            await _inventoryDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBySerialNumberAsync), new { serialNumber = weapon.SerialNumber }, weapon);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var weapons = await _inventoryDbContext.Weapons
                .Include(w => w.User) // Eager loading the User object
                .ToListAsync();
            return Ok(weapons);
        }

        [HttpGet("{serialNumber}")]
        public async Task<IActionResult> GetBySerialNumberAsync(string serialNumber)
        {
            var weapon = await _inventoryDbContext.Weapons
                .Include(w => w.User) // Eager loading the User object
                .FirstOrDefaultAsync(w => w.SerialNumber == serialNumber);

            if (weapon == null)
            {
                return NotFound();
            }

            return Ok(weapon);
        }

        [HttpGet("user/{ssnNumber}")]
        public async Task<IActionResult> GetByUserSSNNumberAsync(string ssnNumber)
        {
            var weapons = await _inventoryDbContext.Weapons
                .Include(w => w.User) // Eager loading the User object
                .Where(w => w.UserSSN == ssnNumber)
                .ToListAsync();

            if (weapons == null || weapons.Count == 0)
            {
                return NotFound();
            }

            return Ok(weapons);
        }

        [HttpGet("weapon-type/{weaponType}")]
        public async Task<IActionResult> GetByWeaponTypeAsync(string weaponType)
        {
            var weapons = await _inventoryDbContext.Weapons
                .Include(w => w.User) // Eager loading the User object
                .Where(w => w.WeaponType == weaponType)
                .ToListAsync();

            if (weapons == null || weapons.Count == 0)
            {
                return NotFound();
            }

            return Ok(weapons);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatusAsync(bool status)
        {
            var weapons = await _inventoryDbContext.Weapons
                .Include(w => w.User) // Eager loading the User object
                .Where(w => w.WeaponStatus == status)
                .ToListAsync();

            if (weapons == null || weapons.Count == 0)
            {
                return NotFound();
            }

            return Ok(weapons);
        }

        [HttpPut("{serialNumber}")]
        public async Task<IActionResult> UpdateAsync(string serialNumber, [FromBody] JsonElement jsonElement)
        {
            var weapon = await _inventoryDbContext.Weapons.Include(w => w.User).FirstOrDefaultAsync(w => w.SerialNumber == serialNumber);
            if (weapon == null)
            {
                return NotFound();
            }

            if (jsonElement.TryGetProperty("userSSN", out var userSSNNumberProperty))
            {
                var userSSNNumber = userSSNNumberProperty.GetString();

                // Update the UserSSNumber property
                weapon.UserSSN = userSSNNumber;

                // Find the user associated with the provided SSN
                var user = await _inventoryDbContext.Users.FirstOrDefaultAsync(u => u.SSN == userSSNNumber);
                if (user != null)
                {
                    // Assign the user to the Weapon's User field
                    weapon.User = user;
                }
                else
                {
                    // If no user is found, set the User field to null or false, depending on its data type
                    weapon.User = null; // or false, depending on the data type of the User field
                }
            }

            if (jsonElement.TryGetProperty("weaponType", out var WeaponTypeProperty))
            {
                weapon.WeaponType = WeaponTypeProperty.GetString();
            }

            if (jsonElement.TryGetProperty("weaponStatus", out var weaponStatusProperty))
            {
                weapon.WeaponStatus = weaponStatusProperty.GetBoolean();
            }

            _inventoryDbContext.Weapons.Update(weapon);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{serialNumber}")]
        public async Task<IActionResult> DeleteAsync(string serialNumber)
        {
            var weapon = await _inventoryDbContext.Weapons.FirstOrDefaultAsync(w => w.SerialNumber == serialNumber);
            if (weapon == null)
            {
                return NotFound();
            }
            _inventoryDbContext.Weapons.Remove(weapon);
            await _inventoryDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            _inventoryDbContext.Weapons.RemoveRange(_inventoryDbContext.Weapons);
            await _inventoryDbContext.SaveChangesAsync();
            await _inventoryDbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('weapons', RESEED, 0)");
            return NoContent();
        }
    }
}
