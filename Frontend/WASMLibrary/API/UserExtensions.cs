using System.Security.Claims;
using System.Text.Json;

namespace WASMLibrary.API
{
    public static class UserExtensions
    {
        public static bool HasRole(this ClaimsPrincipal principal, string role)
        {
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            string rolesArray = principal.Claims.FirstOrDefault(x => x.Type == "roles")?.Value ?? "[]";
            var jsonArray = JsonSerializer.Deserialize<List<string>>(rolesArray);
            return jsonArray?.Contains(role) ?? false;
        }

        public static string GetRole(this ClaimsPrincipal principal)
        {
            string rolesArray = principal.Claims.FirstOrDefault(x => x.Type == "roles")?.Value ?? "[]";
            string result = rolesArray.Trim('[', ']', '"');

            if(result.Contains("Admin"))
            {
                return result;
            } else
            {
                return "User";
            }
        }

    }
}
