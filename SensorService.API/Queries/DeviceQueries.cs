using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SensorService.API.Models;
using SensorService.Shared.Dtos;

namespace SensorService.API.Queries
{
    public class DeviceQueries : IDeviceQueries
    {
        private readonly SensorContext _context;

        public DeviceQueries(SensorContext context)
        {
            _context = context;
        }

        public Device GetById(string id)
        {
            return _context.Devices.Include(d => d.Sensors).ThenInclude(d => d.Data).SingleOrDefault(d => d.Id == id);
        }

        public List<Device> Get()
        {
            return _context.Devices.ToList();
        }

        public List<Device> GetByUser(int userId)
        {
            return _context.Devices.Where(d => d.UserId == userId).ToList();
        }

        public Device UpdateDeviceData(int userId, UpdateDeviceDataDto updateDeviceDataDto)
        {
            var existingDevice = GetById(updateDeviceDataDto.DeviceId);
            if (existingDevice == null)
            {
                var createdDevice = new Device
                {
                    Id = updateDeviceDataDto.DeviceId,
                    Name = updateDeviceDataDto.Name,
                    UserId = userId
                };
                foreach (var sensor in updateDeviceDataDto.SensorData)
                {
                    var createdSensor = new Sensor { SensorKey = sensor.SensorKey, SensorType = sensor.SensorType };
                    var data = new SensorData(sensor.Value);
                    createdSensor.Data.Add(data);
                    createdDevice.Sensors.Add(createdSensor);
                }

                _context.Add(createdDevice);
                _context.SaveChanges();
                return createdDevice;
            }

            foreach (var sensor in updateDeviceDataDto.SensorData)
            {
                var dataToAdd = new SensorData(sensor.Value);
                if (existingDevice.Sensors.All(s => s.SensorKey != sensor.SensorKey))
                {
                    var sensorToAdd = new Sensor
                    {
                        SensorKey = sensor.SensorKey,
                        SensorType = sensor.SensorType
                    };
                    sensorToAdd.Data.Add(dataToAdd);
                    existingDevice.Sensors.Add(sensorToAdd);
                }
                else
                {
                    existingDevice.Sensors.SingleOrDefault(s => s.SensorKey == sensor.SensorKey)?.Data
                        .Add(dataToAdd);
                }
            }

            _context.Update(existingDevice);
            _context.SaveChanges();
            return existingDevice;
        }

        public Device Insert(DeviceDto deviceDto)
        {
            var device = new Device
            {
                Name = deviceDto.Name,
                IsVisible = deviceDto.IsVisible,
                Created = DateTime.Now
            };
            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }

        public Device Update(DeviceDto deviceDto)
        {
            var device = GetById(deviceDto.Id);
            if (device == null)
            {
                return null;
            }
            device.UserId = deviceDto.UserId;
            device.Name = deviceDto.Name;
            device.IsVisible = deviceDto.IsVisible;

            _context.Devices.Update(device);
            _context.SaveChanges();
            return device;
        }
    }
}
