namespace SF.SGL.UI.Pages.Login
{
    public class UserInfoState
    {
        public string LoginDeRede { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool PodeCriarUnidades { get; set; }
        public bool PodeVerUnidades { get; set; }
        public bool PodeEditarUnidades { get; set; }
        public bool PodeExcluirUnidades { get; set; }

        public UserInfoState()
        {
            UserClaims = Enumerable.Empty<Claim>();
            Roles = Enumerable.Empty<string>();
        }

    }
}
