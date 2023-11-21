using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaregiverPlatform.Models {

    //    job_app_id = Column(Integer(), primary_key= True)
    //job_id = Column(Integer(), ForeignKey('tbjobs.job_id'), nullable=False)
    //caregiver_user_id = Column(Integer(), ForeignKey('tbusers.user_id'), nullable=False)
    //date_applied = Column(DateTime(), nullable= False)
    //is_active = Column(Boolean(), nullable= False)
    [Table("tbjobapplications", Schema = "public")]

    public class JobApplication {
        [Key]
        [Column("job_app_id")]
        public int JobApplicationId { get; set; }

        [Column("job_id")]
        public int JobId { get; set; }

        [Column("caregiver_user_id")]
        public int CaregiverUserId { get; set; }
        [Column("date_applied")]
        public DateTime DateApplied { get; set; }
        [Column("is_active")]

        public bool IsActive { get; set; }
    }
}
