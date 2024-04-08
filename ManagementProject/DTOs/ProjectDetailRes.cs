using ManagementProject.Models;

namespace ManagementProject.DTOs
{
    public class ProjectDetailRes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int isProject { get; set; }
        public string CreatedId { get; set; }
        public ApplicationUser CreatedUser { get; set; }
        //public List<ProjectMember> Members { get; set; }

        public int isDelete { get; set; }

        public DateTime Created { get; set; }
        //public List<Work> Works { get; set; }

        public int QuyTrinhId { get; set; }
        //public QuyTrinh QuyTrinh { get; set; }
        //public int BuocHienTaiId { get; set; }
        public BuocThucHien BuocHienTai { get; set; }
    }
}
