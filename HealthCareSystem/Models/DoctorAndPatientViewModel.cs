using BusinessObjects;

namespace HealthCareSystem.Models
{
    public class DoctorAndPatientViewModel
    {
        public Doctor Doctor { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
