using NativoPlusStudio.AuthToken.Core.Enums;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Collections.Generic;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TimeSpan> GenerateBackOffTimeSpans(this BackOffAlgorithmTypeEnums enums, int initialDelayInSeconds, int retryCount)
        {
            return enums switch
            {
                BackOffAlgorithmTypeEnums.Constant => Backoff.ConstantBackoff(delay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackOffAlgorithmTypeEnums.Linear => Backoff.LinearBackoff(initialDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackOffAlgorithmTypeEnums.Exponential => Backoff.ExponentialBackoff(initialDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackOffAlgorithmTypeEnums.Jitter => Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                _ => new TimeSpan[] { TimeSpan.FromSeconds(1) },
            };
        }
    }
}
