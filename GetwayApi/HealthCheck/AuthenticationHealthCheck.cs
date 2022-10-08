using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GetwayApi.HealthCheck
{
    public class AuthenticationHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
