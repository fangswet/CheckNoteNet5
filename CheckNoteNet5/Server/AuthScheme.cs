using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CheckNoteNet5.Server
{
    public static class AuthScheme
    {
        public const string Jwt = JwtBearerDefaults.AuthenticationScheme;
        public const string All = Jwt + ",Identity.Application"; // IdentityConstants.ApplicationScheme
    }
}
