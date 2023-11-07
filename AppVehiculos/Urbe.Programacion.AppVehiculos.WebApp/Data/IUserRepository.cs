using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleUser;
using Urbe.Programacion.Shared.ModelServices;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data;

public interface IUserRepository : IEntityCRUDRepository<VehicleUser, Guid, VehicleUserCreationModel, VehicleUserUpdateModel>
{
}
