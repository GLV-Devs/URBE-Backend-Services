using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Internal;

namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

public class VehicleUserRoleAssignation : IEntity, IKeyed<Guid>
{
    private readonly KeyedNavigation<Guid, VehicleUserRole> rolenav = new();
    private readonly KeyedNavigation<Guid, VehicleUser> usernav = new();

    public Guid Id { get; init; }

    public Guid UserId
    {
        get => usernav.Id;
        set => usernav.Id = value;
    }

    public VehicleUser? User
    {
        get => usernav.Entity;
        set => usernav.Entity = value;
    }

    public Guid UserRoleId
    {
        get => rolenav.Id;
        set => rolenav.Id = value;
    }

    public VehicleUserRole? UserRole
    {
        get => rolenav.Entity;
        set => rolenav.Entity = value;
    }
}
