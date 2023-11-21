using CaregiverPlatform.Common;
using CaregiverPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CaregiverPlatform.Controllers {
    public class UsersController : Controller {
        private readonly CaregiverPlatformDbContext _context;
        public UsersController(CaregiverPlatformDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var users = await _context.TbUsers.ToArrayAsync();
            return View(new GetUsersRes(users));
        }


        [HttpGet]
        public IActionResult AddUser() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto addUserDto) {
            var user = addUserDto
                .ToUser()
                .SetUserId(IdGen.GetId())
                .SetCreatedAt(DateTime.Now)
                .SetIsActive(true);

            await _context.TbUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "User was added successfully!";
            var users = await _context.TbUsers.ToArrayAsync();

            return View("Index", new GetUsersRes(users));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id) {
            var user = await _context.TbUsers.FindAsync(id);
            if(user == null) {
                ViewData["errorMessage"] = "User does not exist";
            } 
            return View(user?.ToEditUserViewDto());
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewDto editUserDto) {
            var user = await _context.TbUsers.FindAsync(editUserDto.Id);
            if(user == null) {
                throw new InvalidOperationException();
            }
            user.GivenName = editUserDto.Name;
            user.Surname = editUserDto.Surname;
            user.Email = editUserDto.Email;
            user.PhoneNumber = editUserDto.PhoneNumber;
            user.City = editUserDto.City;
            user.ProfileDescription = editUserDto.ProfileDescription;

            _context.TbUsers.Update(user);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "User was updated successfully!";
            var users = await _context.TbUsers.ToArrayAsync();

            return View("Index", new GetUsersRes(users));
        }

        [HttpGet]
        public IActionResult DeleteUser(int id) {
            return View(new DeleteUserDto(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserPost(DeleteUserDto deleteUserDto) {
            var user = await _context.TbUsers.FindAsync(deleteUserDto.Id);
            _context.TbUsers.Remove(user);
            await _context.SaveChangesAsync();
            ViewData["postbackMessage"] = "User was deleted successfully!";
            var users = await _context.TbUsers.ToArrayAsync();

            return View("Index", new GetUsersRes(users));
        }
    }
    
    public record GetUsersRes(User[] users);
    public record AddUserDto(string Email, string Name, string Surname, string City,string PhoneNumber, string ProfileDescription, string Password);
    public record EditUserViewDto(int Id, string Email, string Name, string Surname, string City,string PhoneNumber, string ProfileDescription);
    public record DeleteUserDto(int Id);
    public static class UsersExt {
        public static User UpdateUserDataWith(this User user, EditUserViewDto dto) {
            return new User {
                UserId = user.UserId,
                Email = dto.Email,
                GivenName = dto.Name,
                Surname = dto.Surname,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber,
                ProfileDescription = dto.ProfileDescription,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }
        public static EditUserViewDto ToEditUserViewDto(this User user)
            => new(user.UserId, user.Email, user.GivenName, user.Surname, user.City, user.PhoneNumber, user.ProfileDescription);
        public static User ToUser(this AddUserDto addUserDto) {
            return new User() {
                Email = addUserDto.Email,
                GivenName = addUserDto.Name,
                Surname = addUserDto.Surname,
                City = addUserDto.City,
                PhoneNumber = addUserDto.PhoneNumber,
                ProfileDescription = addUserDto.ProfileDescription,
                Password = addUserDto.Password
            };
        }
        public static User SetUserId(this User addUserDto, int userId) {
            return new User() {
                UserId = userId,
                Email = addUserDto.Email,
                GivenName = addUserDto.GivenName,
                Surname = addUserDto.Surname,
                City = addUserDto.City,
                PhoneNumber = addUserDto.PhoneNumber,
                ProfileDescription = addUserDto.ProfileDescription,
                Password = addUserDto.Password
            };
        }

        public static User SetCreatedAt(this User addUserDto, DateTime now) {
            if(addUserDto.UserId == 0) {
                throw new InvalidOperationException();
            }
            return new User() {
                UserId = addUserDto.UserId,
                Email = addUserDto.Email,
                GivenName = addUserDto.GivenName,
                Surname = addUserDto.Surname,
                City = addUserDto.City,
                PhoneNumber = addUserDto.PhoneNumber,
                ProfileDescription = addUserDto.ProfileDescription,
                Password = addUserDto.Password,
                CreatedAt = now
            };
        }

        public static User SetIsActive(this User addUserDto, bool isActive) {
            if(addUserDto.UserId == 0) {
                throw new InvalidOperationException();
            }
            return new User() {
                UserId = addUserDto.UserId,
                Email = addUserDto.Email,
                GivenName = addUserDto.GivenName,
                Surname = addUserDto.Surname,
                City = addUserDto.City,
                PhoneNumber = addUserDto.PhoneNumber,
                ProfileDescription = addUserDto.ProfileDescription,
                Password = addUserDto.Password,
                CreatedAt = addUserDto.CreatedAt,
                IsActive = isActive
            };
        }
    }

}
