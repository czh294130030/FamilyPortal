using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFamilyPortal.Controls
{
    public partial class ConsumeControl : System.Web.UI.UserControl
    {
        public delegate void EditDelegate(Object sender, EventArgs e);
        public delegate void DeleteDelegate(Object sender, EventArgs e);
        public event EditDelegate editEvent;
        public event DeleteDelegate deleteEvent;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void SetConsume(FamilyPortal.Common.NewConsume item)
        {
            TypeLabel.Text = item.TypeDesc;
            AmountLabel.Text = item.Amount.ToString();
            DescriptionLabel.Text = item.Description.Replace("\r\n", "<br />");
        }

        protected void EditLinkButton_Click(object sender, EventArgs e)
        {
            if (editEvent != null)
            {
                editEvent(this.ID, e);
            }
        }

        protected void DeleteLinkButton_Click(object sender, EventArgs e)
        {
            if (deleteEvent != null)
            {
                deleteEvent(this.ID, e);
            }
        }
    }
}