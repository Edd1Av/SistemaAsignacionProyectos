namespace WEB.ViewModels
{
    public class NewPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NPassword { get; set; }
        public string NPasswordConfirm { get; set; }
    }

    public class ResetPassword
    {
        public string Email { get; set; }
    }
}
