using System;
using System.Collections.Generic;

namespace DGIIApi.Models;

public partial class ListadoContribuyente
{
    public string RncCedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool Tipo { get; set; }

    public bool Estatus { get; set; }
}
