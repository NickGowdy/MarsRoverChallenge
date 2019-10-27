namespace MarsRover.Models.Domain
{
    public class Instruction
    {
        public Instruction(int x, int y)
        {
            StartPosition = new Position { XPosition = x, YPosition = y };
        }

        private Position StartPosition { get; set; }
    }
}
