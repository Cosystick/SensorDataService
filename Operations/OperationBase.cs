using Microsoft.AspNetCore.Mvc;
using SensorAPI.Models;

namespace SensorAPI.Operations
{
    public abstract class OperationBase<T> : IOperation<T>
    {
        public OperationBase(SensorContext context)
        {
            Context = context;
        }

        public SensorContext Context { get; set; }

        public abstract IActionResult OperationBody(T input);

        public IActionResult Execute(T input)
        {
            return OperationBody(input);
        }
    }

    public abstract class OperationBase : IOperation
    {
        public OperationBase(SensorContext context)
        {
            Context = context;
        }

        public SensorContext Context { get; set; }

        public abstract IActionResult OperationBody();

        public IActionResult Execute()
        {
            return OperationBody();
        }
    }
}