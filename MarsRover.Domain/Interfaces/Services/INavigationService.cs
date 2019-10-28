using System.Collections.Generic;

namespace MarsRover.Domain.Interfaces.Services
{
    public interface INavigationService
    {
        string Navigate(IList<string> instructions);
    }
}
