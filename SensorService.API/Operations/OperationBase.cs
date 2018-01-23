using Microsoft.AspNetCore.Mvc;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public abstract class OperationBase<T> : IOperation<T>
    {
        protected OperationBase(SensorContext context)
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
        protected OperationBase(SensorContext context)
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