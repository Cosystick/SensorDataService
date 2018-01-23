using Microsoft.AspNetCore.Mvc;

namespace SensorService.API.Operations
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