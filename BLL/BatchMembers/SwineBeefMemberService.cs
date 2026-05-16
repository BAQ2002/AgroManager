using MODEL;

namespace BLL.BatchMembers;

public interface ISwineBeefMemberService : IBatchMemberService<SwineBeefMember>
{
}

public sealed class SwineBeefMemberService : BatchMemberService<SwineBeefMember, ISwineBeefMemberRepository>, ISwineBeefMemberService
{
    public SwineBeefMemberService(ISwineBeefMemberRepository repository)
        : base(repository)
    {
    }
}