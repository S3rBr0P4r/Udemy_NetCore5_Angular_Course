namespace Udemy.NetCore5.Angular.Data.Entities
{
    public class AppUserLike
    {
        public AppUser SourceUser { get; set; }

        public int SourceUserId { get; set; }

        public AppUser LikedUser { get; set; }

        public int LikedUserId { get; set; }
    }
}