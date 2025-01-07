using Catedra3.Data;
using Catedra3.Model;
using Microsoft.EntityFrameworkCore;

namespace Catedra3.Service;

public class PostService : IPostService
{

    private readonly CloudinaryImageService _cloudinaryImageService;
    private readonly DbSet<Post> _posts;

    public PostService(CloudinaryImageService cloudinaryImageService,
        DbContextProvider dbContextProvider)
    {
        _cloudinaryImageService = cloudinaryImageService;
        _posts = dbContextProvider.Posts;
    }
    
    public async Task<Post> Publish(CreationPost creationPost, IFormFile formFile)
    {
       var result = await _cloudinaryImageService.Upload(formFile);
       var url = result.SecureUrl.AbsoluteUri;

       var post = new Post
       {
            Title = creationPost.Title,
            DateCreated = creationPost.DateCreated,
            Url = url
       };

       _posts.Add(
           post
       );

       return post;
    }

    public List<Post> All()
    {
        return _posts.ToList();
    }
}