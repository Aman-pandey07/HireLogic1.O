namespace JobPortal1.O.DTOs.ApplicationDtos
{
    public class ApplicationListDTO
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal Salary { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ResumeUrl { get; set; }
        public string Status { get; set; }
    }
}
