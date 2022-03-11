using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class GroupConfiguration : EntityConfiguration<Group>
{
    public override void Configure(EntityTypeBuilder<Group> builder)
    {
        base.Configure(builder);

        builder.ToTable("Groups", Constant.Schema);

        builder.Property(x => x.Administrator);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Description);

        builder.HasMany(x => x.GroupMembers)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.Id);

        builder.HasOne(x => x.Member)
            .WithOne(x => x.Group)
            .HasForeignKey<Group>(x => x.Administrator);

        builder.HasMany(x => x.SourceConnectionRequests)
            .WithOne(x => x.SourceGroup)
            .HasForeignKey(x => x.SourceGroupId);
    }
}