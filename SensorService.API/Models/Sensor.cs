using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorService.API.Models
{
    public class Sensor
    {
        public Sensor()
        {
            Data = new List<SensorData>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SensorKey { get; set; }
        public int SensorType { get; set; }
        public List<SensorData> Data { get; set; }
    }

    public class SensorData
    {
        public SensorData()
        {
            Created = DateTime.Now;
        }
        public SensorData(long value)
        {
            Created = DateTime.Now;
            Value = value;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public long Value { get; set; }
    }
}