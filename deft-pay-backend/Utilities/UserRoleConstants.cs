namespace deft_pay_backend.Utilities
{
    public class UserRoleConstants
    {
        public static readonly string ADMIN = "Admin";
        public static readonly string USER = "User";

        public static bool IsValidRole(string role)
        {
            return ADMIN == role || USER == role;
        }
    }
}
