namespace PsiogPaisaAPI.Models
{
    public class AuthenticatedResponse
    {
        public int Id { get; set; }

        public string EmpFname { get; set; }
        public string? Token { get; set; }
    }
}
