using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaregiverPlatform.Models {

    [Table("tbcaregivers", Schema = "public")]
    public class Caregiver {
        [Key]
        [Column("caregiver_id")]
        public int CaregiverId { get; set; }
        [Column("caregiver_user_id")]
        public int CaregiverUserId { get; set; }
        [Column("photo")]
        public string Photo { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("caregiving_type")]
        public string CaregivingType { get; set; }
        [Column("hourly_rate")]
        public double HourlyRate { get; set; }

    }

    public static class CaregivingType {
        public const string Babysitter = "babysitter";
        public const string PlaymateForChildren = "playmate for children";
        public const string CaregiverForElderly = "caregiver for elderly";
    }

    public static class Gender {
        public const string Male = "male";
        public const string Female = "female";
    }
}
