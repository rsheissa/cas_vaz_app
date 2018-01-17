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
    bool authenticated = this.ValidCredentials(LoginControl.UserName, LoginControl.Password);

    if (authenticated)
    {
      
    }
  }
}