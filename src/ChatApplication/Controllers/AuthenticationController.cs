    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace ChatApplication.Controllers; 

    [Route("api/v1/auth")]
    [AllowAnonymous]
    public class AuthenticationController : HomeController
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignIn()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }