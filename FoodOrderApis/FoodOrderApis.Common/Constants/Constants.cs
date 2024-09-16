namespace FoodOrderApis.Common.Constants;

public static class Constants
{
    public static class Role
    {
        public const string Admin = "ADMIN";
        public const string Eater = "EATER";
        public const string Merchant = "MERCHANT";
    }

    public static class CustomClaimTypes
    {
        public const string UserId = "user_id";
        public const string Role = "role";
        public const string UserName = "user_name";
        public const string ClientId = "user_client_id";
    }

    public static class ImageDefault
    {
        public const string AvatarDefault =
            "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg";
    }
}
