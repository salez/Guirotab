using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Controllers
{
    public class GruposFormView
    {
        ControladorRepository controladorRepository = new ControladorRepository();

        public IQueryable<Controlador> Controladores { get; set; }
        public Grupo Grupo { get; set; }

        public GruposFormView()
        {
            Controladores = controladorRepository.GetControladores();
        }

        public GruposFormView(Grupo grupo)
        {
            this.Grupo = grupo;
            Controladores = controladorRepository.GetControladores();
        }

    }
}
