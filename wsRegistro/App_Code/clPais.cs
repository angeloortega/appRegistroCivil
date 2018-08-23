using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for clPais
/// </summary>
[DataContract]
public class clPais
{
    public clPais()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember]
    public decimal idPais { get; set; }
    [DataMember]
    public string nbrPais { get; set; }
    [DataMember]
    public decimal area { get; set; }
    [DataMember]
    public decimal poblacionActual { get; set; }
    [DataMember]
    public int fotoBandera { get; set; }
    [DataMember]
    public int himnoNacional { get; set; }
    [DataMember]
    public Nullable<decimal> idPresidenteActual { get; set; }


    public clPais(decimal idPais, string nbrPais, decimal area, decimal poblacionActual, int fotoBandera, int himnoNacional, Nullable<decimal> idPresidenteActual)
    {
        this.idPais = idPais;
        this.nbrPais = nbrPais;
        this.area = area;
        this.poblacionActual = poblacionActual;
        this.fotoBandera = fotoBandera;
        this.himnoNacional = himnoNacional;
        this.idPresidenteActual = idPresidenteActual;

    }
}
