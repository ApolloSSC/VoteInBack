using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using VoteIn.Auth;
using VoteIn.BL.Interfaces.Services;
using VoteIn.Model.Models;
using VoteIn.Model.ViewModels;
using VoteIn.Utils;

namespace VoteIn.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        /// <summary>
        /// The user manager
        /// </summary>
        protected UserManager<User> UserManager;
        /// <summary>
        /// The email sender service
        /// </summary>
        private IEmailSenderService EmailSenderService;
        /// <summary>
        /// The file service
        /// </summary>
        private IFileService FileService;
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration _configuration;
        /// <summary>
        /// The localizer
        /// </summary>
        private IStringLocalizer<AuthController> _localizer;

        #region Ctros.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="_userManager">The user manager.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="emailSenderService">The email sender service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="localizer">The localizer.</param>
        public AuthController(UserManager<User> _userManager, IConfiguration config, IEmailSenderService emailSenderService, IFileService fileService, IStringLocalizer<AuthController> localizer)
        {
            UserManager = _userManager;
            EmailSenderService = emailSenderService;
            FileService = fileService;
            _configuration = config;
            _localizer = localizer;
        }
        #endregion

        #region Public Methods
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName, Email = model.Email };
                var result = UserManager.CreateAsync(user, model.Password);
                result.Wait();
                if (!result.Result.Succeeded)
                {
                    return BadRequest(result.Result.Errors);
                }
                return Ok();
            }
            else
            {
                return BadRequest(Constantes.MSG_INVALID_DATA);
            }
        }

        [HttpPost("getToken")]
        public IActionResult GetToken([FromBody]AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
                var res = UserManager.CheckPasswordAsync(user, model.Password);
                res.Wait();
                if (!res.Result)
                {
                    return Unauthorized();
                }
                else
                {
                    var requestAt = DateTime.Now;
                    var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                    var token = GenerateToken(user, expiresIn);
                    var result = new AuthToken
                    {
                        requestAt = requestAt,
                        expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                        tokenType = TokenAuthOption.TokenType,
                        accessToken = token
                    };
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest(Constantes.MSG_INVALID_DATA);
            }
        }

        [HttpPost("generateNewPasswordLink")]
        public async Task<IActionResult> GenerateNewPasswordLink([FromBody] ResetPasswordViewModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(user);
                code = HttpUtility.UrlEncode(code).Replace("%","-");

                var culture = Thread.CurrentThread.CurrentUICulture;
                var mailContent = FileService.LoadFile(@"Mail\ResetPasswordMail.html", culture);

                if (mailContent != null)
                {
                    var url = _configuration["AppPath"] + "/password/" +code;
#if DEBUG
                    url = _configuration["DebugAppPath"] + "/password/" + code;
#endif
                    var filledContent = mailContent.Replace("{0}", url).Replace("{1}", user.UserName);
                    EmailSenderService.SendEmailAsync(model.Email, _localizer["FORGOTTEN_PASSWORD"], filledContent).Wait();
                }
            }
            return Ok();
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
            }
            else
            {
                var result = await UserManager.ResetPasswordAsync(user, 
                    HttpUtility.UrlDecode(model.Code.Replace("-","%")).Replace(" ", "+"), 
                    model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
        #endregion

        #region Private Methods
        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();
            
            var claims = new[] {
                new Claim("Id", user.Id),
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: TokenAuthOption.Issuer,
                audience: TokenAuthOption.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return handler.WriteToken(token);
        }
        #endregion
    }
}