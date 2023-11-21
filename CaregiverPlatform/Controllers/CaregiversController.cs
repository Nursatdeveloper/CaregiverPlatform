using CaregiverPlatform.Common;
using CaregiverPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaregiverPlatform.Controllers {
    public class CaregiversController : Controller{
        private readonly CaregiverPlatformDbContext _context;
        public CaregiversController(CaregiverPlatformDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var Caregivers = await _context.TbCaregivers.ToArrayAsync();
            return View(new GetCaregiversRes(Caregivers));
        }


        [HttpGet]
        public IActionResult AddCaregiver() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCaregiver(AddCaregiverDto addCaregiverDto) {
            var Caregiver = addCaregiverDto
                .ToCaregiver(IdGen.GetId());

            await _context.TbCaregivers.AddAsync(Caregiver);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Caregiver was added successfully!";
            var Caregivers = await _context.TbCaregivers.ToArrayAsync();

            return View("Index", new GetCaregiversRes(Caregivers));
        }

        [HttpGet]
        public async Task<IActionResult> EditCaregiver(int id) {
            var Caregiver = await _context.TbCaregivers.FindAsync(id);
            if(Caregiver == null) {
                ViewData["errorMessage"] = "Caregiver does not exist";
            }
            return View(Caregiver?.ToEditCaregiverViewDto());
        }

        [HttpPost]
        public async Task<IActionResult> EditCaregiver(EditCaregiverViewDto editCaregiverDto) {
            var Caregiver = await _context.TbCaregivers.FindAsync(editCaregiverDto.Id);
            if(Caregiver == null) {
                throw new InvalidOperationException();
            }
            Caregiver.Gender = editCaregiverDto.Gender;
            Caregiver.HourlyRate = editCaregiverDto.HourlyRate;
            Caregiver.CaregivingType = editCaregiverDto.CaregivingType;
            Caregiver.Photo = editCaregiverDto.Photo;

            _context.TbCaregivers.Update(Caregiver);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Caregiver was updated successfully!";
            var Caregivers = await _context.TbCaregivers.ToArrayAsync();

            return View("Index", new GetCaregiversRes(Caregivers));
        }

        [HttpGet]
        public IActionResult DeleteCaregiver(int id) {
            return View(new DeleteCaregiverDto(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCaregiverPost(DeleteCaregiverDto deleteCaregiverDto) {
            var Caregiver = await _context.TbCaregivers.FindAsync(deleteCaregiverDto.Id);
            _context.TbCaregivers.Remove(Caregiver);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Caregiver was deleted successfully!";
            var Caregivers = await _context.TbCaregivers.ToArrayAsync();

            return View("Index", new GetCaregiversRes(Caregivers));
        }
    }
    public record GetCaregiversRes(Caregiver[] Caregivers);
    public record AddCaregiverDto(int CaregiverUserId, string Photo, string Gender, string CaregivingType, double HourlyRate);
    public record EditCaregiverViewDto(int Id, int CaregiverUserId, string Photo, string Gender, string CaregivingType, double HourlyRate);
    public record DeleteCaregiverDto(int Id);

    public static class CaregiverExt {
        public static Caregiver ToCaregiver(this AddCaregiverDto dto, int id) {
            return new Caregiver {
                CaregiverId = id,
                CaregiverUserId = dto.CaregiverUserId,
                Photo = dto.Photo,
                Gender = dto.Gender,
                CaregivingType = dto.CaregivingType,
                HourlyRate = dto.HourlyRate
            };
        }

        public static EditCaregiverViewDto ToEditCaregiverViewDto(this Caregiver caregiver) {
            return new EditCaregiverViewDto(caregiver.CaregiverId, caregiver.CaregiverUserId, caregiver.Photo, caregiver.Gender, caregiver.CaregivingType, caregiver.HourlyRate);
        }
    }
}
