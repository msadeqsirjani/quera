    using ChatApplication.Application.Services;
    using ChatApplication.Application.ViewModels.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    namespace ChatApplication.Controllers; 

    [Route("api/v1/auth")]
    [AllowAnonymous]
    public class AuthenticationController : HomeController
    {
        private readonly IMemberService _memberService;

        public AuthenticationController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// Sign up for new user
        /// </summary>
        /// <param name="parameter"></param>
        /// <response code="200"><h2>important</h2> JWT must contains userId and email in payload</response>
        /// <response code="400">Password is not valid, Email is already exist</response>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthenticationSuccess))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
        [HttpPost("signup")]
        public async Task<IActionResult> SignIn(SignInDto parameter)
        {
            var result = await _memberService.SignInAsync(parameter);

            return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="parameter"></param>
        /// <response code="200"><h2>important</h2> JWT must contains userId and email in payload</response>
        /// <response code="400">Authentication failed!</response>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthenticationSuccess))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto parameter)
        {
            var result = await _memberService.LoginAsync(parameter);

            return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }
    }