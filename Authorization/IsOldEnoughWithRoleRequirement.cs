using Microsoft.AspNetCore.Authorization;


namespace TrendingNews.Authorization
{
    public class IsOldEnoughWithRoleRequirement : IAuthorizationRequirement
    {
        public IsOldEnoughWithRoleRequirement(int minAge)
        {
            MinAge = minAge;
        }

        public int MinAge { get; set; }
    }
}
