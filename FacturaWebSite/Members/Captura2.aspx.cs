using System;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.IO.Compression;
using MySql.Data.MySqlClient;

public partial class Captura2 : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    Clear_Data();
  }

  protected void UploadBtn_Click(object sender, EventArgs e)
  {
    // Specify the path on the server to
    // save the uploaded file to.
    string savePath = @"c:\temp\uploads\XML\Procesar\";
    // The path and the filename to be uploaded
    string sProcess = string.Empty;

    // Before attempting to save the file, verify
    // that the FileUpload control contains a file or files.
    if (FileUpload1.HasFiles)
    {
      foreach (HttpPostedFile uploadedFile in FileUpload1.PostedFiles)
      {
        // Get the name of the file to upload.
        string fileName = Server.HtmlEncode(uploadedFile.FileName);

        // Get the extension of the uploaded file.
        string extension = System.IO.Path.GetExtension(fileName);

        // Solo los archivos mayores a 1000 bytes serán procesados
        if (uploadedFile.ContentLength > 1000)
        {
          // Append the name of the file to upload to the path.
          sProcess = savePath + fileName;

          // Allow files with .zip or .ZIP extensions
          if (extension.ToLower() == ".zip")
          {
            FileUpload1.SaveAs(sProcess);
            ZipFile.ExtractToDirectory(sProcess, savePath);
            File.Delete(sProcess);
            UploadStatusLabel.Text = "Archivo subido al servidor exitosamente.";

            // Call the function that process the archives uploaded
            Data_Process(savePath);
          }
          else
          {
            // Allow files with .xml or .XML extensions
            if (extension.ToLower() == ".xml")
            {
              // Call the SaveAs method to save the uploaded file to the specified path.
              // This example does not perform the necessary error checking.
              // If a file with the same name already exists in the specified path,
              // the uploaded file overwrites it.
              uploadedFile.SaveAs(sProcess);

              // Notify the user that their file was successfully uploaded.
              UploadStatusLabel.Text = "Archivo subido al servidor exitosamente.";

              // Call the function that process the archive(s) uploaded
              Data_Process(savePath);
            }
            else
            {
              // Notify the user why their file was not uploaded.
              UploadStatusLabel.Text = "El archivo no ha sido subido debido " +
                                       "a que no tiene extensión .xml o .zip";
              Clear_Data();
            }
          }
        }
        else
        {
          UploadStatusLabel.Text = "El archivo XML está dañado.";
        }
      }
    }
  }

  protected string[] sValida_XML(string sFile_XML)
  {
    // The route where XML files are placed to be processed
    string path = @"c:\temp\uploads\XML\procesar\";
    // Adds the file number
    string sRuta_XML = path + sFile_XML;
    XmlTextReader reader = null;
    XmlDocument xmlDoc = new XmlDocument();
    string[] sComprobante;
    sComprobante = new string[20];
    // Variables string para guardar los atributos correspondientes
    string sUuid = string.Empty;
    string cantidad = string.Empty;
    string unidad = string.Empty;
    string descripcion = string.Empty;
    string valorunitario = string.Empty;
    string importe = string.Empty;
    string subtotal = string.Empty;
    string tasa_iva = string.Empty;
    string importe_iva = string.Empty;
    string domicilio_e = string.Empty;
    string domicilio_r = string.Empty;
    string nombre_e = string.Empty; // Nombre del emisor de factura
    string nombre_r = string.Empty; // Nombre del receptor de factura
    string fecha_e = string.Empty; // Fecha de emisión factura
    string fecha_t = string.Empty; // Fecha timbrado factura

    try
    {
      // Set up parameters for the XML document
      NameTable nt = new NameTable();
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
      nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
      nsmgr.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
      xmlDoc.Load(sRuta_XML);
      var Comprobante = xmlDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr);
      if (Comprobante == null)
      {
        sComprobante[0] = "";
        sComprobante[1] = "";
        sComprobante[2] = "";
        sComprobante[3] = "";
        sComprobante[4] = "";
        sComprobante[5] = "";
        sComprobante[6] = "";
        sComprobante[7] = "";
        sComprobante[8] = "";
        sComprobante[9] = "";
        sComprobante[10] = "";
        sComprobante[11] = "";
        sComprobante[12] = "";
        sComprobante[13] = "";
        sComprobante[14] = "";
        sComprobante[15] = "";
        sComprobante[16] = "";
        sComprobante[17] = "";
        sComprobante[18] = "";
        sComprobante[19] = "";
      }
      else
      {
        var XmlComprobante = xmlDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr);
        var Cfdi_Emisor = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Emisor", nsmgr);
        var Cfdi_Receptor = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Receptor", nsmgr);
        var Complemento_TFD = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmgr);
        var XmlConceptos = xmlDoc.GetElementsByTagName("cfdi:Conceptos");
        var XmlConcepto = ((XmlElement)XmlConceptos[0]).GetElementsByTagName("cfdi:Concepto");
        var TasaIva = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmgr);
        var DomicilioEmisor = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal", nsmgr);
        var DomicilioReceptor = xmlDoc.SelectSingleNode("//cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio", nsmgr);

        if (XmlComprobante != null)
        {
          if (XmlComprobante.Attributes["subTotal"] != null)
          {
            subtotal = XmlComprobante.Attributes["subTotal"].Value;
          }

          if (XmlComprobante.Attributes["SubTotal"] != null)
          {
            subtotal = XmlComprobante.Attributes["SubTotal"].Value;
          }

          if (XmlComprobante.Attributes["fecha"] != null)
          {
            fecha_e = XmlComprobante.Attributes["fecha"].Value;
          }

          if (XmlComprobante.Attributes["Fecha"] != null)
          {
            fecha_e = XmlComprobante.Attributes["Fecha"].Value;
          }

          if (XmlComprobante.Attributes["total"] != null)
          {
            sComprobante[0] = XmlComprobante.Attributes["total"].Value;
          }

          if (XmlComprobante.Attributes["Total"] != null)
          {
            sComprobante[0] = XmlComprobante.Attributes["Total"].Value;
          }
        }

        if (Cfdi_Emisor != null)
        {
          if (Cfdi_Emisor.Attributes["rfc"] != null)
          {
            sComprobante[1] = Cfdi_Emisor.Attributes["rfc"].Value; // Clave RFC Emisor
          }

          if (Cfdi_Emisor.Attributes["Rfc"] != null)
          {
            sComprobante[1] = Cfdi_Emisor.Attributes["Rfc"].Value; // Clave RFC Emisor
          }

          if (Cfdi_Emisor.Attributes["nombre"] != null)
          {
            sComprobante[16] = Cfdi_Emisor.Attributes["nombre"].Value; // Asignacion del nombre emisor de factura
          }

          if (Cfdi_Emisor.Attributes["Nombre"] != null)
          {
            sComprobante[16] = Cfdi_Emisor.Attributes["Nombre"].Value; // Asignacion del nombre emisor de factura
          }
        }

        if (Cfdi_Receptor != null)
        {
          if (Cfdi_Receptor.Attributes["rfc"] != null)
          {
            sComprobante[2] = Cfdi_Receptor.Attributes["rfc"].Value; // Clave RFC Emisor
          }

          if (Cfdi_Receptor.Attributes["Rfc"] != null)
          {
            sComprobante[2] = Cfdi_Receptor.Attributes["Rfc"].Value; // Clave RFC Emisor
          }

          if (Cfdi_Receptor.Attributes["nombre"] != null)
          {
            sComprobante[17] = Cfdi_Receptor.Attributes["nombre"].Value; // Asignacion del nombre emisor de factura
          }

          if (Cfdi_Receptor.Attributes["Nombre"] != null)
          {
            sComprobante[17] = Cfdi_Receptor.Attributes["Nombre"].Value; // Asignacion del nombre emisor de factura
          }
        }

        if (Complemento_TFD != null)
        {
          if (Complemento_TFD.Attributes["UUID"] != null)
          {
            sComprobante[3] = Complemento_TFD.Attributes["UUID"].Value; // Código UUID
          }

          if (Complemento_TFD.Attributes["FechaTimbrado"] != null)
          {
            fecha_t = Complemento_TFD.Attributes["FechaTimbrado"].Value;
          }
        }

        if (DomicilioEmisor != null)
        {
          if (DomicilioEmisor.Attributes["calle"] != null)
          {
            domicilio_e = DomicilioEmisor.Attributes["calle"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["noExterior"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["noExterior"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["colonia"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["colonia"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["localidad"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["localidad"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["municipio"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["municipio"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["estado"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["estado"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["pais"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["pais"].Value + "\n";
          }

          if (DomicilioEmisor.Attributes["codigoPostal"] != null)
          {
            domicilio_e = domicilio_e + DomicilioEmisor.Attributes["codigoPostal"].Value + "\n";
          }
        }

        if (TasaIva != null)
        {
          if (TasaIva.Attributes["tasa"] != null)
          {
            tasa_iva = TasaIva.Attributes["tasa"].Value;
          }

          if (TasaIva.Attributes["TasaOCuota"] != null)
          {
            tasa_iva = TasaIva.Attributes["TasaOCuota"].Value;
          }

          if (TasaIva.Attributes["importe"] != null)
          {
            importe_iva = TasaIva.Attributes["importe"].Value;
          }

          if (TasaIva.Attributes["Importe"] != null)
          {
            importe_iva = TasaIva.Attributes["Importe"].Value;
          }
        }

        if (DomicilioReceptor != null)
        {
          if (DomicilioReceptor.Attributes["calle"] != null)
          {
            domicilio_r = DomicilioReceptor.Attributes["calle"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["noExterior"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["noExterior"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["noInterior"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["noInterior"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["colonia"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["colonia"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["municipio"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["municipio"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["estado"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["estado"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["pais"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["pais"].Value + "\n";
          }

          if (DomicilioReceptor.Attributes["codigoPostal"] != null)
          {
            domicilio_r = domicilio_r + DomicilioReceptor.Attributes["codigoPostal"].Value + "\n";
          }
        }

        if (TasaIva != null)
        {
          if (TasaIva.Attributes["tasa"] != null)
          {
            tasa_iva = TasaIva.Attributes["tasa"].Value;
          }

          if (TasaIva.Attributes["importe"] != null)
          {
            importe_iva = TasaIva.Attributes["importe"].Value;
          }
        }

        //Save the parameters to an string array
        sComprobante[4] = sRuta_XML;
        sComprobante[5] = sFile_XML;

        foreach (XmlElement atributo in XmlConcepto)
        {
          cantidad = cantidad + " " + (atributo.GetAttribute("cantidad")).Trim() + "\n";
          cantidad = cantidad + " " + (atributo.GetAttribute("Cantidad")).Trim() + "\n";
          unidad = unidad + " " + (atributo.GetAttribute("unidad")).Trim() + "\n";
          descripcion = descripcion + " " + (atributo.GetAttribute("descripcion")).Trim() + "\n";
          descripcion = descripcion + " " + (atributo.GetAttribute("Descripcion")).Trim() + "\n";
          valorunitario = valorunitario + " " + (atributo.GetAttribute("valorUnitario")).Trim() + "\n";
          valorunitario = valorunitario + " " + (atributo.GetAttribute("ValorUnitario")).Trim() + "\n";
          importe = importe + " " + (atributo.GetAttribute("importe")).Trim() + "\n";
          importe = importe + " " + (atributo.GetAttribute("Importe")).Trim() + "\n";
        }

        sComprobante[6] = cantidad;
        sComprobante[7] = unidad;
        sComprobante[8] = descripcion;
        sComprobante[9] = valorunitario;
        sComprobante[10] = importe;
        sComprobante[11] = subtotal;
        sComprobante[12] = tasa_iva;
        sComprobante[13] = importe_iva;
        sComprobante[14] = domicilio_e; // Domicilio Fiscal Emisor
        sComprobante[15] = domicilio_r; // Domicilio Fiscal Receptor
        sComprobante[18] = fecha_e; // Fecha emisión de factura
        sComprobante[19] = fecha_t; // Fecha timbrado de factura
      }

      TotalLabel.Text = sComprobante[0];
      RfcEmisorLabel.Text = sComprobante[1];
      RfcReceptorLabel.Text = sComprobante[2];
      UuidLabel.Text = sComprobante[3];
      NombreArchLabel.Text = sComprobante[5];
    }
    finally
    {
      if (reader != null)
      {
        reader.Close();
      }
    }

    return sComprobante;
  }

  protected string[] sLee_XML(string pDirectorio)
  {
    // Obtains information from the process directory
    DirectoryInfo dir = new DirectoryInfo(pDirectorio);
    List<string> lFiles = new List<string>();
    // Saves all the XML files found in a list
    foreach (FileInfo file in dir.GetFiles("*.xml"))
    {
      lFiles.Add(file.Name);
    }

    // Convert the list into array
    string[] sFileList = lFiles.ToArray();

    // Returns the list
    return sFileList;
  }

  protected void sValida_SAT(string[] sData)
  {
    string[] sCfdiData;
    sCfdiData = new string[22];
    string sCadena = string.Empty;
    string sEstado = string.Empty;
    string sNuevoArch = string.Empty;

    CFDIService.Acuse oAcuse = new CFDIService.Acuse();
    CFDIService.ConsultaCFDIServiceClient oConsulta = new CFDIService.ConsultaCFDIServiceClient();
    sCadena = "?re=" + sData[1] + "&rr=" + sData[2] + "&tt=" + sData[0] + "&id=" + sData[3] + "";
    oAcuse = oConsulta.Consulta(sCadena);

    sCfdiData[0] = sData[1]; // RFC Emisor
    sCfdiData[1] = sData[2]; // RFC Receptor
    sCfdiData[2] = sData[0]; // Total
    sCfdiData[3] = sData[3]; // UUID
    sCfdiData[4] = sData[4]; // Ruta XML
    sCfdiData[5] = sData[5]; // Nombre archivo XML
    sCfdiData[6] = oAcuse.CodigoEstatus.ToString(); // Estatus en SAT
    sCfdiData[7] = oAcuse.Estado.ToString(); // Estado en SAT
    sCfdiData[8] = sData[6]; // Concepto -> cantidad
    sCfdiData[9] = sData[7]; // Concepto -> unidad
    sCfdiData[10] = sData[8]; // Concepto -> descripcion
    sCfdiData[11] = sData[9]; // Concepto -> valorunitario
    sCfdiData[12] = sData[10]; // Concepto -> importe
    sCfdiData[13] = sData[11]; // Subtotal
    sCfdiData[14] = sData[12]; // Tasa IVA
    sCfdiData[15] = sData[13]; // Importe de IVA
    sCfdiData[16] = sData[14]; // Domicilio Fiscal Emisor
    sCfdiData[17] = sData[15]; // Domicilio Fiscal Receptor
    sCfdiData[18] = sData[16]; // Nombre emisor factura
    sCfdiData[19] = sData[17]; // Nombre receptir factura
    sCfdiData[20] = sData[18]; // Fecha emisión factura
    sCfdiData[21] = sData[19]; // Fecha timbrado factura

    CodigoEstatusLabel.Text = oAcuse.CodigoEstatus.ToString();
    sEstado = oAcuse.Estado.ToString();
    EstadoLabel.Text = oAcuse.Estado.ToString();

    sData_DBSave(sCfdiData);
  }

  protected void Clear_Data()
  {
    TotalLabel.Text = string.Empty;
    RfcEmisorLabel.Text = string.Empty;
    RfcReceptorLabel.Text = string.Empty;
    UuidLabel.Text = string.Empty;
    NombreArchLabel.Text = string.Empty;
    CodigoEstatusLabel.Text = string.Empty;
    EstadoLabel.Text = string.Empty;
  }

  protected void Data_Process(string RutaDirectorio)
  {
    // String array for the received information
    string[] sComprobantes;

    sComprobantes = sLee_XML(RutaDirectorio);

    if (sComprobantes.Length >= 1)
    {
      foreach (string file in sComprobantes)
      {
        sComprobantes = sValida_XML(file);
        if (sComprobantes[0] != "")
        {
          sValida_SAT(sComprobantes);
        }
        else
        {
          UploadStatusLabel.Text = "El archivo no es factura válida del SAT";
          Clear_Data();
          NombreArchLabel.Text = file;
          File.Delete(RutaDirectorio + file);
        }
      }
    }
    else
    {
      sComprobantes = sValida_XML(sComprobantes[0]);
      if (sComprobantes[0] != "")
      {
        sValida_SAT(sComprobantes);
      }
      else
      {
        UploadStatusLabel.Text = "El archivo no es factura válida del SAT";
        Clear_Data();
        NombreArchLabel.Text = sComprobantes[0];
        File.Delete(RutaDirectorio + sComprobantes[0]);
      }
    }
  }

  protected void sData_DBSave(string[] datos)
  {
    byte[] rawData = File.ReadAllBytes(datos[4]);
    FileInfo info = new FileInfo(datos[4]);
    DateTime temp_proceso = new DateTime();

    int FileSize = Convert.ToInt32(info.Length);

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    using (MySqlConnection connection = new MySqlConnection(constr))
    {
      using (MySqlCommand command = new MySqlCommand())
      {
        try
        {

          command.Connection = connection;
          command.CommandText = "INSERT INTO cfdi_facturas (rfc_e,nom_e,dom_e,rfc_r,nom_r,dom_r,uuid,total,estatus,estado,fecha,cantidad,unidad,descripcion,identificador,valorunitario,importe,subtotal,tasa_iva,importe_iva,fecha_emision,fecha_timbrado,arch_name,arch_size,arch_data) VALUES (?Rfc_e,?Nom_e,?Dom_e,?Rfc_r,?nom_r,?Dom_r,?Uuid,?Total,?Estatus,?Estado,?Fecha,?Cantidad,?Unidad,?Descripcion,?Identi,?Valorunitario,?Importe,?Subtotal,?Tasa_iva,?Importe_iva,?Fecha_e,?Fecha_t,?Arch_name,?Arch_size,?Arch_data);";


          MySqlParameter RfcEParameter = new MySqlParameter("?Rfc_e", MySqlDbType.VarChar, 15);
          MySqlParameter NomEParameter = new MySqlParameter("?Nom_e", MySqlDbType.VarChar, 300);
          MySqlParameter DomiEParameter = new MySqlParameter("?Dom_e", MySqlDbType.VarChar, 1000);
          MySqlParameter RfcRParameter = new MySqlParameter("?Rfc_r", MySqlDbType.VarChar, 15);
          MySqlParameter NomRParameter = new MySqlParameter("?Nom_r", MySqlDbType.VarChar, 300);
          MySqlParameter DomiRParameter = new MySqlParameter("?Dom_r", MySqlDbType.VarChar, 1000);
          MySqlParameter UuidParameter = new MySqlParameter("?Uuid", MySqlDbType.VarChar, 50);
          MySqlParameter TotalParameter = new MySqlParameter("?Total", MySqlDbType.VarChar, 40);
          MySqlParameter EstatusParameter = new MySqlParameter("?Estatus", MySqlDbType.VarChar, 50);
          MySqlParameter EstadoParameter = new MySqlParameter("?Estado", MySqlDbType.VarChar, 50);
          MySqlParameter FechaParameter = new MySqlParameter("?Fecha", MySqlDbType.DateTime);
          MySqlParameter CantidadParameter = new MySqlParameter("?Cantidad", MySqlDbType.VarChar, 300);
          MySqlParameter UnidadParameter = new MySqlParameter("?Unidad", MySqlDbType.VarChar, 300);
          MySqlParameter DescripcionParameter = new MySqlParameter("?Descripcion", MySqlDbType.VarChar, 5000);
          MySqlParameter IdentificadorParameter = new MySqlParameter("?Identi", MySqlDbType.VarChar, 50);
          MySqlParameter ValorunitarioParameter = new MySqlParameter("?Valorunitario", MySqlDbType.VarChar, 300);
          MySqlParameter ImporteParameter = new MySqlParameter("?Importe", MySqlDbType.VarChar, 300);
          MySqlParameter SubtotalParameter = new MySqlParameter("?Subtotal", MySqlDbType.VarChar, 300);
          MySqlParameter TasaIvaParameter = new MySqlParameter("?Tasa_iva", MySqlDbType.VarChar, 10);
          MySqlParameter ImporteIvaParameter = new MySqlParameter("?Importe_iva", MySqlDbType.VarChar, 300);
          MySqlParameter FechaEParameter = new MySqlParameter("?Fecha_e", MySqlDbType.VarChar, 50);
          MySqlParameter FechaTParameter = new MySqlParameter("?Fecha_t", MySqlDbType.VarChar, 50);
          MySqlParameter ArchNParameter = new MySqlParameter("?Arch_name", MySqlDbType.VarChar, 128);
          MySqlParameter ArchSParameter = new MySqlParameter("?Arch_size", MySqlDbType.Int32, 11);
          MySqlParameter ArchDParameter = new MySqlParameter("?Arch_data", MySqlDbType.Blob, rawData.Length);

          RfcEParameter.Value = datos[0];
          RfcRParameter.Value = datos[1];
          UuidParameter.Value = datos[3].ToUpper();
          TotalParameter.Value = datos[2];
          EstatusParameter.Value = datos[6];
          EstadoParameter.Value = datos[7];
          ArchNParameter.Value = datos[5].ToLower();
          ArchSParameter.Value = FileSize;
          ArchDParameter.Value = rawData;
          temp_proceso = DateTime.Now;
          FechaParameter.Value = temp_proceso.ToString(format: "yyyy-MM-dd HH:mm:ss");
          CantidadParameter.Value = datos[8];
          UnidadParameter.Value = datos[9];
          DescripcionParameter.Value = datos[10];
          IdentificadorParameter.Value = IdentificadorTextBox.Text;
          ValorunitarioParameter.Value = datos[11];
          ImporteParameter.Value = datos[12];
          SubtotalParameter.Value = datos[13];
          TasaIvaParameter.Value = datos[14];
          ImporteIvaParameter.Value = datos[15];
          DomiEParameter.Value = datos[16];
          DomiRParameter.Value = datos[17];
          NomEParameter.Value = datos[18];
          NomRParameter.Value = datos[19];
          FechaEParameter.Value = datos[20];
          FechaTParameter.Value = datos[21];

          command.Parameters.Add(RfcEParameter);
          command.Parameters.Add(DomiEParameter);
          command.Parameters.Add(RfcRParameter);
          command.Parameters.Add(DomiRParameter);
          command.Parameters.Add(UuidParameter);
          command.Parameters.Add(TotalParameter);
          command.Parameters.Add(EstatusParameter);
          command.Parameters.Add(EstadoParameter);
          command.Parameters.Add(FechaParameter);
          command.Parameters.Add(CantidadParameter);
          command.Parameters.Add(UnidadParameter);
          command.Parameters.Add(DescripcionParameter);
          command.Parameters.Add(IdentificadorParameter);
          command.Parameters.Add(ValorunitarioParameter);
          command.Parameters.Add(ImporteParameter);
          command.Parameters.Add(SubtotalParameter);
          command.Parameters.Add(TasaIvaParameter);
          command.Parameters.Add(ImporteIvaParameter);
          command.Parameters.Add(ArchNParameter);
          command.Parameters.Add(ArchSParameter);
          command.Parameters.Add(ArchDParameter);
          command.Parameters.Add(NomEParameter);
          command.Parameters.Add(NomRParameter);
          command.Parameters.Add(FechaEParameter);
          command.Parameters.Add(FechaTParameter);

          connection.Open();

          command.ExecuteNonQuery();

          connection.Close();

          // Remove the processed file because it is already in the DB
          if (File.Exists(datos[4]))
          {
            File.Delete(datos[4]);
          }
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
          UploadStatusLabel.Text = "Error " + ex.Number + " ha ocurrido: " + ex.Message;
        }
      }
    }
  }

  protected void LimpiarButton_Click(object sender, EventArgs e)
  {
    IdentificadorTextBox.Text = string.Empty;
  }
}