using ManagementProject.DTOs;
using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        //[Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.Projects.GetAllProject();
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
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var project = await _unitOfWork.Projects.GetById(id);
                if (project == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Dự án không tồn tại" });
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        //[Authorize( Roles = UserRoles.Admin )]
        public async Task<IActionResult> AddProject([FromBody] ProjectReq model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _unitOfWork.BeginTransaction();

                //Create Project
                Project project = new Project()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Type = model.Type,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    isProject = 1,
                    isDelete = 1,   
                    Status = 1,
                    Created = DateTime.Now,
                    CreatedId = GetCurrentUserId()
                };
                await _unitOfWork.Projects.Add(project);
                _unitOfWork.Complete();

                //Add List Member
                if(model.Members.Count > 0)
                {
                    foreach(var member in model.Members)
                    {
                        await _unitOfWork.ProjectMembers.Add(new ProjectMember { MemberId = member.MemberId, ProjectId = project.Id, Position = member.Position});
                    }
                }
                _unitOfWork.Complete();
                _unitOfWork.CommitTransaction();
                return Ok(new { Status = "Success", Message = "Tạo dự án thành công." });
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
            var userId = claimsIdentity.FindFirst("Id")?.Value;
            return userId;
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
        public async Task<IActionResult> DeleteEquipment(string id)
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
