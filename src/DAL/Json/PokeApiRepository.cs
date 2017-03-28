using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DAL.Json.Entities;
using DAL.Json.Interfaces;
using Newtonsoft.Json;

namespace DAL.Json
{
    public class PokeApiRepository : IPokeApiRepository
    {
        private readonly HttpClient _client;

        public PokeApiRepository()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v1/") };
        }

        public async Task<IList<PokemonJson>> GetPokemons(int start, int end)
        {
            var ids = Enumerable.Range(start, end);
            return await ExecuteMulitpleGets<PokemonJson>("pokemon", ids);
        }

        private async Task<List<T>> ExecuteMulitpleGets<T>(string url, IEnumerable<int> ids)
        {
            var list = new List<T>();

            var restCalls = Task.Run(() =>
            {
                Parallel.ForEach(ids, id =>
                {
                    var response = _client.GetAsync($"{url}/{id}").GetAwaiter().GetResult();
                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var deserializedObject = JsonConvert.DeserializeObject<T>(json);
                    list.Add(deserializedObject);
                });
            });

            await restCalls;
            return list;
        }
    }
}
