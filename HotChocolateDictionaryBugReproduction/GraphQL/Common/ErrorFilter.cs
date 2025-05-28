namespace HotChocolateDictionaryBugReproduction.GraphQL.Common;

/// <summary>
/// ErrorFilter that translates known exceptions like NotFoundException to
/// GraphQL error codes.
/// </summary>
public class ErrorFilter(ILogger<ErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is null) return error;

        // var vnExceptionType = error.Exception.GetType();
        // if (ErrorCodes.Codes.TryGetValue(vnExceptionType, out var code))
        // {
        //     var result = ErrorBuilder.FromError(error)
        //         .SetCode(code)
        //         .SetMessage(error.Exception.Message)
        //         .Build();
        //
        //     return result;
        // }

        return error;
    }
}