using AutoMapper;
using ManagementProject.DTOs;
using ManagementProject.Models;
using static ManagementProject.DTOs.QuyTrinhRes;

namespace ManagementProject.UtilityServices
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //CreateMap<QuyTrinh, QuyTrinhRes>();
            //CreateMap<ApplicationUser, ApplicationUserRes>();
            CreateMap<Project, GetAllProjectRes>()
                 .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => StatusToStatusName(src)));

            CreateMap<Project, ProjectDetailRes>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => StatusToStatusName(src)));

            CreateMap<Work, CongViecThucHienRes>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => StatusWorkToStatusName(src)));

        }

        
        
        private string StatusToStatusName(Project source)
        {
            var status = source.Status;
            string statusName = "";

            switch (status)
            {
                case 1:
                    statusName = "Vừa mới tạo";
                    break;
                case 2:
                    statusName = "Đang thực hiện";
                    break;
                case 3:
                    statusName = "Đang tạm dừng";
                    break;
                case 4:
                    statusName = "Đã hoàn thành";
                    break;
                default:
                    statusName = "Unknown";
                    break;
            }

            return statusName;
        }

        private string StatusWorkToStatusName(Work source)
        {
            var status = source.Status;
            string statusName = "";

            switch (status)
            {
                case 1:
                    statusName = "Vừa mới tạo";
                    break;
                case 2:
                    statusName = "Đang thực hiện";
                    break;
                case 3:
                    statusName = "Đang tạm dừng";
                    break;
                case 4:
                    statusName = "Đã hoàn thành";
                    break;
                default:
                    statusName = "Unknown";
                    break;
            }

            return statusName;
        }

    }
}
