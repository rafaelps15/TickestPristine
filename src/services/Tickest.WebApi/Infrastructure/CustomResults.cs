using Tickest.SharedKernel;

namespace WebApi.Infrastructure;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        var problemDetails = ApiProblemDetailsFactory.Create(result);

        return Results.Problem(problemDetails);
    }
}
