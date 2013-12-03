using System.Collections.Generic;

namespace Models
{
    public class Block
    {
        public Block()
        {
            Particles = new List<Particle>();
        }

        public long Id { get; set; }
        public string Caption { get; set; }
        public IList<Particle> Particles { get; set; }
    }
}
