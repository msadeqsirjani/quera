using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations;

public class ChatConfiguration : EntityConfiguration<Chat>
{
    public override void Configure(EntityTypeBuilder<Chat> builder)
    {
        base.Configure(builder);

        builder.ToTable("Chats", Constant.Schema);

        builder.Property(x => x.SourceMemberId);

        builder.Property(x => x.TargetMemberId);

        builder.Property(x => x.Message);

        builder.HasOne(x => x.SourceMember)
            .WithMany(x => x.SourceChats)
            .HasForeignKey(x => x.SourceMemberId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.TargetMember)
            .WithMany(x => x.TargetChats)
            .HasForeignKey(x => x.TargetMemberId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}