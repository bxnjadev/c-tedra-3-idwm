using Catedra3.Model;
using Catedra3.Service;
using Microsoft.AspNetCore.Mvc;

namespace Catedra3.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{

    private IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("/api/auth")]
    public ActionResult<string> Auth(
        [FromBody] Authentication authentication
    )
    {
        var token = "";
        try
        {
            token =
                _authenticationService.Auth(authentication);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }

        return Ok(token);
    }
    
    [HttpPost]
    [Route("/api/register")]
    public ActionResult<User> Register(
        [FromBody] CreationUser creationUser
    )
    {
        User user = null;
        try
        {
            user =
                _authenticationService.Register(creationUser);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(user);
    }

}