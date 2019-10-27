using System.Collections.Generic;
using System.Linq;
using MarsRover.Models.Domain;

namespace MarsRover.Logic
{
    public class NavigateSurface
    {
        private List<char> Directions = new List<char> { 'N', 'S', 'E', 'W' };

        private List<(char FacingDirection, char Instruction, char NewDirection)> DirectionLookup
            = new List<(char FacingDirection, char Instruction, char NewDirection)>
            {
                ('N', 'L', 'W'),
                ('W', 'L', 'S'),
                ('S', 'L', 'E'),
                ('E', 'L', 'N'),
                ('N', 'R', 'E'),
                ('E', 'R', 'S'),
                ('S', 'R', 'W'),
                ('W', 'R', 'N'),

            };

        public string Navigate(List<string> Instructions)
        {
            if (!Instructions.Any() || Instructions.Count != 3) return string.Empty;
            var plateau = Instructions[0].Split(' ');
            Surface surface = CreateSurface(plateau);
            if (!surface.Positions.Any()) return string.Empty;
            Instructions.RemoveAt(0);

            var startingPositionInstructions = Instructions[0].Split(' ');
            var position = SetStartingPosition(surface, startingPositionInstructions);
            if (!position.IsOccupied) return string.Empty;
            Instructions.RemoveAt(0);

            var movementInstructions = Instructions[0].ToArray();


            foreach (var instruction in movementInstructions)
            {
                position = Move(instruction, surface, position);
            }

            return $"{position.XPosition} {position.YPosition} {position.FacingDirection}";
        }

        private Position Move(char instruction, Surface surface, Position currentPosition)
        {
            if (instruction != 'M')
            {
                var lookupValue = DirectionLookup
                    .FirstOrDefault(lookup => lookup.FacingDirection == currentPosition.FacingDirection.ToCharArray().FirstOrDefault() && lookup.Instruction == instruction);

                currentPosition.FacingDirection = lookupValue.NewDirection.ToString();
                return currentPosition;
            }

            var movementAxis = currentPosition.FacingDirection == "E" || currentPosition.FacingDirection == "W" ? "X" : "Y";
            var xPosition = currentPosition.XPosition;
            var yPosition = currentPosition.YPosition;

            if (movementAxis == "X")
            {
                xPosition = currentPosition.FacingDirection == "W" ? currentPosition.XPosition - 1 : currentPosition.XPosition + 1;
            }
            else
            {
                yPosition = currentPosition.FacingDirection == "S" ? currentPosition.YPosition - 1 : currentPosition.YPosition + 1;
            }

            var newPosition = surface.Positions.FirstOrDefault(surface => surface.XPosition == xPosition && surface.YPosition == yPosition);
            newPosition.FacingDirection = currentPosition.FacingDirection;
            return newPosition;
        }

        private Position SetStartingPosition(Surface surface, string[] startingPositionInstructions)
        {
            if (startingPositionInstructions.Length == 3 &&
                int.TryParse(startingPositionInstructions[0], out var x) &&
                int.TryParse(startingPositionInstructions[1], out var y) &&
                Directions.Contains(startingPositionInstructions[2].FirstOrDefault()))
            {
                foreach (var position in surface.Positions)
                {
                    if (position.XPosition == x && position.YPosition == y && !position.IsOccupied)
                    {
                        position.IsOccupied = true;
                        position.FacingDirection = startingPositionInstructions[2].ToUpper();
                        return position;
                    }
                }
            }

            return new Position();
        }

        private static Surface CreateSurface(string[] plateau)
        {
            if (plateau.Length == 2 && int.TryParse(plateau[0], out var x) && int.TryParse(plateau[1], out var y))
            {
                var surface = new Surface
                {
                    XMinBoundary = -1,
                    YMinBoundary = -1,
                    XMaxBoundary = x-1,
                    YMaxBoundary = y-1
                };

                for (var i = 0; i <= x; i++)
                {
                    for (var ii = 0; ii <= y; ii++)
                    {
                        surface.Positions.Add(new Position { XPosition = i, YPosition = ii });
                    }
                }
                return surface;
            }
            return new Surface();
        }
    }
}
