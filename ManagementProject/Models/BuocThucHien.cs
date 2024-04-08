namespace ManagementProject.Models
{
    public class BuocThucHien
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TenBuoc { get; set; }
        public string NguoiThucHienId { get; set; }
        public ApplicationUser NguoiThucHien { get; set; }
        public string BuocTiepTheo { get; set; }
        public string BuocTruocDo { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int QuyTrinhId { get; set; }
        public QuyTrinh QuyTrinh { get; set; }
    }
}
