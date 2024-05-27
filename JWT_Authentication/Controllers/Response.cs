namespace JWT_Authentication.Controllers
{
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public string Token { get; set; }
        public object Data { get; set; }
    }
}