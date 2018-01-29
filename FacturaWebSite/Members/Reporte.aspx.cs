using System;
using System.Data;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

public partial class Reporte : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {

  }

  protected void Load_DB(object sender, EventArgs e)
  {
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    using (MySqlConnection connection = new MySqlConnection(constr))
    {
      using (MySqlCommand command = new MySqlCommand("SELECT doc_id,rfc_e,nom_e,dom_e,rfc_r,nom_r,dom_r,uuid,total,estatus,estado,fecha,cantidad,unidad,descripcion,identificador,valorunitario,importe,subtotal,tasa_iva,importe_iva,fecha_emision,fecha_timbrado,arch_name FROM cfdi_facturas;"))
      {
        using (MySqlDataAdapter sda = new MySqlDataAdapter())
        {
          command.Connection = connection;
          sda.SelectCommand = command;
          using (DataTable dt = new DataTable())
          {
            sda.Fill(dt);
            ExportToExcel(dt);
          }
        }
      }
    }
  }

  protected void ExportToExcel(DataTable DataTable)
  {
    string ExcelFilePath = @"c:\temp\uploads\DB\";
    string ExcelName = string.Empty;

    if (NomArchText.Text == "")
    {
      ExcelName = "test.xlsx";
      ExcelFilePath = ExcelFilePath + ExcelName;
    }
    else
    {
      ExcelName = NomArchText.Text + ".xlsx";
      ExcelFilePath = ExcelFilePath + ExcelName;
    }

    try
    {
      int ColumnsCount;

      if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
      {
        //throw new Exception("ExportToExcel: Null or empty input table!\n");
        StatusLineLabel.Text = "ExportToExcel: Tabla de datos vacía!";
      }

      // load excel, and create a new workbook
      Excel.Application Excel = new Excel.Application();
      Excel.Workbooks.Add();

      // single worksheet
      Excel._Worksheet Worksheet = (Excel._Worksheet)Excel.ActiveSheet;

      object[] Header = new object[ColumnsCount=DataTable.Columns.Count];

      // column headings               
      for (int i = 0; i < ColumnsCount; i++)
        Header[i] = DataTable.Columns[i].ColumnName;

      Excel.Range HeaderRange = Worksheet.get_Range((Excel.Range)(Worksheet.Cells[1, 1]), (Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
      HeaderRange.Value = Header;
      HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
      HeaderRange.Font.Bold = true;

      // DataCells
      int RowsCount = DataTable.Rows.Count;
      object[,] Cells = new object[RowsCount, ColumnsCount];

      for (int j = 0; j < RowsCount; j++)
        for (int i = 0; i < ColumnsCount; i++)
          Cells[j, i] = DataTable.Rows[j][i];

      Worksheet.get_Range(Cell1: (Excel.Range)(Worksheet.Cells[2, 1]), Cell2: (Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;

      // check fielpath
      if (ExcelFilePath != null && ExcelFilePath != "")
      {
        try
        {
          Worksheet.SaveAs(ExcelFilePath);
          Excel.Quit();
          StatusLineLabel.Text = "El archivo " + ExcelName + " ha sido guardado";
          NomArchText.Text = string.Empty;
        }
        catch (Exception ex)
        {
          //throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n" + ex.Message);
          StatusLineLabel.Text = "ExportToExcel: El archivo Excel no se pudo guardar! Verifique la ruta." + ex.Message;
        }
      }
      else    // no filepath is given
      {
        Excel.Visible = true;
      }
    }
    catch (Exception ex)
    {
      //throw new Exception("ExportToExcel: \n" + ex.Message);
      StatusLineLabel.Text = "ExportToExcel: " + ex.Message;
    }
  }
}