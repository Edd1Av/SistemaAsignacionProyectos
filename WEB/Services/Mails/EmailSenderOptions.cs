namespace WEB.Services.Mails
{
    public class EmailSenderOptions
    {
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Img { get; set; }
    }
}
