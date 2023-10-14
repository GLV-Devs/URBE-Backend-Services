using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IPostRepository : IEntityCRDRepository<Post, Snowflake, PostCreationModel> { }

#error Create IPostRepository
#error Create IMailVerificationService
#error Create ISessionServices