using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoUsadosGrupo4
{
    public static class Sesiones
    {
        public static int Rol { get; set; }        // 1 = Administrador, 2= Agente, 3 = Recepcionista, 4 = Cajera 5 = Cliente
        public static string Usuario { get; set; } 
    }
}
