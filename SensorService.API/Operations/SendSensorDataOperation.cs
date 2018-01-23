using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class SendSensorDataOperation : OperationBase<DeviceDataDTO>, ISendSensorDataOperation
    {
        public SendSensorDataOperation(SensorContext context) : base(context)
        {
        }

        public override IActionResult OperationBody(DeviceDataDTO deviceDataDTO)
        {
            if (deviceDataDTO == null || deviceDataDTO.SensorData == null || !deviceDataDTO.SensorData.Any())
            {
                return new BadRequestResult();
            }
            var existingDevice = Context.Devices.Include(d => d.Sensors)
                                                 .SingleOrDefault(d => d.Id == deviceDataDTO.DeviceId);
            if (existingDevice == null)
            {
                var createdDevice = new Device { Id = deviceDataDTO.DeviceId };
                foreach (var sensor in deviceDataDTO.SensorData)
                {
                    var createdSensor = new Sensor { SensorKey = sensor.SensorKey, SensorType = sensor.SensorType };
                    var data = new SensorData(sensor.Value);
                    createdSensor.Data.Add(data);
                    createdDevice.Sensors.Add(createdSensor);
                }
                Context.Add(createdDevice);
                Context.SaveChanges();
                return new ObjectResult(createdDevice);
            }
            else
            {
                foreach (var sensor in deviceDataDTO.SensorData)
                {
                    var dataToAdd = new SensorData(sensor.Value);
                    if (!existingDevice.Sensors.Any(s => s.SensorKey == sensor.SensorKey))
                    {
                        var sensorToAdd = new Sensor { SensorKey = sensor.SensorKey, SensorType = sensor.SensorType };
                        sensorToAdd.Data.Add(dataToAdd);
                        existingDevice.Sensors.Add(sensorToAdd);
                    }
                    else
                    {
                        existingDevice.Sensors.SingleOrDefault(s => s.SensorKey == sensor.SensorKey).Data.Add(dataToAdd);
                    }
                }
                Context.Update(existingDevice);
                Context.SaveChanges();
                return new ObjectResult(existingDevice);
            }
        }
    }
}