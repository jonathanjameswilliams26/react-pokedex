using System.Collections.Generic;

namespace Backend.Domain
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pokemon> Pokemon { get; set; } = new List<Pokemon>();
    }
}
