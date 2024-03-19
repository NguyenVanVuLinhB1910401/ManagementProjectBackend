using ManagementProject.Interfaces;
using ManagementProject.Models;
using ManagementProject.DTOs;
using ManagementProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        //[Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.UserManager.Users.Where(u => u.UserName != "admin" && u.Status == 1).ToListAsync();
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _unitOfWork.UserManager.FindByIdAsync(id);
                if (result == null)
                {
                    return BadRequest("Không tìm thấy tài khoản");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Department/{departmentId}")]
        //[Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> GetByDepartment(string departmentId)
        {
            try
            {
                var result = await _unitOfWork.UserManager.Users.Where(u => u.UserName != "admin" && u.Status == 1 && u.DepartmentId == departmentId).ToListAsync();
                if (result == null)
                {
                    return BadRequest("Không tìm thấy tài khoản");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeReq model)
        {
            return await RegisterUser(model, new string[] { UserRoles.Employee });
        }

        private async Task<IActionResult> RegisterUser([FromBody] EmployeeReq model, string[] roles)
        {
            string userName = RemoveDiacritics(model.LastName) + model.DateOfBirth.ToString("ddMMyyyy");
            var userExists = await _unitOfWork.UserManager.FindByNameAsync(userName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "User already exists!" });
            var emailExists = await _unitOfWork.UserManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "Email already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                CCCD = model.CCCD,
                Address = model.Address,
                MobilePhone = model.MobilePhone,
                Location = model.Location,
                Status = 1,
                isLock = 0
            };
            var department = await _unitOfWork.Departments.Get(model.DepartmentId);
            if (department != null)
            {
                user.DepartmentId = department.Id;
                user.Position = model.Position;
            }
            if (model.DateOfBirth != null) user.DateOfBirth = DateTime.Parse(model.DateOfBirth.ToString()!);
            var result = await _unitOfWork.UserManager.CreateAsync(user, userName + "@123");
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Thêm nhân viên thất bại." });

            foreach (var role in roles)
            {
                if (!await _unitOfWork.RoleManager.RoleExistsAsync(role))
                    await _unitOfWork.RoleManager.CreateAsync(new IdentityRole(role));

                if (await _unitOfWork.RoleManager.RoleExistsAsync(role))
                    await _unitOfWork.UserManager.AddToRoleAsync(user, role);
            }

            return Ok(new Response { Status = "Success", Message = "Thêm nhân viên thành công." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeReq model, string id)
        {

            var employee = await _unitOfWork.UserManager.FindByIdAsync(id);
            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.Gender = model.Gender;
                employee.CCCD = model.CCCD;
                employee.Address = model.Address;
                employee.MobilePhone = model.MobilePhone;
                var department = await _unitOfWork.Departments.Get(model.DepartmentId);
                if (department != null)
                {
                    employee.DepartmentId = department.Id;
                    employee.Position = model.Position;
                }
                else
                {
                    employee.DepartmentId = null;
                    employee.Position = null;
                }
                if (model.DateOfBirth != null) employee.DateOfBirth = DateTime.Parse(model.DateOfBirth.ToString());
                var result = await _unitOfWork.UserManager.UpdateAsync(employee);
                if (result.Succeeded)
                {
                    return Ok(new { Status = "Success", Message = "Thông tin nhân viên đã được cập nhật." });
                }
                else
                {
                    return Ok(new { Status = "Failure", Message = "Cập nhật thất bại" });
                }

            }
            else
            {
                return BadRequest(new { Status = "Failure", Message = "Không tìm thấy nhân viên" });
            }   
        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var employee = await _unitOfWork.UserManager.FindByIdAsync(id);
                if (employee != null)
                {
                    employee.Status = 0;
                    var result = await _unitOfWork.UserManager.UpdateAsync(employee);
                    if (result.Succeeded)
                    {
                        return Ok(new { Status = "Success", Message = "Nhân viên đã được xóa." });
                    }
                    else
                    {
                        return Ok(new { Status = "Failure", Message = "Xóa nhân viên thất bại" });
                    }    
                    
                }
                else
                {
                    return NotFound(new { Status = "Failure", Message = "Không tìm thấy nhân viên" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        private string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex("\\p{M}");

            return regex.Replace(normalizedString, string.Empty);
        }
    }
}
