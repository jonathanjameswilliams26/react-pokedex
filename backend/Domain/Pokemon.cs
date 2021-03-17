using System.Collections.Generic;

namespace Backend.Domain
{
    public class Pokemon
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public int Generation { get; set; }
        public string Artwork { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int HP { get; set; }
        public int Speed { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefence { get; set; }
        public List<Type> Types { get; set; } = new List<Type>();
    }
}
