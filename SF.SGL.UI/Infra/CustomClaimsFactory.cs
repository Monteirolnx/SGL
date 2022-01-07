namespace SF.SGL.UI.Infra
{
    public class CustomClaimsFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public CustomClaimsFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            ClaimsPrincipal user = await base.CreateUserAsync(account, options);

            if (!user.Identity.IsAuthenticated)
            {
                return user;
            }

            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> roleClaims = identity.FindAll(identity.RoleClaimType);

            if (roleClaims == null || !roleClaims.Any())
            {
                return user;
            }

            object rolesElem = account.AdditionalProperties[identity.RoleClaimType];

            if (rolesElem is JsonElement roles)
            {
                if (roles.ValueKind == JsonValueKind.Array)
                {
                    identity.RemoveClaim(identity.FindFirst(options.RoleClaim));
                    foreach (var role in roles.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(options.RoleClaim, role.GetString()));
                    }
                }
            }

            return user;
        }
    }
}
