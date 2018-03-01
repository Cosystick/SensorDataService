using AutoMapper;
using SensorService.API.Models;
using SensorService.Shared.Dtos;

namespace SensorService.API.Services
{
    public class MapperService : IMapperService
    {
        public MapperService()
        {
            Mapper.Initialize(Configure);
        }

        private void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Device, DeviceDto>();
            cfg.CreateMap<Sensor, SensorDto>();
            cfg.CreateMap<User, UserDto>().BeforeMap((user, dto) => user.Password = string.Empty);
            cfg.CreateMap<SensorData, SensorDataDto>();
        }
    }
}
