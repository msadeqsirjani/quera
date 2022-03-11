using ChatApplication.Application.Services.Common;
using ChatApplication.Application.ViewModels.Group;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Enums;
using ChatApplication.Domain.Repositories;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Application.Services;

public interface IGroupService : IServiceAsync<Group>
{
    Task<Result> ShowAllGroupsAsync(CancellationToken cancellationToken = new());
    Task<Result> ShowMyGroupsAsync(int memberId, CancellationToken cancellationToken = new());
    Task<Result> CreateGroupAsync(int memberId, CreateGroupDto parameter, CancellationToken cancellationToken = new());
}

public class GroupService : ServiceAsync<Group>, IGroupService
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IUnitOfWorkAsync _unitOfWork;

    public GroupService(IRepositoryAsync<Group> repository, IGroupMemberRepository groupMemberRepository, IUnitOfWorkAsync unitOfWork) : base(repository)
    {
        _groupMemberRepository = groupMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ShowAllGroupsAsync(CancellationToken cancellationToken = new())
    {
        var groups = await Repository.Queryable(false)
            .OrderByDescending(x => x.Date)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description
            })
            .ToListAsync(cancellationToken);

        var values = new
        {
            Groups = groups
        };

        return Result.WithSuccess(values);
    }

    public async Task<Result> ShowMyGroupsAsync(int memberId, CancellationToken cancellationToken = new())
    {
        var groups = await Repository.Queryable(false)
            .OrderByDescending(x => x.Date)
            .Include(x => x.GroupMembers)
            .ThenInclude(x => x.Member)
            .Where(x => x.GroupMembers.Any(y => y.MemberId == memberId))
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                Members = x.GroupMembers.Select(y => new
                {
                    y.Member.Id,
                    y.Member.Name,
                    y.Member.Email,
                    Rule = y.MemberType == MemberType.Administrator ? "Owner" : "Normal"
                })
            })
            .ToListAsync(cancellationToken);

        var values = new
        {
            Groups = groups
        };

        return Result.WithSuccess(values);
    }

    public async Task<Result> CreateGroupAsync(int memberId, CreateGroupDto parameter, CancellationToken cancellationToken = new())
    {
        var group = new Group
        {
            Administrator = memberId,
            Name = parameter.Name,
            Description = parameter.Description,
            Date = DateTime.UtcNow
        };

        await Repository.AddAsync(group, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var groupMember = new GroupMember
        {
            Id = group.Id,
            MemberId = memberId,
            MemberType = MemberType.Administrator
        };

        await _groupMemberRepository.AddAsync(groupMember, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var value = new
        {
            Group = new
            {
                group.Id
            },
            Message = "Successful"
        };

        return Result.WithSuccess(value);
    }
}