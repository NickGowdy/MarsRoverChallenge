﻿using System.Collections.Generic;
using System.Linq;
using MarsRover.Domain.Models;
using MarsRover.Models.Domain;

namespace MarsRover.Logic.Services
{
    public class NavigationService
    {
        private readonly List<char> Directions = new List<char> { 'N', 'S', 'E', 'W' };

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

        /// <summary>
        /// Navigate Mars Rover using set of instructions
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns>String of the Mars Rovers final coordinates and the direction it's facing.</returns>
        public string Navigate(IList<string> instructions)
        {
            var parsedInstructionsTuple = ValidateAndParseInsutrctions(instructions);
            if (!parsedInstructionsTuple.isValid) return string.Empty;

            var xLength = parsedInstructionsTuple.parsedInstructions.Plateau.XTotalLength;
            var yLength = parsedInstructionsTuple.parsedInstructions.Plateau.YTotalLength;
            var xStartingPosition = parsedInstructionsTuple.parsedInstructions.StartingPosition.XStartingPosition;
            var yStartingPosition = parsedInstructionsTuple.parsedInstructions.StartingPosition.YStartingPosition;
            var facingDirection = parsedInstructionsTuple.parsedInstructions.StartingPosition.DirectionFacing;

            var positions = CreateSurface(xLength, yLength);
            var position = SetStartingPosition(positions, xStartingPosition, yStartingPosition, facingDirection);

            foreach (var instruction in parsedInstructionsTuple.parsedInstructions.MovementInstructions)
            {
                position = Move(instruction, positions, position);
            }

            return $"{position.XPosition} {position.YPosition} {position.FacingDirection}";
        }


        /// <summary>
        /// Turn or move Mars Rover based on the current instruction
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="positions"></param>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        private Position Move(char instruction, IEnumerable<Position> positions, Position currentPosition)
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

            var newPosition = positions.FirstOrDefault(surface => surface.XPosition == xPosition && surface.YPosition == yPosition);
            newPosition.FacingDirection = currentPosition.FacingDirection;
            return newPosition;
        }

        /// <summary>
        /// Set starting position that Mars Rover will navigate from.
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="facingDirection"></param>
        /// <returns>Position object with x and y coordinates and is occupied flag.</returns>
        private Position SetStartingPosition(IEnumerable<Position> positions, int x, int y, string facingDirection)
        {
            var position = positions.FirstOrDefault(surface => surface.XPosition == x && surface.YPosition == y);
            position.FacingDirection = facingDirection;
            return position;
        }

        /// <summary>
        /// Creates array of possible coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>IEnumerable of possible positions for the Mars Rover to navigate.</returns>
        private static IEnumerable<Position> CreateSurface(int x, int y)
        {
            var positions = new List<Position>();
            for (var i = 0; i <= x; i++)
            {
                for (var ii = 0; ii <= y; ii++)
                {
                    positions.Add(new Position { XPosition = i, YPosition = ii });
                }
            }
            return positions;
        }

        /// <summary>
        /// Helper method to parse and validate Mars Rover inputs.
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns>Boolean based on validation of instructions and tuple of instructions for navigation.</returns>
        private (bool isValid, ParsedInstructions parsedInstructions) ValidateAndParseInsutrctions(IList<string> instructions)
        {
            if (!instructions.Any() || instructions.Count != 3)
                return (isValid: false, parsedInstructions: null);

            var plateau = instructions[0].Split(' ');
            if(plateau.Length != 2)
                return (isValid: false, parsedInstructions: null);

            var startingPosition = instructions[1].Split(' ');
            var movementInstructions = instructions[2].ToArray();

            var invalidInputsList = new List<bool>
            {
                int.TryParse(plateau[0], out var xTotalLength),
                int.TryParse(plateau[1], out var yTotalLength),
                int.TryParse(startingPosition[0], out var xStartingPosition),
                int.TryParse(startingPosition[1], out var yStartingPosition)
            };

            if (plateau.Length != 2 || startingPosition.Length != 3 || invalidInputsList.Contains(false))
                return (isValid: false, null);
           
            return (isValid: true, parsedInstructions: new ParsedInstructions
            {
                Plateau = (xTotalLength, yTotalLength),
                StartingPosition = (xStartingPosition, yStartingPosition, startingPosition[2].FirstOrDefault().ToString()),
                MovementInstructions = movementInstructions
            });
        }
    }
}
