namespace UserAPI.Models
{
    public class ResponseData
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;

        public ResponseData()
        {
        }
        public ResponseData(int id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
