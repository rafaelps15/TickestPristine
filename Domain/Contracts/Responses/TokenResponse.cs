using Tickest.Domain.Common;

namespace Tickest.Domain.Contracts.Responses
{
    public record TokenResponse : IResponse
    {
        public TokenResponse(Result<string> tokenResult)
        {
            TokenResult = tokenResult;
            Token = tokenResult.IsSuccess ? tokenResult.Data : null;
        }

        public TokenResponse(string token)
        {
            Token = token;
        }

        public string Token { get; }
        public Result<string> TokenResult { get; }
    }
}
