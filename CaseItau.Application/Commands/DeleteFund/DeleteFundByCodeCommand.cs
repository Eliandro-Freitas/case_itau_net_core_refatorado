using MediatR;

namespace CaseItau.Application.Commands.DeleteFund;

public class DeleteFundByCodeCommand : IRequest
{
    public string Code { get; set; }
}
