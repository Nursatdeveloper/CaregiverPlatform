using CaregiverPlatform;
using CaregiverPlatform.Common;
using CaregiverPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemberPlatform.Controllers {
    public class MembersController : Controller{
        private readonly CaregiverPlatformDbContext _context;
        public MembersController(CaregiverPlatformDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var Members = await _context.TbMembers.ToArrayAsync();
            return View(new GetMembersRes(Members));
        }


        [HttpGet]
        public IActionResult AddMember() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(AddMemberDto addMemberDto) {
            var Member = addMemberDto
                .ToMember(IdGen.GetId());

            await _context.TbMembers.AddAsync(Member);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Member was added successfully!";
            var Members = await _context.TbMembers.ToArrayAsync();

            return View("Index", new GetMembersRes(Members));
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id) {
            var Member = await _context.TbMembers.FindAsync(id);
            if(Member == null) {
                ViewData["errorMessage"] = "Member does not exist";
            }
            return View(Member?.ToEditMemberViewDto());
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(EditMemberViewDto editMemberDto) {
            var Member = await _context.TbMembers.FindAsync(editMemberDto.Id);
            if(Member == null) {
                throw new InvalidOperationException();
            }
            Member.MemberUserId = editMemberDto.MemberUserId;
            Member.HouseRules = editMemberDto.HouseRules.Split(",");

            _context.TbMembers.Update(Member);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Member was updated successfully!";
            var Members = await _context.TbMembers.ToArrayAsync();

            return View("Index", new GetMembersRes(Members));
        }

        [HttpGet]
        public IActionResult DeleteMember(int id) {
            return View(new DeleteMemberDto(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMemberPost(DeleteMemberDto deleteMemberDto) {
            var Member = await _context.TbMembers.FindAsync(deleteMemberDto.Id);
            _context.TbMembers.Remove(Member);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "Member was deleted successfully!";
            var Members = await _context.TbMembers.ToArrayAsync();

            return View("Index", new GetMembersRes(Members));
        }
    }
    public record GetMembersRes(Member[] Members);
    public record AddMemberDto(int MemberUserId, string HouseRules);
    public record EditMemberViewDto(int Id, int MemberUserId, string HouseRules);
    public record DeleteMemberDto(int Id);

    public static class MemberExt {
        public static Member ToMember(this AddMemberDto dto, int id) {
            return new Member {
                MemberId = id,
                MemberUserId = dto.MemberUserId,
                HouseRules = dto.HouseRules.Split(","),
            };
        }

        public static EditMemberViewDto ToEditMemberViewDto(this Member Member) {
            var rules = "";
            for(var a = 0; a < Member.HouseRules.Length; a++) {
                if(a != Member.HouseRules.Length - 1) {
                    rules += $"{Member.HouseRules[a]}, ";
                } else {
                    rules += $"{Member.HouseRules[a]}";

                }
            }
            return new EditMemberViewDto(Member.MemberId, Member.MemberUserId, rules);
        }
    }
}
