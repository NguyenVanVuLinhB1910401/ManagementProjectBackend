namespace ManagementProject.DTOs
{
    public class ProjectRes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int Status { get; set; }
        //public int isProject { get; set; }
        //public string CreatedId { get; set; }
        public List<MemberRes>? Members { get; set; }

        //public int isDelete { get; set; }
    }

    public class MemberRes
    {
        public string MemberId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
