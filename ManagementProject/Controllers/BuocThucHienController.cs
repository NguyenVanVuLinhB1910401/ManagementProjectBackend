using ManagementProject.DTOs;
using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
    public class BuocThucHienController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BuocThucHienController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        //[Authorize(Roles = "" + UserRoles.Admin + ", " + UserRoles.Director)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.Departments.GetAllDepartment();
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


        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var buoc = await _unitOfWork.BuocThucHiens.GetBuocHienTaiById(id);
                if (buoc == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Id không tồn tại" });
                }
                return Ok(buoc);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("code/{code}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var buoc = await _unitOfWork.BuocThucHiens.GetBuocHienTaiByCode(code);
                if (buoc == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Code không tồn tại" });
                }
                return Ok(buoc);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        //[Authorize( Roles = UserRoles.Admin )]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentReq model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Department department = new Department()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Status = 1,
                    Created = DateTime.Now,
                };
                await _unitOfWork.Departments.Add(department);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "Tạo phòng ban thành công.", Department = department });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userNameOfHandler = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return await _unitOfWork.UserManager.FindByNameAsync(userNameOfHandler);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDepartment(string id, [FromBody] DepartmentReq model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest(new { Status = "Failure", Message = "Id không giống nhau" });
            }
            try
            {
                var department = await _unitOfWork.Departments.Get(model.Id);
                if (department == null)
                {
                    return BadRequest(new { Status = "Failure", Message = "Không tìm thấy phòng ban." });
                }
                department.Name = model.Name;
                department.Description = model.Description;
                _unitOfWork.Departments.Update(department);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "Cập nhật thông tin phòng ban thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var department = await _unitOfWork.Departments.Get(id);
                if (department != null)
                {
                    department.Status = 0;
                    _unitOfWork.Departments.Update(department);
                    _unitOfWork.Complete();
                    return Ok(new { Status = "Success", Message = "Phòng ban đã được xóa." });
                }
                else
                {
                    return NotFound(new { Status = "Failure", Message = "Không tìm thấy phòng ban" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
