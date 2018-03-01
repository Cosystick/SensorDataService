using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace SensorService.API.Extensions
{
    public class OkObjectResult<TDestination,TSource> : OkObjectResult
    {
        public OkObjectResult(TSource value) : base(value)
        {
            var mappedResult = Mapper.Map<TDestination>(value);
            Value = mappedResult;
        }
    }
}
