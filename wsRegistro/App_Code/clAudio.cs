using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for clAudio
/// </summary>
[DataContract]
public class clAudio
{
    public clAudio()
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


    public clAudio(int id, string descripcion, byte[] info_bytes)
    {
        this.id = id;
        this.descripcion = descripcion;
        this.info_bytes = info_bytes;

    }
}
