﻿using Catedra3.Model;

namespace Catedra3.Service;

public interface IAuthenticationService
{

    User Register(CreationUser creationUser);

    Model.Token Auth(Authentication authentication);

}