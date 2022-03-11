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

        builder.Property(x => x.ChatRoomId); 

        builder.Property(x => x.SenderId); 

        builder.Property(x => x.Message);

        builder.Property(x => x.Date);

        builder.HasOne(x => x.ChatRoom)
            .WithMany(x => x.Chats)
            .HasForeignKey(x => x.ChatRoomId);
    }
}