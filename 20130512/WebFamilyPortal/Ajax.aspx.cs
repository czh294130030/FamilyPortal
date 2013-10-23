using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFamilyPortal
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                string action = Request.QueryString["action"].ToString();
                switch (action)
                { 
                    case "login":
                        string account = string.Empty;
                        string password = string.Empty;
                        if (Request.QueryString["account"] != null) { account = Request.QueryString["account"].ToString(); }
                        if (Request.QueryString["password"] != null) { password = Request.QueryString["password"].ToString(); }
                        if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
                        {
                            Response.Write(Login(account, password));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool Login(string account, string password)
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            FamilyServiceReference.UserInfo item=client.GetUserInfoByAccountAndPassword(account, password);
            if (item != null)
            {
                HttpContext.Current.Session["userName"] = item.userName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}