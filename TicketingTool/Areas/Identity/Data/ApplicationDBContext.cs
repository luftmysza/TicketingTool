using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;
using TicketingTool.Areas.Identity.Data;
using TicketingTool.Models;

namespace TicketingTool.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<TicketingTool.Models.Ticket> Ticket { get; set; } = default!;
    public DbSet<TicketingTool.Models.Project> Project { get; set; } = default!;

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);


        // Seed ApplicationUser first
        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "1",
                UserName = "X001",
                NormalizedUserName = "X001",
                AccessFailedCount = 0,
                LockoutEnabled = false
            }
        );

        SeedData(builder);
        ConfigureEntities(builder);
    }
    private void ConfigureEntities(ModelBuilder builder)
    {
        //ApplicationUser
        builder.Entity<ApplicationUser>()
            .HasMany(au => au.CreatedTickets)
            .WithOne(t => t.CreatorRef)
            .HasForeignKey(t => t.CreatorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(au => au.AssignedTickets)
            .WithOne(t => t.AssigneeRef)
            .HasForeignKey(t => t.AssigneeID)
            .OnDelete(DeleteBehavior.Restrict);

        //Ticket Change
        builder.Entity<TicketChange>()
            .HasOne(tc => tc.TicketRef)
            .WithMany(t => t.Changes)
            .HasForeignKey(tc => tc.TicketID)
            .OnDelete(DeleteBehavior.Restrict);

        //Ticket
        builder.Entity<Ticket>()
            .HasIndex(t => t.IssueKey)
            .IsUnique();
        
        builder.Entity<Ticket>()
            .HasOne(t => t.ProjectRef)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ticket>()
            .HasOne(t => t.CreatorRef)
            .WithMany(au => au.CreatedTickets)
            .HasForeignKey(t => t.CreatorID)
            .HasPrincipalKey(au => au.UserName)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ticket>()
            .HasOne(t => t.AssigneeRef)
            .WithMany(au => au.AssignedTickets)
            .HasForeignKey(t => t.AssigneeID)
            .HasPrincipalKey(au => au.UserName)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ticket>()
            .HasMany(t => t.Changes)
            .WithOne(au => au.TicketRef)
            .OnDelete(DeleteBehavior.Restrict);

        //Component2Project
        builder.Entity<Component2Project>().
            HasKey(t => new { t.ComponentID, t.ProjectID });
    }
    private void SeedData(ModelBuilder builder)
    {
        builder.Entity<Ticket>().HasData(
                new Ticket { ID = 1, IssueKey = "BSC-1", ProjectID = 1, ComponentID = 1, Title = "Seed Ticket 1", Description = "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", StatusID = 1, CreatorID = "X001", CreatedDate = DateTime.Now, LastUpdatedDate = DateTime.Now },
                new Ticket { ID = 2, IssueKey = "BSC-2", ProjectID = 1, ComponentID = 1, Title = "Seed Ticket 2", Description = "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", StatusID = 1, CreatorID = "X001", CreatedDate = DateTime.Now, LastUpdatedDate = DateTime.Now },
                new Ticket { ID = 3, IssueKey = "BSC-3", ProjectID = 1, ComponentID = 1, Title = "Seed Ticket 3", Description = "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", StatusID = 1, CreatorID = "X001", CreatedDate = DateTime.Now, LastUpdatedDate = DateTime.Now },
                new Ticket { ID = 4, IssueKey = "BSC-4", ProjectID = 1, ComponentID = 1, Title = "Seed Ticket 4", Description = "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", StatusID = 1, CreatorID = "X001", CreatedDate = DateTime.Now, LastUpdatedDate = DateTime.Now },
                new Ticket { ID = 5, IssueKey = "BSC-5", ProjectID = 1, ComponentID = 1, Title = "Seed Ticket 5", Description = "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", StatusID = 1, CreatorID = "X001", CreatedDate = DateTime.Now, LastUpdatedDate = DateTime.Now }
            );

        builder.Entity<Project>().HasData(
                new Project { ID = 1, ProjectKey = "BSC", Counter = 5, ProjectName = "Basic Project" }
            );
        builder.Entity<Component>().HasData(
                new Component { ID = 1, ComponentName = "Unidentified" },
                new Component { ID = 2, ComponentName = "User Interface Module" },
                new Component { ID = 3, ComponentName = "Database Management" },
                new Component { ID = 4, ComponentName = "API Gateway" },
                new Component { ID = 5, ComponentName = "Logging Service" },
                new Component { ID = 6, ComponentName = "Notification System" },
                new Component { ID = 7, ComponentName = "Payment Processor" },
                new Component { ID = 8, ComponentName = "Analytics Engine" },
                new Component { ID = 9, ComponentName = "Reporting Tool" },
                new Component { ID = 10, ComponentName = "Cache Management" },
                new Component { ID = 11, ComponentName = "Authentication Service" }
            );
        builder.Entity<Component2Project>().HasData(
                new Component2Project { ComponentID = 1, ProjectID = 1 },
                new Component2Project { ComponentID = 2, ProjectID = 1 },
                new Component2Project { ComponentID = 3, ProjectID = 1 },
                new Component2Project { ComponentID = 4, ProjectID = 1 },
                new Component2Project { ComponentID = 5, ProjectID = 1 },
                new Component2Project { ComponentID = 6, ProjectID = 1 },
                new Component2Project { ComponentID = 7, ProjectID = 1 },
                new Component2Project { ComponentID = 8, ProjectID = 1 },
                new Component2Project { ComponentID = 9, ProjectID = 1 },
                new Component2Project { ComponentID = 10, ProjectID = 1 },
                new Component2Project { ComponentID = 11, ProjectID = 1 }
    );
        builder.Entity<Status>().HasData(
                new Status { ID = 1, StatusName = "Open" },
                new Status { ID = 2, StatusName = "In Progress" },
                new Status { ID = 3, StatusName = "Resolved"},
                new Status { ID = 4, StatusName = "Closed" }
            );

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}