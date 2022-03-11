using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class GroupMemberConfiguration : EntityConfiguration<GroupMember>
{
    public override void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        base.Configure(builder);

        builder.ToTable("GroupMembers", Constant.Schema);

        builder.HasKey(x => new { x.Id, x.MemberId });

        builder.Property(x => x.Id);

        builder.Property(x => x.MemberId);

        builder.Property(x => x.MemberType).HasConversion<string>();

        builder.HasOne(x => x.Member)
            .WithMany(x => x.GroupMembers)
            .HasForeignKey(x => x.MemberId);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.GroupMembers)
            .HasForeignKey(x => x.Id);
    }
}