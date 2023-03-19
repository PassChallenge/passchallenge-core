using System;
using System.Runtime.Serialization;

namespace PassChallenge.Core.Exceptions;

public class ChallengeException : Exception
{
    public ChallengeException()
    {
    }

    protected ChallengeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ChallengeException(string message) : base(message)
    {
    }

    public ChallengeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}