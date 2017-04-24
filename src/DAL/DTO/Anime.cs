using System.Collections.Generic;
using DAL_Database.Dapper.Enums;

namespace DAL_Database.DTO
{
    public class Anime : SearchAnime
    {
        public Anime()
        {
            Genres = new List<string>();
        }

        public int? EpisodeCount { get; set; }
        public int? EpisodeLength { get; set; }

        public AnimeAgeRatingEnum AgeRating { get; set; }
        public string AgeRatingDescription { get; set; }

        public string Synopsis { get; set; }
        public string YoutubeId { get; set; }

        public int UserCount { get; set; }

        public string JapaneseTitle { get; set; }

        public IList<string> Genres { get; set; }
    }
}
