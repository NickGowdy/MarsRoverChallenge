using Xunit;
using MarsRover.Logic;
using System.Collections.Generic;

namespace MarsRover.UnitTests
{
    public class NavigateSurfaceTests
    {
        [Theory]
        [InlineData(new string[] { "5 5", "1 2 N", "LMLMLMLMM" }, "1 3 N", "Rover navigated to expected destination.")]
        [InlineData(new string[] { "5 5", "3 3 E", "MMRMMRMRRM" }, "5 1 E", "Rover navigated to expected destination.")]
        public void NavigateSurfaceTest(string[] input, string expected, string reason)
        {
            // Arrange
            var instructions = new List<string>(input);
            var navigateSurface = new NavigateSurface();

            // Act
            var actual = navigateSurface.Navigate(instructions);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

