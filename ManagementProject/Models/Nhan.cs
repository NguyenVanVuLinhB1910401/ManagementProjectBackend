using AutoMapper.Configuration.Conventions;

namespace ManagementProject.Models
{
    public class Nhan
    {
        public int Id { get; set; }
        public string TenNhan { get; set; }
        public string LoaiNhan { get; set; }
        public int isLock { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
