using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.Security;
using MySql.Data.MySqlClient;
using System.Data;
using HashLibrary;

public partial class _Default : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
  }

  protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
  {
    bool authenticated = this.ValidateCredentials(LoginControl.UserName, LoginControl.Password);

    if (authenticated)
    {
      FormsAuthentication.RedirectFromLoginPage(LoginControl.UserName, LoginControl.RememberMeSet);
    }
  }

  public bool IsAlphanumeric(string text)
  {
    return Regex.IsMatch(text, "^[a-zA-Z0-9]+$");
  }

  private bool ValidateCredentials(string userName, string password)
  {
    bool returnValue = false;
    //bool bHasRows = false;
    string str;
    string strcon = "Server=localhost;Database=cfdi_users;Uid=root;Pwd=112233445566;";
    MySqlConnection connection = new MySqlConnection(strcon);
    MySqlCommand com;
    object obj;

    if (this.IsAlphanumeric(userName) && userName.Length <= 50 && password.Length <= 50)
    {

      try
      {
        connection.Open();
        str = "SELECT count(*) FROM users WHERE username=@userName and password=@password";
        com = new MySqlCommand(str, connection);
        com.CommandType = CommandType.Text;
        com.Parameters.AddWithValue("@UserName", userName.Trim());
        com.Parameters.AddWithValue("@Password", Hasher.HashString(password.Trim()));
        obj = com.ExecuteScalar();
        if (Convert.ToInt32(obj) != 0)
        {
          Session["uName"] = userName.Trim();
          Session["pAss"] = Hasher.HashString(password.Trim());
          returnValue = true;
        }
        else
        {
          ErrorStatus.Text = "Invalid user name and password";
        }
        connection.Close();
      }
      catch (System.Exception ex)
      {
        ErrorStatus.Text = ex.Message;
        connection.Close();
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

    return returnValue;
  }
}