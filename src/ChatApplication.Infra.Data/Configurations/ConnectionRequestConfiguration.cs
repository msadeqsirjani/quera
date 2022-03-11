using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class ConnectionRequestConfiguration : EntityConfiguration<ConnectionRequest>
{
    public override void Configure(EntityTypeBuilder<ConnectionRequest> builder)
    {
        base.Configure(builder);

        builder.ToTable("ConnectionRequests", Constant.Schema);

        builder.Property(x => x.Id);

        builder.Property(x => x.SourceGroupId);

        builder.Property(x => x.TargetGroupId);

        builder.Property(x => x.Accepted);

        builder.Property(x => x.Date);

        builder.HasOne(x => x.SourceGroup)
            .WithMany(x => x.SourceConnectionRequests)
            .HasForeignKey(x => x.SourceGroupId);
    }
}