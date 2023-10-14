using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.Services.Attributes;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

[RegisterService(typeof(IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>))]
[RegisterService(typeof(IUserRepository))]
public class UserRepository : IUserRepository
{
    public ValueTask<ErrorList> Update(SocialAppUser entity, UserUpdateModel update)
    {
        throw new NotImplementedException();
    }

    public ValueTask<SuccessResult<SocialAppUser>> Create(UserCreationModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask<object> GetView(SocialAppUser entity)
    {
        throw new NotImplementedException();
    }
}
