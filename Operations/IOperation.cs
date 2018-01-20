using Microsoft.AspNetCore.Mvc;
using SensorAPI.Models;

namespace SensorAPI.Operations
{
    public interface IOperation<T>
    {
        IActionResult Execute(T input);
    }

    public interface IOperation
    {
        IActionResult Execute();
    }
}