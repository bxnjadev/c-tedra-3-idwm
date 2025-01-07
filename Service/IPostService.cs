using Catedra3.Model;

namespace Catedra3.Service;

public interface IPostService
{

    Task<Post> Publish(CreationPost creationPost,
        IFormFile formFile);

    List<Post> All();

}