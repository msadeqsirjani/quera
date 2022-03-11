using System.Reflection;
using ChatApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }    
    public DbSet<JoinRequest> JoinRequests { get; set; }
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var relationship in modelBuilder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}