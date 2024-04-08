namespace UserAPI.Models
{
    public class ResponseLogin
    {
        public int Id { get; set; }
        public string LoginToken { get; set; } = string.Empty;
        public string Status { get; set; }
        public ResponseData responseData {get; set;}

        public ResponseLogin()
        {
        }

        public ResponseLogin(string loginToken, string status, ResponseData responseData)
        {
            LoginToken = loginToken;
            Status = status;
            this.responseData = responseData;
        }
    }
}
