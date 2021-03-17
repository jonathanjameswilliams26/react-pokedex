using Backend.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Backend.Features.LoadAllPokemon
{
    public record PokeApiPokemonDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public PokeApiSpecies species { get; set; }
        public List<PokeApiStatWrapperDTO> stats { get; set; } = new List<PokeApiStatWrapperDTO>();
        public List<PokeApiTypeWrapperDTO> types { get; set; } = new List<PokeApiTypeWrapperDTO>();
        public PokeApiSprites sprites { get; set; }

        private string Artwork => sprites.other.artwork.front_default;
        private int GetStat(string name) => stats.Find(x => x.stat.name == name).base_stat;

        public Pokemon ToPokemon() => new Pokemon
        {
            Id = id,
            Name = name,
            Weight = weight,
            Height = height,
            Artwork = Artwork,
            Attack = GetStat("attack"),
            Defence = GetStat("defense"), // American spelling
            HP = GetStat("hp"),
            Speed = GetStat("speed"),
            SpecialAttack = GetStat("special-attack"),
            SpecialDefence = GetStat("special-defense") // American spelling
        };

        public IEnumerable<Type> GetTypes() => types.Select(x => x.ToType());
    }

    public record PokeApiSpecies
    {
        public string name { get; set; }
        public PokeApiGeneration generation { get; set; }
    }

    public record PokeApiStatWrapperDTO
    {
        public int base_stat { get; set; }
        public PokeApiStat stat { get; set; }
    }

    public record PokeApiStat
    {
        public string name { get; set; }
    }

    public record PokeApiTypeWrapperDTO
    {
        public PokeApiType type { get; set; }

        public Type ToType() => new Type { Name = type.name };
    }

    public record PokeApiType
    {
        public string name { get; set; }
    }

    public record PokeApiSprites
    {
        public PokeApiSpriteOther other { get; set; }
    }

    public record PokeApiSpriteOther
    {
        [JsonPropertyName("official-artwork")]
        public PokeApiSpriteOtherOfficalArtwork artwork { get; set; }
    }

    public record PokeApiSpriteOtherOfficalArtwork
    {
        public string front_default { get; set; }
    }

    public record PokeApiGeneration
    {
        public string url { get; set; }
    }
}
