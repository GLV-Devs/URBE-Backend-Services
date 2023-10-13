using Microsoft.EntityFrameworkCore;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public class SocialContext : DbContext
{
    public SocialContext(DbContextOptions<SocialContext> options) : base(options)
    {

    }
}
