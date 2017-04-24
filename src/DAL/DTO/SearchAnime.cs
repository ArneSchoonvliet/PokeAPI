using System;
using DAL_Database.Dapper.Enums;
using DAL_Database.DTO.Abstract;

namespace DAL_Database.DTO
{
    public class SearchAnime : BaseAnime
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AnimeTypeEnum ShowType { get; set; }
        public double AverageRating { get; set; }

    }
}
