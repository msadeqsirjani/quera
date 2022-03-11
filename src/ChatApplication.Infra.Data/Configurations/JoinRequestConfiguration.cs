using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class JoinRequestConfiguration : EntityConfiguration<JoinRequest>
{
    public override void Configure(EntityTypeBuilder<JoinRequest> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.GroupId);

        builder.Property(x => x.MemberId);

        builder.Property(x => x.Date);

        builder.HasOne(x => x.Member)
            .WithMany(x => x.JoinRequests)
            .HasForeignKey(x => x.MemberId);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.JoinRequests)
            .HasForeignKey(x => x.GroupId);
    }
}