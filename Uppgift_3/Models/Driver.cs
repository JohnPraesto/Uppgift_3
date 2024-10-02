using System.ComponentModel.DataAnnotations;

namespace Uppgift_3.Models
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }
        public string DriverName { get; set; }

        // Navigation property for related Incidents
        public ICollection<Incident> Incidents { get; set; }
    }
}
