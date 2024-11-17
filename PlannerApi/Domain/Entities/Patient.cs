using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApi.Domain.Entities
{
    public class Patient
    {

        public class ContactInfo
        {
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }


        public string PatientId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactInfo ContantInfos { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}