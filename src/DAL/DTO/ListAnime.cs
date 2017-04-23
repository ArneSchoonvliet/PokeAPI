using DAL_Database.Dapper.Enums;
using DAL_Database.DTO.Abstract;
using DAL_Database.Ef.Enums;

namespace DAL_Database.DTO
{
    public class ListAnime : BaseAnime
    {
        public string UserId { get; set; }
        public int UserRating { get; set; }
        public int SeenEpisodes { get; set; }
        public AnimeUserStatusEnum UserStatus { get; set; }

        public int? EpisodeCount { get; set; }
        public AnimeTypeEnum ShowType { get; set; }
    }
}
