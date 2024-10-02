using System.ComponentModel.DataAnnotations;

namespace Uppgift_3.Models
{
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }
        public string Description { get; set; }
        public DateTime TimeOfIncident { get; set; }

        // Foreign key to reference the Driver
        public int DriverID { get; set; }
        public Driver Driver { get; set; }
    }
}
