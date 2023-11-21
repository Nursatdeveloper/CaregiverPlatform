using CaregiverPlatform;
using CaregiverPlatform.Common;
using CaregiverPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPlatform.Controllers {
    public class JobsController : Controller{
        private readonly CaregiverPlatformDbContext _context;
        public JobsController(CaregiverPlatformDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var Jobs = await _context.TbJobs.ToArrayAsync();
            return View(new GetJobsRes(Jobs));
        }


        [HttpGet]
        public IActionResult AddJob() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddJob(AddJobDto addJobDto) {
            var Job = addJobDto
                .ToJob(IdGen.GetId());

            await _context.TbJobs.AddAsync(Job);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Job was added successfully!";
            var Jobs = await _context.TbJobs.ToArrayAsync();

            return View("Index", new GetJobsRes(Jobs));
        }

        [HttpGet]
        public async Task<IActionResult> EditJob(int id) {
            var Job = await _context.TbJobs.FindAsync(id);
            if(Job == null) {
                ViewData["errorMessage"] = "Job does not exist";
            }
            return View(Job?.ToEditJobViewDto());
        }

        [HttpPost]
        public async Task<IActionResult> EditJob(EditJobViewDto editJobDto) {
            var Job = await _context.TbJobs.FindAsync(editJobDto.Id);
            if(Job == null) {
                throw new InvalidOperationException();
            }
            Job.MemberUserId = editJobDto.MemberUserId;
            Job.RequiredCaregivingType = editJobDto.RequiredCaregivingType;
            Job.OtherRequirements = editJobDto.OtherReqs.Split(",");

            _context.TbJobs.Update(Job);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Job was updated successfully!";
            var Jobs = await _context.TbJobs.ToArrayAsync();

            return View("Index", new GetJobsRes(Jobs));
        }

        [HttpGet]
        public IActionResult DeleteJob(int id) {
            return View(new DeleteJobDto(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJobPost(DeleteJobDto deleteJobDto) {
            var Job = await _context.TbJobs.FindAsync(deleteJobDto.Id);
            _context.TbJobs.Remove(Job);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Job was deleted successfully!";
            var Jobs = await _context.TbJobs.ToArrayAsync();

            return View("Index", new GetJobsRes(Jobs));
        }
    }
    //job_id, member_user_id, required_caregiving_type, other_requirements, date_posted, is_active
    public record GetJobsRes(Job[] Jobs);
    public record AddJobDto(int MemberUserId, string RequiredCaregivingType, string OtherReqs);
    public record EditJobViewDto(int Id, int MemberUserId, string RequiredCaregivingType, string OtherReqs);
    public record DeleteJobDto(int Id);

    public static class JobExt {
        public static Job ToJob(this AddJobDto dto, int id) {
            return new Job {
                JobId = id,
                MemberUserId = dto.MemberUserId,
                RequiredCaregivingType = dto.RequiredCaregivingType,
                OtherRequirements = dto.OtherReqs.Split(","),
                DatePosted = DateTime.Now,
                IsActive = true
            };
        }

        public static EditJobViewDto ToEditJobViewDto(this Job Job) {
            var rules = "";
            for(var a = 0; a < Job.OtherRequirements.Length; a++) {
                if(a != Job.OtherRequirements.Length - 1) {
                    rules += $"{Job.OtherRequirements[a]}, ";
                } else {
                    rules += $"{Job.OtherRequirements[a]}";

                }
            }
            return new EditJobViewDto(Job.JobId, Job.MemberUserId, Job.RequiredCaregivingType, rules);
        }
    }
}
