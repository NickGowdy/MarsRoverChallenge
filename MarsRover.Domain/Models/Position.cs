namespace MarsRover.Models.Domain
{
    public class Position
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string FacingDirection { get; set; }
        public bool IsOccupied { get; set; }
    }
}
