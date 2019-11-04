using System.Collections.Generic;

namespace MarsRover.Logic.Services
{
    public class AdvancedNavigationService : NavigationService
    {
        protected new IReadOnlyCollection<(string FacingDirection, char Instruction, string NewDirection)> DirectionLookup
            = new List<(string FacingDirection, char Instruction, string NewDirection)>
            {
                ("N", 'L', "NW"),
                ("NW", 'L', "W"),
                ("W", 'L', "SW"),
                ("SW", 'L', "S"),
                ("S", 'L', "SE"),
                ("SE", 'L', "E"),
                ("S", 'L', "NE"),
                ("S", 'L', "N"),
                ("N", 'R', "NE"),
                ("NE", 'R', "E"),
                ("E", 'R', "SE"),
                ("SE", 'R', "S"),
                ("S", 'R', "SW"),
                ("SW", 'R', "W"),
                ("W", 'R', "NW"),
                ("NW", 'R', "N"),
            };
    }
}
