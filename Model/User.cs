﻿namespace Catedra3.Model;

public class User
{

    public int id { get; set; } = 0;
    public string Email { get; set; } = string.Empty;

    public string PasswordEncrypt { get; set; } = string.Empty;

    public List<Post> Posts { get; set; } = null;

}

public class UserView
{
    
}

public class CreationUser
{

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

}

public class Authentication
{

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

}