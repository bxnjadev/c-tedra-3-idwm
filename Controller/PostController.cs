using Catedra3.Model;
using Catedra3.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catedra3.Controller;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase {

    private IPostService postService;
    public PostController(IPostService postService){
        this.postService = postService;
    }

    [HttpPost]
    [Route("/api/post")]
    [Authorize]
    public async Task<ActionResult<Post>> Post([FromForm] CreationPost creationPost,
    [FromForm] IFormFile formFile)
    {

        var post = await postService.
              Publish(creationPost, formFile);
        return post;
    }

    [HttpGet]
    [Route("/api/post")]
    [Authorize]
    public ActionResult<List<Post>> All() {
        return postService.All();        
    }

}
