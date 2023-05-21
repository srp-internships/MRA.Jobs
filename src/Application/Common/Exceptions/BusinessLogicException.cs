using System.Runtime.Serialization;

namespace MRA.Jobs.Application.Common.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException() { }

    public BusinessLogicException(string message) : base(message) { }

    public BusinessLogicException(string message, Exception inner) : base(message, inner) { }

    protected BusinessLogicException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
