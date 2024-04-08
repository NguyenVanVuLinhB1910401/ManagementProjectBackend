using ManagementProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class TaoQuyTrinhReq
    {

        [Required(ErrorMessage = "TenQuyTrinh is required")]
        public string TenQuyTrinh { get; set; }

        [Required(ErrorMessage = "NgayTao is required")]
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        [Required(ErrorMessage = "NguoiTaoId is required")]
        public string NguoiTaoId { get; set; }
        public List<BuocThucHienReq> BuocThucHiens { get; set; }


        public class BuocThucHienReq
        {
            [Required(ErrorMessage = "Code is required")]
            public string Code { get; set; }

            [Required(ErrorMessage = "TenBuoc is required")]
            public string TenBuoc { get; set; }

            [Required(ErrorMessage = "NguoiThucHienId is required")]
            public string NguoiThucHienId { get; set; }

            [Required(ErrorMessage = "BuocTiepTheo is required")]
            public string BuocTiepTheo { get; set; }

            [Required(ErrorMessage = "BuocTruocDo is required")]
            public string BuocTruocDo { get; set; }
        }
    }
}
