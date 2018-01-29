using System;
using MySql.Data.MySqlClient;
using System.IO;
using System.Configuration;

public partial class Admin : System.Web.UI.Page
{

  protected void Page_Load(object sender, EventArgs e)
  {
   
  }

  protected void GuardaButton_Click(object sender, EventArgs e)
  {
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    MemoryStream ms = new MemoryStream();

    using (MySqlConnection conn = new MySqlConnection(constr))
    {
      MySqlCommand cmd = new MySqlCommand();
      MySqlBackup mb = new MySqlBackup(cmd);
      cmd.Connection = conn;
      conn.Open();
      mb.ExportToMemoryStream(ms);
    }

    Response.ContentType = "text/plain";
    Response.AppendHeader("Content-Disposition", "attachments; filename=*.sql");
    Response.BinaryWrite(ms.ToArray());
    Response.End();
  }

  protected void SubeButton_Click(object sender, EventArgs e)
  {
    // Este código carga un archivo de base de datos MySQL al servidor web, de ahí lo sube al servidor MySQL

    // Primero revisa si el control tiene un archivo
    if (FileUpload1.HasFile)
    {
      try
      {
        string sExtension = System.IO.Path.GetExtension(FileUpload1.FileName);
        if (sExtension.ToLower() == ".sql")
        {
          // Usa Server.MapPath para dar lo ubicación en el servidor donde el archivo será descargado
          FileUpload1.SaveAs(Server.MapPath("~/Databases/") + FileUpload1.FileName); // Guarda el archivo en el servidor
          UploadDB(FileUpload1.FileName);
        }
        else
        {
          InfoLabel.Text = "El archivo no es válido MySQL";
        }
      }
      catch (Exception ex)
      {
        InfoLabel.Text = "Upload status: No se puede subir el archivo. Error: " + ex.Message;
      }
    }
  }

  protected void UploadDB(string sFileName)
  {
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    using (MySqlConnection conn = new MySqlConnection(constr))
    {
      using (MySqlCommand cmd = new MySqlCommand())
      {
        using (MySqlBackup mb = new MySqlBackup(cmd))
        {
          cmd.Connection = conn;
          conn.Open();
          mb.ImportFromFile(Server.MapPath("~/Databases/") + sFileName); // Lee el archivo en el servidor
          conn.Close();
        }
      }
    }
  }

  protected void BorraDbButton_Click(object sender, EventArgs e)
  {
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    using (MySqlConnection connection = new MySqlConnection(constr))
    {
      using (MySqlCommand command = new MySqlCommand())
      {
        try
        {
          command.Connection = connection;
          command.CommandText = "DELETE FROM cfdi_facturas;";

          connection.Open();

          command.ExecuteNonQuery();

          command.CommandText = "ALTER TABLE cfdi_facturas AUTO_INCREMENT=1;";

          command.ExecuteNonQuery();

          connection.Close();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
          InfoLabel.Text = "Error " + ex.Number + " ha ocurrido: " + ex.Message;
        }
      }
    }
  }
}