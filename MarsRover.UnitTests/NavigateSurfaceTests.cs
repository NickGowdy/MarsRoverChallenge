using Xunit;
using MarsRover.Logic.Services;

namespace MarsRover.UnitTests
{
    public class NavigationServiceTests
    {
        [Theory]
        [InlineData(new string[] { "5 5", "1 2 N", "LMLMLMLMM" }, "1 3 N", "Rover navigated to expected destination.")]
        [InlineData(new string[] { "5 5", "3 3 E", "MMRMMRMRRM" }, "5 1 E", "Rover navigated to expected destination.")]
        [InlineData(new string[] { }, "", "Invalid input because of empty string array")]
        [InlineData(new string[] { "5 5", "3 3", "MMRMMRMRRM" }, "", "Invalid input because the starting destination doesn't have direction.")]
        [InlineData(new string[] { "5 5", "3 3", "WASDRRRLLLSDD" }, "", "Invalid input because of invalid characters.")]
        [InlineData(new string[] { ",,,,rrrr", "3 3", "MMRMMRMRRM" }, "", "Invalid input because of invalid characters.")]
        [InlineData(new string[] { "5", "3 3", "MMRMMRMRRM" }, "", "Invalid input because no x or y position to create surface array.")]
        [InlineData(new string[] { "5 5", "1 2 N", "" }, "1 2 N", "Rover stayed in the same position.")]
        public void NavigateSurfaceTest(string[] input, string expected, string reason)
        {
            // Arrange
            var SUT = new NavigationService();

            // Act
            var actual = SUT.Navigate(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

