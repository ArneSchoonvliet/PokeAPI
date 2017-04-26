using DAL_Database.DTO;
using MediatR;

namespace DAL_Database.Dapper.Queries
{
    public class AnimeQuery : IRequest<Anime>
    {
        public AnimeQuery(int animeId)
        {
            Id = Id;
        }

        public int Id { get; }
    }
}
