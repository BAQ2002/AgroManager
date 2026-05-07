using MODEL;

namespace BLL.BatchMembers;

public interface IBovinePastureMemberService : IBatchMemberService<BovinePastureMember>
{
}

public sealed class BovinePastureMemberService : BatchMemberService<BovinePastureMember, IBovinePastureMemberRepository>, IBovinePastureMemberService
{
    public BovinePastureMemberService(IBovinePastureMemberRepository repository)
        : base(repository)
    {
    }
}