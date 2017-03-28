using System;
using System.Collections.Generic;

namespace DAL.DbContext.Entities
{
    public class Pokemon
    {
        public Pokemon()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public int PokedexId { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }

        // Skills
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Hp { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
    }
}
