using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Models;

namespace OfferManagementApi.Data
{

    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {

            }

            // Add DbSet properties for your entities here
        }
    
}
