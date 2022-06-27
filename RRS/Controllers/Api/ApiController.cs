using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RRS.Controllers
{
    [Route("api/v1/test")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("roles")]
        public HashSet<string> Roles()
        {
            HashSet<string> roles = new();

            foreach (string role in User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))
            {
                roles.Add(role);
            }

            return roles;
        }

        [HttpGet("authorize")]
        [Authorize]
        public IActionResult Authorize()
        {
            return Ok();
        }

        [HttpGet("authorize-customer")]
        [Authorize(Roles = "Member")]
        public IActionResult AuthorizeUser()
        {
            return Ok();
        }
    }
}
