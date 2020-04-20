namespace MVPWeek.Client.Models
{
    public class ErrorResult
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Error Errors { get; set; }
    }

    public class Error
    {
        public string[] Password { get; set; }
    }
}