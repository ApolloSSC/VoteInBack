using Microsoft.IdentityModel.Tokens;
using System;

namespace VoteIn.Auth
{
    public class TokenAuthOption
    {
        /// <summary>
        /// Gets the audience.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        public static string Audience { get; } = "VoteInFront";
        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public static string Issuer { get; } = "VoteInBack";
        /// <summary>
        /// The key
        /// </summary>
        public static RsaSecurityKey Key = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        /// <summary>
        /// Gets the signing credentials.
        /// </summary>
        /// <value>
        /// The signing credentials.
        /// </value>
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        /// <summary>
        /// Gets the expires span.
        /// </summary>
        /// <value>
        /// The expires span.
        /// </value>
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(40);
        /// <summary>
        /// Converts to kentype.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public static string TokenType { get; } = "Bearer";
    }
}
