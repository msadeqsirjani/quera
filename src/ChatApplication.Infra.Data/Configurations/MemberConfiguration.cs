using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class MemberConfiguration : EntityConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.ToTable("Members", Constant.Schema);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Password)
            .IsRequired();

        builder.HasOne(x => x.Group)
            .WithOne(x => x.Member)
            .HasForeignKey<Group>(x => x.Administrator);

        builder.HasMany(x => x.GroupMembers)
            .WithOne(x => x.Member)
            .HasForeignKey(x => x.MemberId);

        builder.HasMany(x => x.JoinRequests)
            .WithOne(x => x.Member)
            .HasForeignKey(x => x.MemberId);

        builder.HasMany(x => x.SourceChats)
            .WithOne(x => x.SourceMember)
            .HasForeignKey(x => x.SourceMemberId);

        builder.HasMany(x => x.TargetChats)
            .WithOne(x => x.TargetMember)
            .HasForeignKey(x => x.TargetMemberId);
    }
}