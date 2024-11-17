using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApi.Domain.Entities
{
    public static class CosmosDbContainerName
    {
        public const string Users = "Users";
        public const string Patients = "Patients";
        public const string Appointments = "Appointments";
    }
}