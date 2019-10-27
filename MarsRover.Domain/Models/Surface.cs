using System.Collections.Generic;

namespace MarsRover.Models.Domain
{
    public class Surface
    {
        public Surface()
        {
            Positions = new List<Position>();
           
        }

        public IList<Position> Positions { get; set; }
        public int XMinBoundary { get; set; }
        public int YMinBoundary { get; set; }
        public int XMaxBoundary { get; set; }
        public int YMaxBoundary { get; set; }
    }
}
