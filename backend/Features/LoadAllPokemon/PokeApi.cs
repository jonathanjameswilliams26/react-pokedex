using Backend.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Features.LoadAllPokemon
{
    public class PokeApi
    {
        private readonly HttpClient client;
        private readonly ILogger<PokeApi> logger;
        private const int NUM_OF_POKEMON = 10;

        public PokeApi(HttpClient client, ILogger<PokeApi> logger)
        {
            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            this.client = client;
            this.logger = logger;
        }

        public async Task<IEnumerable<Pokemon>> GetAllPokemon()
        {
            var types = new Dictionary<string, Domain.Type>();
            var pokemon = new List<Pokemon>(NUM_OF_POKEMON);
            for(int i = 1; i <= NUM_OF_POKEMON; i++)
            {
                // Need to do this so we don't get rate limited
                // Would be faster to use a list of tasks then run Task.WhenAll
                pokemon.Add(await GetPokemon(i, types));
            }
            return pokemon;
        }

        async Task<Pokemon> GetPokemon(int id, Dictionary<string, Domain.Type> types)
        {
            logger.LogInformation("Getting Pokemon {ID}", id);
            var response = await client.GetAsync($"pokemon/{id}");
            var body = await response.Content.ReadAsStreamAsync();
            var pokemonDTO = await JsonSerializer.DeserializeAsync<PokeApiPokemonDTO>(body);
            var pokemon = pokemonDTO.ToPokemon();
            foreach(var type in pokemonDTO.GetTypes())
            {
                if (types.ContainsKey(type.Name) == false)
                    types.Add(type.Name, type);

                pokemon.Types.Add(types[type.Name]);
                types[type.Name].Pokemon.Add(pokemon);
            }
            pokemon.Generation = await GetGeneration(pokemonDTO.species.name);
            logger.LogInformation("Succesfully got Pokemon {ID}: {Name}", pokemon.Id, pokemon.Name);
            return pokemon;
        }

        async Task<int> GetGeneration(string speciesName)
        {
            var respone = await client.GetAsync($"pokemon-species/{speciesName}");
            var body = await respone.Content.ReadAsStreamAsync();
            var species = await JsonSerializer.DeserializeAsync<PokeApiSpecies>(body);

            // The url is formated as:
            // https://pokeapi.co/api/v2/generation/5/
            // So the generation will be the 2nd last index in the array
            var split = species.generation.url.Split("/");
            return int.Parse(split[split.Length - 2]);
        }
    }
}
