using System;
using System.Collections.Generic;
using DAL_Database.Dapper.Enums;
using DAL_Database.DTO.Abstract;

namespace DAL_Database.DTO
{
    public class Anime : BaseAnime
    {
        public int? EpisodeCount { get; set; }
        public int? EpisodeLength { get; set; }

        public AnimeTypeEnum ShowType { get; set; }
        public AnimeAgeRatingEnum AgeRating { get; set; }
        public string AgeRatingDescription { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Synopsis { get; set; }
        public string YoutubeId { get; set; }

        public int UserCount { get; set; }
        public double AvarageRating { get; set; }

        public string JapaneseTitle { get; set; }

        public ICollection<string> Genre { get; set; }
    }
}
