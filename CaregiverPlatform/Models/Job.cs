using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaregiverPlatform.Models {

    [Table("tbjobs", Schema = "public")]
    public class Job {
        [Key]
        [Column("job_id")]
        public int JobId { get; set; }
        [Column("member_user_id")]
        public int MemberUserId { get; set; }
        [Column("required_caregiving_type")]
        public string RequiredCaregivingType { get; set; }
        [Column("other_requirements")]
        public string[] OtherRequirements { get; set; }
        [Column("date_posted")]
        public DateTime DatePosted { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }

    }
}
