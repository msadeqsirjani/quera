using ChatApplication.Application.Constants;
using ChatApplication.Application.Services.Common;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Application.ViewModels.Chat;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Application.Services;

public interface IChatService : IServiceAsync<Chat>
{
    Task<Result> ShowChatRoomsAsync(int memberId, CancellationToken cancellationToken = new());
    Task<Result> ShowChatsAsync(int memberId, int other, CancellationToken cancellationToken = new());

    Task<Result> SendMessageAsync(int memberId, int other, SendMessageDto parameter,
        CancellationToken cancellationToken = new());
}

public class ChatService : ServiceAsync<Chat>, IChatService
{
    private readonly IUnitOfWorkAsync _unitOfWork;
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IConnectionRequestRepository _connectionRequestRepository;

    public ChatService(IRepositoryAsync<Chat> repository, IUnitOfWorkAsync unitOfWork, IChatRoomRepository chatRoomRepository, IGroupMemberRepository groupMemberRepository, IConnectionRequestRepository connectionRequestRepository) : base(repository)
    {
        _unitOfWork = unitOfWork;
        _chatRoomRepository = chatRoomRepository;
        _groupMemberRepository = groupMemberRepository;
        _connectionRequestRepository = connectionRequestRepository;
    }

    public async Task<Result> ShowChatRoomsAsync(int memberId, CancellationToken cancellationToken = new())
    {
        var chatRooms = await _chatRoomRepository.Queryable(false)
            .OrderByDescending(x => x.Date)
            .Where(x => x.SourceMemberId == memberId || x.TargetMemberId == memberId)
            .Include(x => x.SourceMember)
            .Include(x => x.TargetMember)
            .ToListAsync(cancellationToken);

        var values = chatRooms.Select(x => new
        {
            UserId = x.SourceMemberId == memberId ? x.TargetMemberId : x.SourceMemberId,
            Name = x.SourceMemberId == memberId ? x.TargetMember.Name : x.SourceMember.Name
        });

        return values.Any() ? Result.WithSuccess(values) : Result.WithException(new FailError(Statement.Failure));
    }

    public async Task<Result> ShowChatsAsync(int memberId, int other, CancellationToken cancellationToken = new())
    {
        var chatRoom = await _chatRoomRepository.FirstOrDefaultAsync(x =>
            x.SourceMemberId == memberId && x.TargetMemberId == other ||
            x.SourceMemberId == other && x.TargetMemberId == memberId, cancellationToken);

        if (chatRoom == null)
            return Result.WithException(new FailError(Statement.Failure));

        var chats = await Repository.Queryable(false)
            .Where(x => x.ChatRoomId == chatRoom.Id)
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        var values = chats.Select(x => new
        {
            x.Message,
            x.Date.ToEpochTimeSpan().TotalSeconds,
            SendBy = x.SenderId
        });

        return values.Any() ? Result.WithSuccess(values) : Result.WithException(new FailError(Statement.Failure));
    }

    public async Task<Result> SendMessageAsync(int memberId, int other, SendMessageDto parameter,
        CancellationToken cancellationToken = new())
    {
        var sourceGroupInformation = await _groupMemberRepository.Queryable(false)
            .Where(x => x.MemberId == memberId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var targetGroupInformation = await _groupMemberRepository.Queryable(false)
            .Where(x => x.MemberId == other)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var hasConnectedGroup = _connectionRequestRepository.Queryable(false)
            .Any(x => (sourceGroupInformation.Contains(x.SourceGroupId) &&
                         targetGroupInformation.Contains(x.TargetGroupId)) ||
                        (sourceGroupInformation.Contains(x.TargetGroupId) &&
                         targetGroupInformation.Contains(x.SourceGroupId)));

        var now = DateTime.UtcNow;
        if (!sourceGroupInformation.Intersect(targetGroupInformation).Any() && !hasConnectedGroup)
            return Result.WithException(new FailError(Statement.Failure));
        
        var chatRoom = await _chatRoomRepository.Queryable(false)
            .FirstOrDefaultAsync(x => (x.SourceMemberId == memberId && x.TargetMemberId == other) ||
                                      (x.TargetMemberId == memberId && x.SourceMemberId == other), cancellationToken);

        if (chatRoom == null)
        {
            chatRoom = new ChatRoom()
            {
                SourceMemberId = memberId,
                TargetMemberId = other,
                Date = now
            };

            await _chatRoomRepository.AddAsync(chatRoom, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            chatRoom.Date = now;
            _chatRoomRepository.Attach(chatRoom);
        }

        var chat = new Chat()
        {
            ChatRoomId = chatRoom.Id,
            SenderId = memberId,
            Date = now,
            Message = parameter.Message
        };

        await Repository.AddAsync(chat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.WithSuccess(new { Message = "Successful" });

    }
}