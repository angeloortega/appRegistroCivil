using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for clImagen
/// </summary>
[DataContract]
public class clImagen
{
    public clImagen()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [DataMember]
    public int id { get; set; }
    [DataMember]
    public string descripcion { get; set; }
    [DataMember]
    public byte[] info_bytes { get; set; }


    public clImagen(int id, string descripcion, byte[] info_bytes)
    {
        this.id = id;
        this.descripcion = descripcion;
        this.info_bytes = info_bytes;

    }
}
