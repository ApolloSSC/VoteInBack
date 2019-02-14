using System;

namespace VoteIn.Model.ViewModels
{
    public class AuthToken
    {
        /// <summary>
        /// The request at
        /// </summary>
        public DateTime requestAt;
        /// <summary>
        /// The expires in
        /// </summary>
        public double expiresIn;
        /// <summary>
        /// The token type
        /// </summary>
        public string tokenType;
        /// <summary>
        /// The access token
        /// </summary>
        public string accessToken;
    }
}
