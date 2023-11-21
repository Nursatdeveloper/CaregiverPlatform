using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaregiverPlatform.Models {

    [Table("tbmembers", Schema = "public")]
    public class Member {
        [Key]
        [Column("member_id")]
        public int MemberId { get; set; }
        [Column("member_user_id")]
        public int MemberUserId { get; set; }
        [Column("house_rules")]
        public string[] HouseRules { get; set; }


    }

}
