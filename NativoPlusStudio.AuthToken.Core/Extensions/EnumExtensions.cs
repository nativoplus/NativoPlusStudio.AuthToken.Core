using NativoPlusStudio.AuthToken.Core.Enums;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Collections.Generic;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TimeSpan> GenerateBackoffDelay(this BackoffAlgorithmTypeEnums enums, int initialDelayInSeconds, int retryCount)
        {
            return enums switch
            {
                BackoffAlgorithmTypeEnums.Constant => Backoff.ConstantBackoff(delay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackoffAlgorithmTypeEnums.Linear => Backoff.LinearBackoff(initialDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackoffAlgorithmTypeEnums.Exponential => Backoff.ExponentialBackoff(initialDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                BackoffAlgorithmTypeEnums.Jitter => Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(initialDelayInSeconds), retryCount: retryCount),
                _ => new TimeSpan[] { TimeSpan.FromSeconds(1) },
            };
        }
    }
}
