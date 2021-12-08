using Microsoft.IdentityModel.JsonWebTokens;
using System.Runtime.InteropServices;

namespace TsheThauLoo.Utils
{
    /// <summary>
    /// https://www.iana.org/assignments/jwt/jwt.xhtml#claims
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct JwtClaims
    {
        public const string Issuer = JwtRegisteredClaimNames.Iss;

        public const string Audience = JwtRegisteredClaimNames.Aud;

        public const string ExpirationTime = JwtRegisteredClaimNames.Exp;

        public const string NotBefore = JwtRegisteredClaimNames.Nbf;

        public const string IssuedAt = JwtRegisteredClaimNames.Iat;

        public const string JWTID = JwtRegisteredClaimNames.Jti;

        public const string Subject = JwtRegisteredClaimNames.Sub;

        public const string Username = "username";

        public const string FullName = JwtRegisteredClaimNames.Name;

        public const string FamilyName = JwtRegisteredClaimNames.FamilyName;

        public const string GivenName = JwtRegisteredClaimNames.GivenName;

        public const string UniqueName = JwtRegisteredClaimNames.UniqueName;

        public const string NameId = JwtRegisteredClaimNames.NameId;

        public const string Website = JwtRegisteredClaimNames.Website;

        public const string Email = JwtRegisteredClaimNames.Email;

        public const string EmailVerified = "email_verified";

        public const string Gender = JwtRegisteredClaimNames.Gender;

        public const string Birthdate = JwtRegisteredClaimNames.Birthdate;

        public const string PhoneNumber = JwtRegisteredClaimNames.PhoneNumber;

        public const string PhoneNumberVerified = JwtRegisteredClaimNames.PhoneNumberVerified;

        public const string Roles = "roles";

        public const string Groups = "groups";

        public const string Azp = JwtRegisteredClaimNames.Azp;

        public const string Nonce = JwtRegisteredClaimNames.Nonce;

        public const string AuthTime = JwtRegisteredClaimNames.AuthTime;

        public const string AtHash = JwtRegisteredClaimNames.AtHash;

        public const string CHash = JwtRegisteredClaimNames.CHash;

        public const string Acr = JwtRegisteredClaimNames.Acr;

        public const string Amr = JwtRegisteredClaimNames.Amr;

        public const string Actort = JwtRegisteredClaimNames.Actort;

        public const string Prn = JwtRegisteredClaimNames.Prn;

        public const string Sid = JwtRegisteredClaimNames.Sid;

        public const string Typ = JwtRegisteredClaimNames.Typ;
    }
}
