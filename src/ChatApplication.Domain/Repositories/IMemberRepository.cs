using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Domain.Repositories;

public interface IMemberRepository : IRepositoryAsync<Member>
{

}