namespace ManagementProject.Models
{
    public class QuyTrinh
    {
        public int Id { get; set; }
        public string TenQuyTrinh { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int isDelete { get; set; }
        public string NguoiTaoId { get; set; }
        public ApplicationUser NguoiTao { get; set; }
        public List<Project> Projects { get; set; }
        public List<BuocThucHien> BuocThucHiens { get; set; }
    }
}
