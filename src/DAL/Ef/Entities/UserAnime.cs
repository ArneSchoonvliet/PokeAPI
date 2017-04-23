using DAL_Database.Ef.Enums;

namespace DAL_Database.Ef.Entities
{
    public class UserAnime
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int AnimeId { get; set; }

        public int UserRating { get; set; }
        public int SeenEpisodes { get; set; }
        public AnimeUserStatusEnum UserStatus { get; set; }

    }
}
