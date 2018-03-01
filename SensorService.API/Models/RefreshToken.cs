using System.ComponentModel.DataAnnotations.Schema;

namespace SensorService.API.Models
{
    public class RefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
    }
}