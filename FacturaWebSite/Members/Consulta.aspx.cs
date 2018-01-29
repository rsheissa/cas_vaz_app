using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Consulta : System.Web.UI.Page
{
  const int pageSize = 50;

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      Button2.Enabled = false;
      Button3.Enabled = false;
      DeleteTextBox.Enabled = false;
      EstatusLabel.Text = string.Empty;
      ActualizaFecha.Visible = false;
      FiltroManual.Visible = false;
      ListaFiltro.Visible = false;
      BorraRegistro.Visible = false;
    }
  }

  protected void Load_DB(object sender, EventArgs e)
  {
    BindPageNos();
    BindData(int.Parse(ddlPageNos.SelectedValue));
    BindDataFilter();
    Button1.Enabled = false;
    Button2.Enabled = true;
    Button3.Enabled = true;
    //DeleteTextBox.Enabled = true;
    ActualizaFecha.Visible = true;
      FiltroManual.Visible = true;
      ListaFiltro.Visible = true;
      BorraRegistro.Visible = true;
  }

  private void BindPageNos()
  {
    int total = GetTotalData();

    double pageCount = Math.Ceiling(total / (double)pageSize);
    
    if (pageCount == 0)
    {
      pageCount = 1;
      EstatusLabel.Text = "La base de datos está vacía";
    }

    for(int i=1; i <= pageCount; i++)
    {
      ddlPageNos.Items.Add(new ListItem(i.ToString()));
    }
  }

  private int GetTotalData()
  {
    int total = 0;

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection cn = new MySqlConnection(constr))
    {
      //MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM cfdi_facturas", cn);
      MySqlCommand cmd = new MySqlCommand("SELECT COUNT(rfc_e) FROM cfdi_facturas", cn);
      cn.Open();

      object retTotal = cmd.ExecuteScalar();

      if(retTotal != null)
      {
        total = int.Parse(retTotal.ToString());
      }
    }

    return total;
  }

  private void BindData(int page)
  {
    DataTable dt = new DataTable();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection cn = new MySqlConnection(constr))
    {
      // Limit pageSize
      // offset (page-1)*pageSize
      string query = "SELECT doc_id,rfc_e,nom_e,dom_e,rfc_r,nom_r,dom_r,uuid,total,estatus,estado,fecha,cantidad,unidad,descripcion,identificador,valorunitario,importe,subtotal,tasa_iva,importe_iva,fecha_emision,fecha_timbrado,arch_name FROM cfdi_facturas limit " + pageSize + " offset " + (page - 1) * pageSize + ";";
      MySqlDataAdapter adp = new MySqlDataAdapter(query, cn);

      adp.Fill(dt);
    }

    if(dt.Rows.Count > 0)
    {
      GridView1.DataSource = dt;
      GridView1.DataBind();
    }
  }

  protected void ddlPageNos_SelectedIndexChanged(object sender, EventArgs e)
  {
    EstatusLabel.Text = "";
    BindData(int.Parse(ddlPageNos.SelectedValue));
  }

  protected void Button2_Click(object sender, EventArgs e)
  {
    if (DeleteTextBox.Text == "")
    {
      EstatusLabel.Text = "Error: Se debe introducir un número";
    }
    else
    {
      string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

      using (MySqlConnection connection = new MySqlConnection(constr))
      {
        using (MySqlCommand command = new MySqlCommand())
        {
          try
          {
            command.Connection = connection;
            command.CommandText = "DELETE FROM cfdi_facturas WHERE doc_id=" + DeleteTextBox.Text + ";";

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            EstatusLabel.Text = "El campo " + DeleteTextBox.Text + " ha sido borrado";
            ddlPageNos.Items.Clear();
            BindPageNos();
            BindData(1);
            DeleteTextBox.Text = string.Empty;
          }
          catch (MySql.Data.MySqlClient.MySqlException ex)
          {
            EstatusLabel.Text = "Error " + ex.Number + " ha ocurrido: " + ex.Message;
          }
        }
      }
    }
  }

  protected void Verificar_Fecha(object sender, EventArgs e)
  {
    string[] parameters = new string[5];
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    using (MySqlConnection connection = new MySqlConnection(constr))
    {
      using (MySqlCommand command = new MySqlCommand())
      {
        try
        {
          command.Connection = connection;
          command.CommandText = "SELECT doc_id,rfc_e,rfc_r,uuid,total FROM cfdi_facturas;";

          connection.Open();
          MySqlDataReader reader = command.ExecuteReader();

          while (reader.Read())
          {
            parameters[0] = reader.GetString("doc_id");
            parameters[1] = reader.GetString("rfc_e");
            parameters[2] = reader.GetString("rfc_r");
            parameters[3] = reader.GetString("uuid");
            parameters[4] = reader.GetString("total");
            Valida_SAT(parameters);
          }

          BindData(1);

          if (connection.State == ConnectionState.Open)
          {
            connection.Close();
          }
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
          EstatusLabel.Text = "Error " + ex.Number + " ha ocurrido " + ex.Message;
        }
      }
    }
  }

  protected void Valida_SAT(string[] sData)
  {
    string[] sCfdiData;
    sCfdiData = new string[9];
    string sCadena = string.Empty;

    CFDIService.Acuse oAcuse = new CFDIService.Acuse();
    CFDIService.ConsultaCFDIServiceClient oConsulta = new CFDIService.ConsultaCFDIServiceClient();
    sCadena = "?re=" + sData[1] + "&rr=" + sData[2] + "&tt=" + sData[4] + "&id=" + sData[3] + "";
    oAcuse = oConsulta.Consulta(sCadena);

    sCfdiData[0] = sData[0]; // doc_id
    sCfdiData[1] = sData[1]; // rfc_e
    sCfdiData[2] = sData[2]; // rfc_r
    sCfdiData[3] = sData[3]; // uuid
    sCfdiData[4] = sData[4]; // total
    sCfdiData[5] = oAcuse.CodigoEstatus.ToString();
    sCfdiData[6] = oAcuse.Estado.ToString();

    UpdateDb(sCfdiData);
  }

  protected void UpdateDb(string[] datos)
  {
    DateTime temp_proceso = new DateTime();

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    MySqlConnection connection = new MySqlConnection(constr);
    MySqlCommand command = new MySqlCommand();

    command.Connection = connection;
    temp_proceso = DateTime.Now;
    command.CommandText = "UPDATE cfdi_facturas SET estatus='" + datos[5] + "',estado='" + datos[6] + "',fecha='" + temp_proceso.ToString(format: "yyyy-MM-dd HH:mm:ss") + "' WHERE doc_id=" + datos[0] + ";";
    connection.Open();

    if (connection.State == ConnectionState.Open)
    {
      command.ExecuteNonQuery();
      connection.Close();
    }
  }

  protected void Filtrar_Identificador(object sender, EventArgs e)
  {
    ddlPageNos.Items.Clear();
    LoadDB_Filtrado();
  }

  protected void LoadDB_Filtrado()
  {
    if (IdentiTextBox.Text == "")
    {
      ddlPageNos.Items.Clear();
      BindPageNos();
      BindData(int.Parse(ddlPageNos.SelectedValue));
    }
    else
    {
      BindPageNos_Filtrado();
      BindData_Filtrado(1, IdentiTextBox.Text);
    }
  }

  private void BindPageNos_Filtrado()
  {
    int total = GetFilteredData(IdentiTextBox.Text);

    double pageCount = Math.Ceiling(total / (double)pageSize);

    if (pageCount == 0)
    {
      pageCount = 1;
      EstatusLabel.Text = "Identificador incorrecto o no existe";
    }

    for (int i = 1; i <= pageCount; i++)
    {
      ddlPageNos.Items.Add(new ListItem(i.ToString()));
    }
  }

  private int GetFilteredData(string sIdentifier)
  {
    int total = 0;
    string instruction = "SELECT COUNT(*) FROM cfdi_facturas WHERE identificador=" + "'" + sIdentifier + "';";

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection cn = new MySqlConnection(constr))
    {
      MySqlCommand cmd = new MySqlCommand(instruction, cn);
      cn.Open();

      object retTotal = cmd.ExecuteScalar();

      if (retTotal != null)
      {
        total = int.Parse(retTotal.ToString());
      }
    }

    return total;
  }

  private void BindData_Filtrado(int page, string selector)
  {
    DataTable dt = new DataTable();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection cn = new MySqlConnection(constr))
    {
      // Limit pageSize
      // offset (page-1)*pageSize
      string query = "SELECT doc_id,rfc_e,nom_e,dom_e,rfc_r,nom_r,dom_r,uuid,total,estatus,estado,fecha,cantidad,unidad,descripcion,identificador,valorunitario,importe,subtotal,tasa_iva,importe_iva,fecha_emision,fecha_timbrado,arch_name FROM cfdi_facturas WHERE identificador='" + selector + "'" + " LIMIT " + pageSize + " OFFSET " + (page - 1) * pageSize + ";";
      MySqlDataAdapter adp = new MySqlDataAdapter(query, cn);

      adp.Fill(dt);
    }

    if (dt.Rows.Count > 0)
    {
      GridView1.DataSource = dt;
      GridView1.DataBind();
    }
  }

  private void BindDataFilter()
  {
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection cn = new MySqlConnection(constr))
    {
      // Se agrega un primer campo vacío a la lista
      ddlFilter.Items.Add("");

      // Abre conexión con la base de datos
      cn.Open();

      // Se seleccionan los valores del campo "identificador"
      // que no sean vacíos
      string query = "SELECT DISTINCT identificador FROM cfdi_facturas WHERE identificador != '' ;";

      using (MySqlCommand cmd = new MySqlCommand(query, cn))
      {
        using (var reader = cmd.ExecuteReader())
        {
          // Iterate through the rows and add it to the dropdownlist
          while (reader.Read())
          {
            ddlFilter.Items.Add(reader.GetString("identificador"));
          }
        }
      }
    }
  }

  protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (ddlFilter.Text == "")
    {
      EstatusLabel.Text = "El valor seleccionado es: VACIO";
    }
    else
    {
      EstatusLabel.Text = "El valor seleccionado es: " + ddlFilter.Text;
    }
  }
}