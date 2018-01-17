using System;
using MySql.Data.MySqlClient;
using System.Configuration;

public partial class Captura : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    Clear_Data();
    QrTextBox.Focus();
  }

  protected void Clear_Data()
  {
    TotalLabel.Text = string.Empty;
    RfcEmisorLabel.Text = string.Empty;
    RfcReceptorLabel.Text = string.Empty;
    UuidLabel.Text = string.Empty;
    CodigoEstatusLabel.Text = string.Empty;
    EstadoLabel.Text = string.Empty;
  }

  protected void Procesar_Click(object sender, EventArgs e)
  {
    string[] datos;
    datos = new string[4];
    string SQrDatos = string.Empty;

    if (TotalTextBox.Text != "")
    {
      if (RfcETextBox.Text != "")
      {
        if (RfcRTextBox.Text != "")
        {
          if (UuidTextBox.Text != "")
          {
            datos[0] = TotalTextBox.Text;
            datos[1] = RfcETextBox.Text;
            datos[2] = RfcRTextBox.Text;
            datos[3] = UuidTextBox.Text;

            sValida_SAT(datos, SQrDatos);
            TotalTextBox.Text = "";
            RfcETextBox.Text = "";
            RfcRTextBox.Text = "";
            UuidTextBox.Text = "";
          }
          else
          {
            UploadStatusLabel.Text = "Ningún campo puede estar vacío a fin de procesar la información!!!";
          }
        }
        else
        {
          UploadStatusLabel.Text = "Ningún campo puede estar vacío a fin de procesar la información!!!";
        }
      }
      else
      {
        UploadStatusLabel.Text = "Ningún campo puede estar vacío a fin de procesar la información!!!";
      }
    }

    if (QrTextBox.Text != "")
    {
      SQrDatos = QrTextBox.Text;
      sValida_SAT(datos, SQrDatos);
      TotalTextBox.Text = "";
      RfcETextBox.Text = "";
      RfcRTextBox.Text = "";
      UuidTextBox.Text = "";
      QrTextBox.Text = "";
    }
    //else
    //{
    //  UploadStatusLabel.Text = "Ningún campo puede estar vacío a fin de procesar la información!!!";
    //}
  }

  protected void sValida_SAT(string[] sData, string sQr)
  {
    string[] sCfdiData;
    sCfdiData = new string[9];
    string sCadena = string.Empty;
    string sEstado = string.Empty;
    string sNuevoArch = string.Empty;
    string delimStr1 = "?&";
    char[] delimiter1 = delimStr1.ToCharArray();
    string sDirCfdi = string.Empty;
    string sEmisor = "re=";
    char[] cEmisor = sEmisor.ToCharArray();
    string sReceptor = "rr=";
    char[] cReceptor = sReceptor.ToCharArray();
    string sTotal = "tt=";
    char[] cTotal = sTotal.ToCharArray();
    string sUuid = "id=";
    char[] cUuid = sUuid.ToCharArray();
    string sUuId = "Id=";
    char[] cUuId = sUuId.ToCharArray();

    string[] split1 = null;
    string[] split2 = null;
    string[] split3 = null;
    string[] split4 = null;
    string[] split5 = null;

    CFDIService.Acuse oAcuse = new CFDIService.Acuse();
    CFDIService.ConsultaCFDIServiceClient oConsulta = new CFDIService.ConsultaCFDIServiceClient();

    if (sQr != string.Empty)
    {
      // Revisa si es versión QR 3.3
      if (sQr.Trim().StartsWith("https://verificacfdi.facturaelectronica.sat.gob.mx"))
      {
        split1 = sQr.Split(delimiter1, 7);

        if (split1[1] == "")
        {
          // Versión alternativa QR 3.3
          split2 = split1[2].Split(cUuid, 4);
          if (split2[2] != "")
          {
            // Sería Id=
            sCfdiData[3] = split2[2]; // UUID
          }
          else
          {
            // Sería id=
            sCfdiData[3] = split2[3]; // UUID
          }
          split3 = split1[3].Split(cEmisor, 4);
          split4 = split1[4].Split(cReceptor, 4);
          split5 = split1[5].Split(cTotal, 4);

          //sCfdiData[3] = split2[2]; // UUID
          sCfdiData[0] = split3[3]; // RFC Emisor
          sCfdiData[1] = split4[3]; // RFC Receptor
          sCfdiData[2] = split5[3]; // Total
        }
        else
        {
          // Versión original QR 3.3
          split2 = split1[1].Split(cUuid, 4);
          if (split2[1] == "")
          {
            split2 = split1[1].Split(cUuId, 4);
          }
          split3 = split1[2].Split(cEmisor, 4);
          split4 = split1[3].Split(cReceptor, 4);
          split5 = split1[4].Split(cTotal, 4);

          sCfdiData[3] = split2[2]; // UUID
          sCfdiData[0] = split3[3]; // RFC Emisor
          sCfdiData[1] = split4[3]; // RFC Receptor
          sCfdiData[2] = split5[3]; // Total
        }

        sCadena = "?re=" + sCfdiData[0] + "&rr=" + sCfdiData[1] + "&tt=" + sCfdiData[2] + "&id=" + sCfdiData[3] + "";
      }
      else
      {
        if (sQr.Trim().StartsWith("?re="))
        {
          split1 = sQr.Split(delimiter1, 5);
          split2 = split1[1].Split(cEmisor, 5);
          split3 = split1[2].Split(cReceptor, 5);
          split4 = split1[3].Split(cTotal, 5);
          split5 = split1[4].Split(cUuid, 4);

          sCfdiData[0] = split2[3]; // RFC Emisor
          sCfdiData[1] = split3[3]; // RFC Receptor
          sCfdiData[2] = split4[3]; // Total
          sCfdiData[3] = split5[3]; // UUID

          sCadena = "?re=" + sCfdiData[0] + "&rr=" + sCfdiData[1] + "&tt=" + sCfdiData[2] + "&id=" + sCfdiData[3] + "";
        }
        else
        {
          TotalTextBox.Text = "";
          RfcETextBox.Text = "";
          RfcRTextBox.Text = "";
          UuidTextBox.Text = "";
          QrTextBox.Text = "";
          UploadStatusLabel.Text = "Código QR incorrecto. Capture datos manualmente.";
          RfcETextBox.Focus();
          return;
        }
      }
    }
    else
    {
      sCadena = "?re=" + sData[1] + "&rr=" + sData[2] + "&tt=" + sData[0] + "&id=" + sData[3] + "";
      sCfdiData[0] = sData[1]; // RFC Emisor
      sCfdiData[1] = sData[2]; // RFC Receptor
      sCfdiData[2] = sData[0]; // Total
      sCfdiData[3] = sData[3]; // UUID
    }
    
    oAcuse = oConsulta.Consulta(sCadena);

    sCfdiData[4] = oAcuse.CodigoEstatus.ToString();
    sCfdiData[5] = oAcuse.Estado.ToString();

    TotalLabel.Text = sCfdiData[2];
    RfcEmisorLabel.Text = sCfdiData[0];
    RfcReceptorLabel.Text = sCfdiData[1];
    UuidLabel.Text = sCfdiData[3];
    CodigoEstatusLabel.Text = oAcuse.CodigoEstatus.ToString();
    sEstado = oAcuse.Estado.ToString();
    EstadoLabel.Text = oAcuse.Estado.ToString();

    sData_DBSave(sCfdiData);
  }

  protected void sData_DBSave(string[] datos)
  {
    DateTime temp_proceso = new DateTime();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    using (MySqlConnection connection = new MySqlConnection(constr))
    {
      using (MySqlCommand command = new MySqlCommand())
      {
        try
        {

          command.Connection = connection;
          command.CommandText = "INSERT INTO cfdi_facturas (rfc_e,rfc_r,uuid,total,estatus,estado,fecha,identificador) VALUES (?Rfc_e,?Rfc_r,?Uuid,?Total,?Estatus,?Estado,?Fecha,?Identi);";


          MySqlParameter RfcEParameter = new MySqlParameter("?Rfc_e", MySqlDbType.VarChar, 15);
          MySqlParameter RfcRParameter = new MySqlParameter("?Rfc_r", MySqlDbType.VarChar, 15);
          MySqlParameter UuidParameter = new MySqlParameter("?Uuid", MySqlDbType.VarChar, 50);
          MySqlParameter TotalParameter = new MySqlParameter("?Total", MySqlDbType.VarChar, 40);
          MySqlParameter EstatusParameter = new MySqlParameter("?Estatus", MySqlDbType.VarChar, 50);
          MySqlParameter EstadoParameter = new MySqlParameter("?Estado", MySqlDbType.VarChar, 50);
          MySqlParameter FechaParameter = new MySqlParameter("?Fecha", MySqlDbType.DateTime);
          MySqlParameter IdentificadorParameter = new MySqlParameter("?Identi", MySqlDbType.VarChar, 50);

          RfcEParameter.Value = datos[0];
          RfcRParameter.Value = datos[1];
          UuidParameter.Value = datos[3];
          TotalParameter.Value = datos[2];
          EstatusParameter.Value = datos[4];
          EstadoParameter.Value = datos[5];
          temp_proceso = DateTime.Now;
          FechaParameter.Value = temp_proceso.ToString(format: "yyyy-MM-dd HH:mm:ss");
          IdentificadorParameter.Value = IdentiTextBox.Text;

          command.Parameters.Add(RfcEParameter);
          command.Parameters.Add(RfcRParameter);
          command.Parameters.Add(UuidParameter);
          command.Parameters.Add(TotalParameter);
          command.Parameters.Add(EstatusParameter);
          command.Parameters.Add(EstadoParameter);
          command.Parameters.Add(FechaParameter);
          command.Parameters.Add(IdentificadorParameter);

          connection.Open();

          command.ExecuteNonQuery();

          connection.Close();
          UploadStatusLabel.Text = "Datos cargados en la base de datos correctamente";
          QrTextBox.Focus();
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
    TotalTextBox.Text = "";
    RfcETextBox.Text = "";
    RfcRTextBox.Text = "";
    UuidTextBox.Text = "";
    QrTextBox.Text = "";
    UploadStatusLabel.Text = string.Empty;
  }

  protected void BorraIdenti_Click(object sender, EventArgs e)
  {
    IdentiTextBox.Text = string.Empty;
  }
}