using System.Data;
using Catedra3.Data;
using Catedra3.Model;
using Catedra3.Token;
using Catedra3.Util;
using Microsoft.EntityFrameworkCore;

namespace Catedra3.Service;

public class JwtAuthenticationService : IAuthenticationService
{

    private DbSet<User> _users;
    private DbContextProvider _dbContextProvider;
    private IEncryptStrategy _encryptStrategy;
    private IUserTokenProvider _userTokenProvider;

    public JwtAuthenticationService(DbContextProvider dbContextProvider,
            IEncryptStrategy encryptStrategy,
            IUserTokenProvider userTokenProvider
        )
    {
        _users = dbContextProvider.Users;
        _encryptStrategy = encryptStrategy;
        _userTokenProvider = userTokenProvider;
        _dbContextProvider = dbContextProvider;
    }
    
    public User Register(CreationUser creationUser)
    {
        
        var userSearched = FindUserByEmail(creationUser.Email);
        if (userSearched != null)
        {
            throw new Exception("This email already exists");
        }

        var passwordHash = _encryptStrategy.Encrypt(creationUser.Password);
        var userCreated = new User
        {
            Email = creationUser.Email,
            PasswordEncrypt = passwordHash
        };
        
        _users.Add(
            userCreated  
        );

        _dbContextProvider.SaveChanges();
        
        return userCreated;
    }

    public Model.Token Auth(Authentication authentication)
    {

        var user = FindUserByEmail(authentication.Email);
        if (user == null)
        {
            throw new Exception("This user not exists");
        }

        var response = 
            _encryptStrategy.Verify(authentication.Password, user.PasswordEncrypt);

        if (!response)
        {
            throw new Exception("The password is incorrect");
        }
        
        
        var token = _userTokenProvider.Token(user);
        Console.WriteLine("Token = " + token);
        return new Model.Token
        {
            tokenContent = token,
            userId = user.id
        };
    }
    
    private User? FindUserByEmail(string email){
        return _users.
            SingleOrDefault(user => user.Email == email);
    }
    
}