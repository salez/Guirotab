using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.WS
{
    /// <summary>
    /// Usuário
    /// </summary>
    public class User
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public List<Territory> Territorys { get; set; }

        public User()
        {
            this.Territorys = new List<Territory>();
        }
    }

    public class Territory
    {
        public String Id { get; set; }
        public String Name { get; set; }
    }
}
