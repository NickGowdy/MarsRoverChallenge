namespace MarsRover.Domain.Models
{
    public class ParsedInstructions
    {
        public (int XTotalLength, int YTotalLength) Plateau { get; set; }
        public (int XStartingPosition, int YStartingPosition, string DirectionFacing) StartingPosition { get; set; }
        public char[] MovementInstructions { get; set; }
    }
}
