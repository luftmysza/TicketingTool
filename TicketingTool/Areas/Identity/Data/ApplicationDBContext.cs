using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection.Emit;
using TicketingTool.Areas.Identity.Data;
using TicketingTool.Models;

namespace TicketingTool.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<TicketingTool.Models.Ticket> Ticket { get; set; } = default!;
    public DbSet<TicketingTool.Models.Project> Project { get; set; } = default!;
    public DbSet<TicketingTool.Models.TicketChange> TicketChange { get; set; } = default!;
    public DbSet<TicketingTool.Models.Comment> Comments { get; set; } = default!;
    public DbSet<TicketingTool.Models.Component> Components { get; set; } = default!;
    public DbSet<TicketingTool.Areas.Identity.Data.ProjectUserRole> ProjectUserRole { get; set; } = default!;


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



        SeedUserData(builder);
        SeedMiscData(builder);
        ConfigureEntities(builder);
    }
    private void ConfigureEntities(ModelBuilder builder)
    {
        //ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
            {
                entity
                    .HasMany(au => au.CreatedTickets)
                    .WithOne(t => t.CreatorRef)
                    .HasForeignKey(t => t.CreatorID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(au => au.AssignedTickets)
                    .WithOne(t => t.AssigneeRef)
                    .HasForeignKey(t => t.AssigneeID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasAlternateKey(au => au.UserName);
            }
        );

        builder.Entity<Comment>()
            .HasOne(c => c.TicketRef)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.IssueKey)
            .HasPrincipalKey(t => t.IssueKey)
            .OnDelete(DeleteBehavior.Cascade);

        //Project


        //Ticket Change
        builder.Entity<TicketChange>(entity =>
            {
                entity
                    .HasOne(tc => tc.TicketRef)
                    .WithMany(t => t.Changes)
                    .HasForeignKey(tc => tc.TicketID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(tc => tc.ChangedByRef)
                    .WithMany()
                    .HasForeignKey(tc => tc.ChangedBy)
                    .HasPrincipalKey(au => au.UserName);
            }
        );
           

        //Ticket
        builder.Entity<Ticket>(entity =>
            {
                entity
                    .HasIndex(t => t.IssueKey)
                    .IsUnique();

                entity
                    .HasOne(t => t.ProjectRef)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(t => t.ProjectID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.CreatorRef)
                    .WithMany(au => au.CreatedTickets)
                    .HasForeignKey(t => t.CreatorID)
                    .HasPrincipalKey(au => au.UserName)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.AssigneeRef)
                    .WithMany(au => au.AssignedTickets)
                    .HasForeignKey(t => t.AssigneeID)
                    .HasPrincipalKey(au => au.UserName)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(t => t.Changes)
                    .WithOne(au => au.TicketRef)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        );            

        //ProjectUserRole
        builder.Entity<ProjectUserRole>(entity =>
            {
                entity.HasKey(pur => new { pur.ProjectId, pur.UserId });

                entity
                    .HasOne(pur => pur.ProjectRef)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(pur => pur.ProjectId);

                entity
                    .HasOne(pur => pur.UserNameRef)
                    .WithMany(au => au.Projects)
                    .HasForeignKey(pur => pur.UserId)
                    .HasPrincipalKey(au => au.UserName);
            }   
        );


    }
    private void SeedUserData(ModelBuilder builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        // Seed ApplicationUser first
        var adminUser = new ApplicationUser
        {
            Id = 1,
            Name = "Admin",
            Surname = "User",
            UserName = "X001",
            NormalizedUserName = "X001",
            AccessFailedCount = 0,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "password");
        var ticketCreator = new ApplicationUser
        {
            Id = 2,
            Name = "Ticket",
            Surname = "Creator",
            UserName = "TECH01",
            NormalizedUserName = "TECH01",
            AccessFailedCount = 0,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        ticketCreator.PasswordHash = hasher.HashPassword(ticketCreator, "password");
        var jobUser = new ApplicationUser
        {
            Id = 3,
            Name = "Job",
            Surname = "User",
            UserName = "TECH02",
            NormalizedUserName = "TECH02",
            AccessFailedCount = 0,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        jobUser.PasswordHash = hasher.HashPassword(jobUser, "password");

        builder.Entity<ApplicationUser>().HasData(
            adminUser, ticketCreator, jobUser    
        );

        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 2, RoleId = 4 },
            new IdentityUserRole<int> { UserId = 3, RoleId = 4 }
        );


    }
    private void SeedMiscData(ModelBuilder builder)
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
                new Component { ID = 1, ComponentName = "Unidentified", ProjectID = 1},
                new Component { ID = 2, ComponentName = "User Interface Module" , ProjectID = 1},
                new Component { ID = 3, ComponentName = "Database Management" , ProjectID = 1},
                new Component { ID = 4, ComponentName = "API Gateway" , ProjectID = 1},
                new Component { ID = 5, ComponentName = "Logging Service" , ProjectID = 1},
                new Component { ID = 6, ComponentName = "Notification System" , ProjectID = 1},
                new Component { ID = 7, ComponentName = "Payment Processor" , ProjectID = 1},
                new Component { ID = 8, ComponentName = "Analytics Engine" , ProjectID = 1},
                new Component { ID = 9, ComponentName = "Reporting Tool" , ProjectID = 1},
                new Component { ID = 10, ComponentName = "Cache Management" , ProjectID = 1},
                new Component { ID = 11, ComponentName = "Authentication Service" , ProjectID = 1}
            );

        builder.Entity<Status>().HasData(
                new Status { ID = 1, StatusName = "Open" },
                new Status { ID = 2, StatusName = "In Progress" },
                new Status { ID = 3, StatusName = "Resolved"},
                new Status { ID = 4, StatusName = "Closed" }
            );

        builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole<int> { Id = 3, Name = "User", NormalizedName = "USER" },
                new IdentityRole<int> { Id = 4, Name = "Technical User", NormalizedName = "TECH" }
            );

        builder.Entity<ProjectUserRole>().HasData(
                new ProjectUserRole
                {
                    ProjectId = 1,
                    UserId = "X001",        
                    RoleId = "ADMIN"          
                },
                new ProjectUserRole
                {
                    ProjectId = 1,
                    UserId = "TECH01",       
                    RoleId = "TECH"           
                },
                new ProjectUserRole
                {
                    ProjectId = 1,
                    UserId = "TECH02",      
                    RoleId = "TECH"            
                }
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}