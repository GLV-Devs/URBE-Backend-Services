﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Internal;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Entities;

public class VehicleReport : ModifiableEntity, IEntity, IKeyed<Guid>, ISelfModelBuilder<VehicleReport>
{
    public static IEnumerable<string> KnownVehicleMakes { get; } = new List<string>()
    {
        "Ford",
        "Chevrolet",
        "Nissan",
        "Honda",
        "Toyota",
        "Volkswagen",
        "Subaru",
        "Tesla",
        "Mazda",
        "BMW",
        "Porsche",
        "Lexus",
        "Chrysler",
        "Buick",
        "Hyundai",
        "Audi",
        "Infiniti",
        "Dodge",
        "Genesis",
        "Mini",
        "Kia",
        "Volvo",
        "Mercedes-Benz",
        "Cadillac",
        "Acura",
        "GMC",
        "Jaguar",
        "Lincoln",
        "Jeep",
        "Mitsubishi",
        "Land Rover",
        "Alfa Romeo"
    };

    public const int VehicleDataMaxLength = 100;
    private readonly KeyedNavigation<Guid, VehicleUser> ownerNav = new();

    public Guid Id { get; }

    public string? VehicleModel { get; set; }

    public string? LicensePlate { get; set; }

    public uint? VehicleColor { get; set; }

    public string? VehicleCountryAlpha3Code { get; set; }

    public VehicleMaintenanceType MaintenanceType { get; set; }

    public VehicleUser? Owner
    {
        get => ownerNav.Entity;
        set => ownerNav.Entity = value;
    }

    public Guid OwnerId
    {
        get => ownerNav.Id;
        set => ownerNav.Id = value;
    }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<VehicleReport> mb)
    {
        mb.HasKey(x => x.Id);
        mb.Property(x => x.VehicleModel).HasMaxLength(VehicleDataMaxLength);
        mb.Property(x => x.LicensePlate).HasMaxLength(VehicleDataMaxLength);
        mb.Property(x => x.VehicleCountryAlpha3Code).HasMaxLength(3);
        mb.HasOne(x => x.Owner).WithMany(x => x.Reports).HasForeignKey(x => x.OwnerId);
    }
}
