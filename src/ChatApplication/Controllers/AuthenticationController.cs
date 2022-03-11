    using ChatApplication.Application.Services;
    using ChatApplication.Application.ViewModels.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("signup")]
        public async Task<IActionResult> SignIn(SignInDto parameter)
        {
            var result = await _memberService.SignInAsync(parameter);

            return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto parameter)
        {
            var result = await _memberService.LoginAsync(parameter);

            return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
        }
    }