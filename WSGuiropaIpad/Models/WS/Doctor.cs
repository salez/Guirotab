using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.WS
{
    public class Doctor
    {
        public Doctor()
        {
            this.Name = String.Empty;
            this.CRM = String.Empty;
            this.PhonePrefix = String.Empty;
            this.PhoneNumber = String.Empty;
            this.Address = String.Empty;
            this.Birthday = String.Empty;
            this.Speciality = String.Empty;
            this.CRMUF = String.Empty;
        }

        public String Id { get; set; }
        public String Name { get; set; }
        public String CRM { get; set; }
        public String CRMUF { get; set; }
        public String PhonePrefix { get; set; }
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
        public String Birthday { get; set; }
        public String Speciality { get; set; }
        public String Version { get; set; }

        /// <summary>
        /// lista de vas na ordem de apresentação
        /// </summary>
        public int[] PresentationId { get; set; }
    }
}
