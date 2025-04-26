using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB.Models.Entities;

namespace WEB.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : IdentityDbContext<AppUser>(options)
    {
    }
}
