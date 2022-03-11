using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApplication.Domain.Entities;
using ChatApplication.Infra.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.Infra.Data.Configurations
{
    public class ChatRoomConfiguration : EntityConfiguration<ChatRoom>
    {
        public override void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            base.Configure(builder);

            builder.ToTable("ChatRooms", Constant.Schema);

            builder.Property(x => x.SourceMemberId);

            builder.Property(x => x.TargetMemberId);

            builder.HasMany(x => x.Chats)
                .WithOne(x => x.ChatRoom)
                .HasForeignKey(x => x.Id);
        }
    }
}
