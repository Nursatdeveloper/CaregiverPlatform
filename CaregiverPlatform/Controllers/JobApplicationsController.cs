using CaregiverPlatform;
using CaregiverPlatform.Common;
using CaregiverPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationPlatform.Controllers {
    public class JobApplicationsController: Controller {
        private readonly CaregiverPlatformDbContext _context;
        public JobApplicationsController(CaregiverPlatformDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var JobApplications = await _context.TbJobApplications.ToArrayAsync();
            return View(new GetJobApplicationsRes(JobApplications));
        }


        [HttpGet]
        public IActionResult AddJobApplication() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddJobApplication(AddJobApplicationDto addJobApplicationDto) {
            var JobApplication = addJobApplicationDto
                .ToJobApplication(IdGen.GetId());

            await _context.TbJobApplications.AddAsync(JobApplication);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "JobApplication was added successfully!";
            var JobApplications = await _context.TbJobApplications.ToArrayAsync();

            return View("Index", new GetJobApplicationsRes(JobApplications));
        }


        [HttpGet]
        public IActionResult DeleteJobApplication(int id) {
            return View(new DeleteJobApplicationDto(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJobApplicationPost(DeleteJobApplicationDto deleteJobApplicationDto) {
            var JobApplication = await _context.TbJobApplications.FindAsync(deleteJobApplicationDto.Id);
            _context.TbJobApplications.Remove(JobApplication);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "JobApplication was deleted successfully!";
            var JobApplications = await _context.TbJobApplications.ToArrayAsync();

            return View("Index", new GetJobApplicationsRes(JobApplications));
        }
    }

    public record GetJobApplicationsRes(JobApplication[] JobApplications);
    public record AddJobApplicationDto(int JobId, int CaregiverUserId);
    public record DeleteJobApplicationDto(int Id);

    public static class JobApplicationExt {
        public static JobApplication ToJobApplication(this AddJobApplicationDto dto, int id) {
            return new JobApplication {
                JobApplicationId = id,
                JobId = dto.JobId,
                CaregiverUserId = dto.CaregiverUserId,
                DateApplied = DateTime.Now,
                IsActive = true
            };
        }


    }
}
