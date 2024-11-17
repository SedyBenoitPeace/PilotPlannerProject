using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApi.Domain.Entities
{
    public class Appointment
    {
        public string AppointmentId { get; set; }
        public string UserId { get; set; }
        public string PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}