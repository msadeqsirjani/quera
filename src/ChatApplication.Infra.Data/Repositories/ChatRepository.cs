using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class ChatRepository : RepositoryAsync<Chat>, IChatRepository
{
    public ChatRepository(ApplicationDbContext context) : base(context)
    {
    }
}