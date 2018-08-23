using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for clPersona
/// </summary>
[DataContract]
public class clPersona
{
    public clPersona()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember]
    public decimal idPersona { get; set; }
    [DataMember]
    public string nbrPersona { get; set; }
    [DataMember]
    public decimal idPaisNacimiento { get; set; }
    [DataMember]
    public decimal idPaisResidencia { get; set; }
    [DataMember]
    public System.DateTime fchNacimiento { get; set; }
    [DataMember]
    public string correo { get; set; }
    [DataMember]
    public int foto { get; set; }
    [DataMember]
    public Nullable<int> videoEntrevista { get; set; }


    public clPersona(decimal idPersona,string nbrPersona, decimal idPaisNacimiento, decimal idPaisResidencia, System.DateTime fchNacimiento, string correo, int foto, Nullable<int> videoEntrevista )
    {
        this.idPersona = idPersona;
        this.nbrPersona = nbrPersona;
        this.idPaisNacimiento = idPaisNacimiento;
        this.idPaisResidencia = idPaisResidencia;
        this.fchNacimiento = fchNacimiento;
        this.correo = correo;
        this.foto = foto;
        this.videoEntrevista = videoEntrevista;


    }
}
