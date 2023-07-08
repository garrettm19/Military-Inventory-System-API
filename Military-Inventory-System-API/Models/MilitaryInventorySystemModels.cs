using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Military_Inventory_System_API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("ssn")]
        public string SSN { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("dob")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Column("rank")]
        public string Rank { get; set; }
    }

    [Table("weapons")]
    public class Weapon
    {
        [Key]
        [Column("serial_number")]
        public string SerialNumber { get; set; }

        [Column("user_ssn")]
        public string? UserSSN { get; set; }

        public User? User { get; set; } // Nullable User property

        [Column("weapon_type")]
        public string WeaponType { get; set; }

        [Column("weapon_status")]
        public bool WeaponStatus { get; set; }
    }

    [Table("weapon_management_users")]
    public class WeaponManagementUser
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    [Table("admin_credentials")]
    public class AdminCredential
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

}
