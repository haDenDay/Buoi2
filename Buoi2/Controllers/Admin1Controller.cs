using Buoi2.Models;
using Buoi2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Buoi2.Controllers
{
    public class Admin1Controller : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public Admin1Controller(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _userManager.Users.ToListAsync();
            return View(user);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Tài khoản không tồn tại.");
            }
            var model = new UpdateUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email
            };
            return View(model);
        }
		[HttpPost]
		public async Task<IActionResult> Edit(UpdateUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				return NotFound("Tài khoản không tồn tại.");
			}

			Console.WriteLine("Giá trị DateOfBirth từ model: " + model.DateOfBirth);

			// Kiểm tra xem giá trị có hợp lệ không
			if (model.DateOfBirth.HasValue)
			{
				user.DateOfBirth = model.DateOfBirth.Value;
			}
			else
			{
				Console.WriteLine("Lỗi: DateOfBirth nhận giá trị null");
			}

			user.UserName = model.UserName;
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Email = model.Email;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cập nhật thất bại");
				return View(model);
			}

			return RedirectToAction("GetUsers", "Admin1");
		}


		[HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Tài khoản không tồn tại.");
            }
            ViewBag.UserName = user.UserName;
            var model = new UserClaimViewModel()
            {
                UserId = user.Id,
            };
            var existingClaims = await _userManager.GetClaimsAsync(user);
            foreach (Claim claim in ClaimStore.GetAllClaims())
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };
                if (existingClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Tài khoản không tồn tại.");
            }
            var currentUserClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in currentUserClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
            foreach (var claim in model.Claims)
            {
                if (claim.IsSelected)
                {
                    await _userManager.AddClaimAsync(user, new Claim("Permission", claim.ClaimType));
                }
            }
            return RedirectToAction("GetUsers", "Admin1");
        }
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("Tài khoản không tồn tại.");
			}

			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				return BadRequest("Xóa tài khoản thất bại.");
			}

			return RedirectToAction("GetUsers");
		}
	}
}
