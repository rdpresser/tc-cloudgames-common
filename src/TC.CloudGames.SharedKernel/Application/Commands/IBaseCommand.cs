namespace TC.CloudGames.SharedKernel.Application.Commands
{
    public interface IBaseCommand : ICommand<Result>
    {
    }

    public interface IBaseCommand<TResponse> : ICommand<Result<TResponse>>
    {
    }
}
