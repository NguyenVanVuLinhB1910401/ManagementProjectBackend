using ManagementProject.DTOs;
using ManagementProject.Interfaces;
using ManagementProject.Models;
using ManagementProject.UtilityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Security.Claims;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WorkController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        //[Authorize(Roles = UserRoles.Admin +"," + UserRoles.Employee +"," + UserRoles.Director)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.Works.GetAllWork();
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

        [HttpGet("{workId}")]
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Employee + "," + UserRoles.Director)]
        public async Task<IActionResult> GetWork(int workId)
        {
            try
            {
                var result = await _unitOfWork.Works.GetWorkById(workId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update-progress")]
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Employee + "," + UserRoles.Director)]
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateProgressReq model)
        {
            try
            {
                var userId = GetCurrentUserId();
                var work = await _unitOfWork.Works.Get(model.Id);
                if (work == null)
                {
                    return NotFound();
                }
                if (work.AssignUserId != userId) { return BadRequest(); }
                work.Progress = model.Progress;
                _unitOfWork.Complete();
                return Ok(new { Status = 200, Message = "Báo cáo tiến độ thành công"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update-status")]
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Employee + "," + UserRoles.Director)]
        
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusWorkReq model)
        {
            try
            {
                var userId = GetCurrentUserId();
                var work = await _unitOfWork.Works.Get(model.Id);
                if (work == null)
                {
                    return NotFound();
                }
                if (work.AssignUserId != userId || !Enum.IsDefined(typeof(WorkStatus), model.Status)) { return BadRequest(); }
                work.Status = model.Status;
                _unitOfWork.Complete();
                return Ok(new { Status = 200, Message = "Cập nhật trạng thái công việc thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("assigned")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        public async Task<IActionResult> GetAllWorkAssignedToMe()
        {
            try
            {
                string userLogin = GetCurrentUserId();
                var result = await _unitOfWork.Works.GetAllWorkAssignedToMe(userLogin);
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

        [HttpGet("assign")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        public async Task<IActionResult> GetAllWorkIAssign()
        {
            try
            {
                string userLogin = GetCurrentUserId();
                var result = await _unitOfWork.Works.GetAllWorkIAssign(userLogin);
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

        [HttpGet("sub-work/{parentWorkId}")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        public async Task<IActionResult> GetAllSubWork(int parentWorkId)
        {
            try
            {
                var result = await _unitOfWork.Works.GetAllSubWork(parentWorkId);
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

        [HttpPost]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        [RequestSizeLimit(2028 * 1024 * 1024)]
        public async Task<IActionResult> CreateWork([FromForm] WorkReq body)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                Work work = new Work()
                {
                    Title = body.Title,
                    Content = body.Content,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    Status = (int)WorkStatus.Created,
                    CreatedUserId = GetCurrentUserId(),
                    AssignUserId = body.AssignUserId,
                    ProjectId = body.ProjectId,
                    Created = body.Created,
                    ParentWorkId = body.ParentWorkId
                };
              
                await _unitOfWork.Works.Add(work);
                _unitOfWork.Complete();

                if(!body.AttachmentFiles.IsNullOrEmpty())
                {
                    var pathFiles = await HandleFile.SaveFilesAsync(body.AttachmentFiles, "www/uploads/works");
                    if(!pathFiles.IsNullOrEmpty())
                    {
                       foreach(var file in pathFiles)
                       {
                            file.WorkId = work.Id;
                            await _unitOfWork.AttachmentFiles.Add(file);
                       }
                       _unitOfWork.Complete();
                    }
                }    
                _unitOfWork.CommitTransaction();
                return Ok(new { Status = 200, Message = "Giao việc thành công"});
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("chuyenbuoctieptheo")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        [RequestSizeLimit(2028 * 1024 * 1024)]
        public async Task<IActionResult> ChuyenBuocTiepTheo([FromForm] ChuyenBuocTiepTheoReq body)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var project = await _unitOfWork.Projects.GetProject(body.ProjectId);
                if (project == null) return BadRequest("Project not found");
                var buocHienTai = await _unitOfWork.BuocThucHiens.GetBuocHienTaiById(project.BuocHienTaiId);
                if (buocHienTai == null) return BadRequest("Không tìm thấy bước hiện tại");
                var buocTiepTheo = await _unitOfWork.BuocThucHiens.GetBuocHienTaiByCode(buocHienTai.BuocTiepTheo);
                if (buocTiepTheo == null) return BadRequest("Không tìm thấy bước tiếp theo");
                //Cập nhật tiến độ thực hiện dự án
                project.BuocHienTaiId = buocTiepTheo.Id;

                Work work = new Work()
                {
                    Title = body.Title,
                    Content = body.Content,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    Status = (int)WorkStatus.Created,
                    CreatedUserId = buocHienTai.NguoiThucHienId,
                    AssignUserId = buocTiepTheo.NguoiThucHienId,
                    ProjectId = body.ProjectId,
                    Created = body.Created,
                    isChuyenBuoc = 1

                };
                await _unitOfWork.Works.Add(work);
                _unitOfWork.Complete();

                if (!body.AttachmentFiles.IsNullOrEmpty())
                {
                    var pathFiles = await HandleFile.SaveFilesAsync(body.AttachmentFiles, "www/uploads/works");
                    if (!pathFiles.IsNullOrEmpty())
                    {
                        foreach (var file in pathFiles)
                        {
                            file.WorkId = work.Id;
                            await _unitOfWork.AttachmentFiles.Add(file);
                        }
                        _unitOfWork.Complete();
                    }
                }
                _unitOfWork.CommitTransaction();
                return Ok(new { Status = 200, Message = "Chuyển bước thành công" });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("quayvebuoctruoc")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc + "," + UserRoles.TruongNhom + "," + UserRoles.ThanhVien)]
        [RequestSizeLimit(2028 * 1024 * 1024)]
        public async Task<IActionResult> QuayVeBuocTruoc([FromForm] QuayVeBuocTruoc body)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var project = await _unitOfWork.Projects.GetProject(body.ProjectId);
                if (project == null) return BadRequest("Project not found");
                var buocHienTai = await _unitOfWork.BuocThucHiens.GetBuocHienTaiById(project.BuocHienTaiId);
                if (buocHienTai == null) return BadRequest("Không tìm thấy bước hiện tại");
                var buocTruocDo = await _unitOfWork.BuocThucHiens.GetBuocHienTaiByCode(buocHienTai.BuocTruocDo);
                if (buocTruocDo == null) return BadRequest("Không tìm thấy bước tiếp theo");
                //Cập nhật tiến độ thực hiện dự án
                project.BuocHienTaiId = buocTruocDo.Id;

                Work work = new Work()
                {
                    Title = body.Title,
                    Content = body.Content,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    Status = (int)WorkStatus.Created,
                    CreatedUserId = buocHienTai.NguoiThucHienId,
                    AssignUserId = buocTruocDo.NguoiThucHienId,
                    ProjectId = body.ProjectId,
                    Created = body.Created,
                    isChuyenBuoc = 1

                };
                await _unitOfWork.Works.Add(work);
                _unitOfWork.Complete();

                if (!body.AttachmentFiles.IsNullOrEmpty())
                {
                    var pathFiles = await HandleFile.SaveFilesAsync(body.AttachmentFiles, "www/uploads/works");
                    if (!pathFiles.IsNullOrEmpty())
                    {
                        foreach (var file in pathFiles)
                        {
                            file.WorkId = work.Id;
                            await _unitOfWork.AttachmentFiles.Add(file);
                        }
                        _unitOfWork.Complete();
                    }
                }
                _unitOfWork.CommitTransaction();
                return Ok(new { Status = 200, Message = "Chuyển bước thành công" });
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


    }
}
