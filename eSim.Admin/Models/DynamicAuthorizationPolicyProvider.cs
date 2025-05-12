using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace eSim.Admin.Models
{
    public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Parse the policy name (e.g., "Customers:view")
            var parts = policyName.Split(':');
            if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && !string.IsNullOrWhiteSpace(parts[1]) )
            {
                var claimType = parts[0];
                var claimValue = parts[1];
                var policy = new AuthorizationPolicyBuilder()
                    .RequireClaim(claimType, claimValue)
                    .Build();
                return Task.FromResult(policy);
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}
