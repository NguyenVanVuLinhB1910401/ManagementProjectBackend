﻿using AutoMapper;
using ManagementProject.DTOs;
using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        //[Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var listProject = await _unitOfWork.Projects.GetAllProject();
                if (listProject.Count() == 0)
                {
                    return NoContent();
                }
                var resultMap = _mapper.Map<List<GetAllProjectRes>>(listProject);
             
                return Ok(resultMap);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Joined")]
        //[Authorize(Roles = UserRoles.Admin + "," +UserRoles.Employee)]
        public async Task<IActionResult> GetAllProjectJoined()
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _unitOfWork.Projects.GetAllProjectJoined(userId);
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

        [HttpGet("Members/{projectId}")]
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Employee)]
        public async Task<IActionResult> GetInfoAllMemberByProject(string projectId)
        {
            try
            {
               
                var result = await _unitOfWork.ProjectMembers.GetInfoMembersByProject(projectId);
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

        [HttpGet("lichsuthuchiencongviec/{projectId}")]
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Employee)]
        public async Task<IActionResult> LichSuThucHienCongViec(string projectId)
        {
            try
            {

                var result = await _unitOfWork.Works.LichSuThucHienCongViec(projectId);
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                var resultMap = _mapper.Map<List<CongViecThucHienRes>>(result);
                return Ok(resultMap);
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
                var result = new ProjectRes()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Address = project.Address,
                    Type = project.Type,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    CompleteDate = project.CompleteDate,

                };
                if(project.Members.Count > 0)
                {
                    var listMember = new List<MemberRes>();
                    foreach (var member in project.Members)
                    {
                        listMember.Add(new MemberRes() { MemberId = member.MemberId, FullName = member.Member.FirstName + " " + member.Member.LastName, Position = member.Position });
                    }
                    result.Members = listMember;
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("detail/{id}")]
        public async Task<IActionResult> GetDetailProject(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var project = await _unitOfWork.Projects.GetDetailProject(id);
                if (project == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Dự án không tồn tại" });
                }
                var result = _mapper.Map<ProjectDetailRes>(project);

                return Ok(new { Status = "Success", Project = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc)]
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
                BuocThucHien buocDauTien = await _unitOfWork.BuocThucHiens.GetBuocKhoiTao(model.QuyTrinhId);
                if(buocDauTien == null) { return BadRequest("Không tìm thấy bước khởi tạo"); }
                Project project = new Project()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Type = model.Type,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    isProject = 1,
                    isDelete = 0,   
                    Status = 1,
                    Created = DateTime.Now,
                    CreatedId = GetCurrentUserId(),
                    QuyTrinhId = model.QuyTrinhId,
                    BuocHienTaiId = buocDauTien.Id
                };
                await _unitOfWork.Projects.Add(project);
                _unitOfWork.Complete();

                //Add List Member
                if(model?.Members?.Count > 0)
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
            var userId = claimsIdentity?.FindFirst("Id")?.Value;
            return userId;
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc)]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] ProjectReq model)
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
                _unitOfWork.BeginTransaction();
                var project = await _unitOfWork.Projects.Get(model.Id);
                if (project == null)
                {
                    return BadRequest(new { Status = "Failure", Message = "Không tìm thấy dự án." });
                }
                project.Name = model.Name;
                project.Address = model.Address;
                project.Type = model.Type;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.CompleteDate = model.CompleteDate;

                var listMemberOld = await _unitOfWork.ProjectMembers.GetMembersByProject(project.Id);
                if(!listMemberOld.IsNullOrEmpty())
                {
                    foreach(var member in listMemberOld)
                    {
                        _unitOfWork.ProjectMembers.Delete(member);
                    }
                }
                if(!model.Members.IsNullOrEmpty())
                {
                    foreach (var member in model.Members)
                    {
                        await _unitOfWork.ProjectMembers.Add(new ProjectMember() { MemberId = member.MemberId, ProjectId = project.Id, Position = member.Position});
                    }
                }
                _unitOfWork.Complete();
                _unitOfWork.CommitTransaction();
                return Ok(new { Status = "Success", Message = "Cập nhật thông tin dự án thành công" });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("update-status")]
        [Authorize(Roles = UserRoles.GiamDoc)]

        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusProjectReq model)
        {
            try
            {
                var project = await _unitOfWork.Projects.GetById(model.Id);
                if (project == null)
                {
                    return NotFound();
                }
                
                project.Status = model.Status;
                _unitOfWork.Complete();
                return Ok(new { Status = 200, Message = "Trạng thái dự án đã được cập nhật" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = UserRoles.GiamDoc + "," + UserRoles.PhoGiamDoc)]
        public async Task<IActionResult> DeleteProject(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var project = await _unitOfWork.Projects.Get(id);
                if (project != null)
                {
                    project.isDelete = 1;
                    _unitOfWork.Projects.Update(project);
                    _unitOfWork.Complete();
                    return Ok(new { Status = "Success", Message = "Dự án đã được xóa." });
                }
                else
                {
                    return NotFound(new { Status = "Failure", Message = "Không tìm thấy dự án" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        
    }
}
