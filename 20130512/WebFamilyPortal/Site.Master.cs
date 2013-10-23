using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FamilyPortal.Common;

namespace WebFamilyPortal
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["userName"] != null)
                {
                    AccountLabel.Text = HttpContext.Current.Session["userName"].ToString();
                    LoginLinkButton.Text = Field.Login.Logout.ToString();
                }
                else
                {
                    AccountLabel.Text = "Guest";
                    LoginLinkButton.Text = Field.Login.Login.ToString();
                }
            }
        }

        protected void LoginLinkButton_Click(object sender, EventArgs e)
        {
            if (LoginLinkButton.Text.Equals(Field.Login.Login.ToString()))//redirect to login page.
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else//logout and redirect to default page.
            {
                HttpContext.Current.Session["userName"] = null;
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}
