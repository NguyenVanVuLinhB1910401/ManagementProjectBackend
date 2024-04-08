using AutoMapper;
using ManagementProject.DTOs;
using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.GiamDoc)]
    public class QuyTrinhController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QuyTrinhController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        //[Authorize(Roles = "" + UserRoles.Admin + ", " + UserRoles.Director)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.QuyTrinhs.GetAllQuyTrinh();
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
                var quyTrinh = await _unitOfWork.QuyTrinhs.GetDetail(id);
                
                if (quyTrinh == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Quy trình không tồn tại" });
                }
                quyTrinh.BuocThucHiens = this.SapXepBuoc(quyTrinh.BuocThucHiens);
                return Ok(quyTrinh);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        [Authorize( Roles = UserRoles.GiamDoc )]
        public async Task<IActionResult> TaoQuyTrinh([FromBody] TaoQuyTrinhReq model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _unitOfWork.BeginTransaction();
                QuyTrinh quyTrinh = new QuyTrinh()
                {
                    TenQuyTrinh = model.TenQuyTrinh,
                    NguoiTaoId = this.GetCurrentUserId(),
                    NgayTao = model.NgayTao,
                    isDelete = 0
                };
                await _unitOfWork.QuyTrinhs.Add(quyTrinh);
                _unitOfWork.Complete();
                if (!model.BuocThucHiens.IsNullOrEmpty())
                {
                    foreach(var buoc in  model.BuocThucHiens)
                    {
                        BuocThucHien buocThucHien = new BuocThucHien()
                        {
                            Code = buoc.Code,
                            TenBuoc = buoc.TenBuoc,
                            NguoiThucHienId = buoc.NguoiThucHienId,
                            BuocTiepTheo = buoc.BuocTiepTheo,
                            BuocTruocDo = buoc.BuocTruocDo,
                            NgayTao = model.NgayTao,
                            QuyTrinhId = quyTrinh.Id
                        };
                        await _unitOfWork.BuocThucHiens.Add(buocThucHien);
                    }
                    _unitOfWork.Complete();
                }

                _unitOfWork.CommitTransaction();
                return Ok(new { Status = "Success", Message = "Tạo quy trình thành công." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        private string GetCurrentUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst("Id")?.Value;
            return userId;
        }
        //private async Task<ApplicationUser> GetCurrentUser()
        //{
        //    var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //    var userNameOfHandler = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //    return await _unitOfWork.UserManager.FindByNameAsync(userNameOfHandler);
        //}

        private List<BuocThucHien> SapXepBuoc(List<BuocThucHien> buocThucHiens)
        {
            var sortedBuoc = new List<BuocThucHien>();
            var firstBuoc = buocThucHiens.FirstOrDefault(b => b.BuocTruocDo == "0");

            if (firstBuoc == null)
                return sortedBuoc;  // Return an empty list if the first step is not found

            sortedBuoc.Add(firstBuoc);

            while (sortedBuoc.Count < buocThucHiens.Count)
            {
                var lastBuoc = sortedBuoc.LastOrDefault();
                var nextBuoc = buocThucHiens.FirstOrDefault(b => b.BuocTruocDo == lastBuoc.Code);

                if (nextBuoc != null)
                {
                    sortedBuoc.Add(nextBuoc);
                }
                else
                {
                    break;  // Break the loop if the next step is not found
                }
            }

            return sortedBuoc;
        }

        //[HttpPut]
        //[Route("{id}")]
        //public async Task<IActionResult> UpdateDepartment(string id, [FromBody] DepartmentReq model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (id != model.Id)
        //    {
        //        return BadRequest(new { Status = "Failure", Message = "Id không giống nhau" });
        //    }
        //    try
        //    {
        //        var department = await _unitOfWork.Departments.Get(model.Id);
        //        if (department == null)
        //        {
        //            return BadRequest(new { Status = "Failure", Message = "Không tìm thấy phòng ban." });
        //        }
        //        department.Name = model.Name;
        //        department.Description = model.Description;
        //        _unitOfWork.Departments.Update(department);
        //        _unitOfWork.Complete();
        //        return Ok(new { Status = "Success", Message = "Cập nhật thông tin phòng ban thành công" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }

        //}

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteQuyTrinh(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var quyTrinh = await _unitOfWork.QuyTrinhs.GetById(id);
                if (quyTrinh != null)
                {

                    quyTrinh.isDelete = 1;
                    _unitOfWork.Complete();
                    return Ok(new { Status = "Success", Message = "Quy trình đã được xóa." });
                }
                else
                {
                    return NotFound(new { Status = "Failure", Message = "Không tìm thấy quy trình" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
