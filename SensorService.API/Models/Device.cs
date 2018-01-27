using System;
using System.Collections.Generic;

namespace SensorService.API.Models
{
    public class Device
    {
        public Device()
        {
            Sensors = new List<Sensor>();
            IsVisible = true;
            Created = DateTime.Now;
        }
        public string Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Sensor> Sensors { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Created { get; set; }
    }
}