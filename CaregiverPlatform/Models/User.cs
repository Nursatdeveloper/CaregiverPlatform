using System.ComponentModel.DataAnnotations.Schema;

namespace CaregiverPlatform.Models {
    [Table("tbusers", Schema = "public")]
    public class User {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("given_name")]
        public string GivenName { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("profile_description")]
        public string ProfileDescription { get; set; }
        [Column("password")]
        public string Password { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
    }



}
