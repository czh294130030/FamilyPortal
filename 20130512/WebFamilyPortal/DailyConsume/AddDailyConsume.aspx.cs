using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFamilyPortal.DailyConsume
{
    public partial class AddDailyConsume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Consumes"] = new List<FamilyPortal.Common.NewConsume>();
                if (Request.QueryString["DailyID"] != null)//Edit daily consume.
                {
                    int dailyID = Int32.Parse(Request.QueryString["DailyID"].ToString());
                    GetDailyConsume(dailyID);
                }
                BindConsumeType();
            }
            else
            {
                ShowConsumes();
            }
        }
        #region Get daily consume.
        private void GetDailyConsume(int dailyID)
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetDailyConsumeByIDCompleted += new EventHandler<FamilyServiceReference.GetDailyConsumeByIDCompletedEventArgs>(client_GetDailyConsumeByIDCompleted);
            client.GetDailyConsumeByIDAsync(dailyID);
        }

        void client_GetDailyConsumeByIDCompleted(object sender, FamilyServiceReference.GetDailyConsumeByIDCompletedEventArgs e)
        {
            FamilyServiceReference.DailyConsume item = e.Result;
            DateTextBox.Text = item.date.ToString().Split(' ')[0];
            DateHiddenField.Value = item.date.ToString().Split(' ')[0];
            DailyAmountLabel.Text = item.amount.ToString();
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetConsumeByDailyIDCompleted += new EventHandler<FamilyServiceReference.GetConsumeByDailyIDCompletedEventArgs>(client_GetConsumeByDailyIDCompleted);
            client.GetConsumeByDailyIDAsync(item.dailyID);
        }

        void client_GetConsumeByDailyIDCompleted(object sender, FamilyServiceReference.GetConsumeByDailyIDCompletedEventArgs e)
        {
            List<FamilyServiceReference.Consume> items = e.Result;
            List<FamilyPortal.Common.NewConsume> newItems = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            if (items.Count() > 0)
            {
                foreach (FamilyServiceReference.Consume item in items)
                {
                    FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                    FamilyServiceReference.ConsumeType type = client.GetConsumeTypeById(item.typeID);
                    newItems.Add(new FamilyPortal.Common.NewConsume()
                    {
                        Amount = item.amount,
                        ConsumeID = item.consumeID,
                        DailyID = item.dailyID,
                        Description = item.description,
                        TypeDesc =type.description,
                        TypeID = item.typeID
                    });
                }
                ShowConsumes();
            }
        }
        #endregion
        /// <summary>
        /// Binding type of consume.
        /// </summary>
        private void BindConsumeType()
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetAllConsumeTypeCompleted += new EventHandler<FamilyServiceReference.GetAllConsumeTypeCompletedEventArgs>(client_GetAllConsumeTypeCompleted);
            client.GetAllConsumeTypeAsync();
        }
        void client_GetAllConsumeTypeCompleted(object sender, FamilyServiceReference.GetAllConsumeTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                TypeDropDownList.DataSource = e.Result;
                TypeDropDownList.DataTextField = "description";
                TypeDropDownList.DataValueField = "typeID";
                TypeDropDownList.DataBind();
            }
        }
        /// <summary>
        /// Save(Add,Edit) consume.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveConsumeLinkButton_Click(object sender, EventArgs e)
        {
            int typeID = Int32.Parse(TypeDropDownList.SelectedItem.Value);
            string type = TypeDropDownList.SelectedItem.Text;
            decimal amount = decimal.Parse(AmountTextBox.Text);
            string description = DecriptionTextBox.Text;
            List<FamilyPortal.Common.NewConsume> items = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            if (TypeHiddenField.Value=="")//Add consume.
            {
                items.Add(new FamilyPortal.Common.NewConsume
                {
                    TypeID = typeID,
                    TypeDesc = type,
                    Amount = amount,
                    Description = description
                });
            }
            else//Edit consume.
            {
                FamilyPortal.Common.NewConsume item = items.SingleOrDefault(u => u.TypeDesc == TypeHiddenField.Value);
                items.Remove(item);
                items.Add(new FamilyPortal.Common.NewConsume
                {
                    TypeID = typeID,
                    TypeDesc = type,
                    Amount = amount,
                    Description = description
                });
                TypeHiddenField.Value = "";
            }
            ShowConsumes();
            AmountTextBox.Text = string.Empty;
            DecriptionTextBox.Text = string.Empty;
            DailyAmountLabel.Text = GetDailyAmount(items).ToString();
        }
        private decimal? GetDailyAmount(List<FamilyPortal.Common.NewConsume> items)
        {
            decimal? dailyAmount = 0;
            if (items.Count > 0)
            {
                foreach (FamilyPortal.Common.NewConsume item in items)
                {
                    dailyAmount += item.Amount;
                }
            }
            return dailyAmount;
        }
        /// <summary>
        /// show all consume.
        /// </summary>
        private void ShowConsumes()
        {
            ConsumePanel.Controls.Clear();
            List<FamilyPortal.Common.NewConsume> oldItems = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            IOrderedEnumerable<FamilyPortal.Common.NewConsume> items = oldItems.OrderBy(u => u.TypeID);
            if (items.Count()>0)
            {
                foreach (FamilyPortal.Common.NewConsume item in items)
                {
                    WebFamilyPortal.Controls.ConsumeControl consumeControl = (WebFamilyPortal.Controls.ConsumeControl)Page.LoadControl("~/Controls/ConsumeControl.ascx");
                    consumeControl.ID = item.TypeID.ToString();
                    consumeControl.editEvent += new WebFamilyPortal.Controls.ConsumeControl.EditDelegate(consumeControl_editEvent);
                    consumeControl.deleteEvent += new WebFamilyPortal.Controls.ConsumeControl.DeleteDelegate(consumeControl_deleteEvent);
                    consumeControl.SetConsume(item);
                    ConsumePanel.Controls.Add(consumeControl);
                }
            }
        }
        /// <summary>
        /// Delete temporary consume information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void consumeControl_deleteEvent(object sender, EventArgs e)
        {
            List<FamilyPortal.Common.NewConsume> items = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            FamilyPortal.Common.NewConsume item = items.SingleOrDefault(u => u.TypeID==Int32.Parse(sender.ToString()));
            items.Remove(item);
            DailyAmountLabel.Text = GetDailyAmount(items).ToString();
            ShowConsumes();
        }
        /// <summary>
        /// Edit temporary consume information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void consumeControl_editEvent(object sender, EventArgs e)
        {
            List<FamilyPortal.Common.NewConsume> items = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            FamilyPortal.Common.NewConsume item = items.SingleOrDefault(u => u.TypeID == Int32.Parse(sender.ToString()));
            TypeDropDownList.SelectedValue = item.TypeID.ToString();
            AmountTextBox.Text = item.Amount.ToString();
            DecriptionTextBox.Text = item.Description.ToString();
            TypeHiddenField.Value = item.TypeDesc;
        }
        /// <summary>
        /// Save daily consume.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveDailyConsumeButton_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(DateTextBox.Text.Trim());
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetTheDailyConsumeByDateCompleted += new EventHandler<FamilyServiceReference.GetTheDailyConsumeByDateCompletedEventArgs>(client_GetTheDailyConsumeByDateCompleted);
            client.GetTheDailyConsumeByDateAsync(date);
        }
        void client_GetTheDailyConsumeByDateCompleted(object sender, FamilyServiceReference.GetTheDailyConsumeByDateCompletedEventArgs e)
        {
            if (Request.QueryString["DailyID"] == null)//Add daily consume
            {
                if (e.Result == null)//There isn't existed daily consume on this day.
                {
                    AddOneDailyConsume();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "daily_consume_exist.", "alert('There is existed daily consume on this day.');", true);
                }
            }
            else//Edit daily consume
            {
                int dailyID = Int32.Parse(Request.QueryString["DailyID"].ToString());
                if (e.Result == null || (e.Result != null && e.Result.date == DateTime.Parse(DateHiddenField.Value)))//There isn't existed daily consume on this day.
                {
                    UpdateDailyConsume(dailyID);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "daily_consume_exist.", "alert('There is existed daily consume on this day.');", true);
                }
            }
        }
        #region update daily consume.
        private void UpdateDailyConsume(int dailyID)
        {
            FamilyServiceReference.DailyConsume item = GetUserInputDailyConsume(dailyID);
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.UpdateDailyConsumeCompleted += new EventHandler<FamilyServiceReference.UpdateDailyConsumeCompletedEventArgs>(client_UpdateDailyConsumeCompleted);
            client.UpdateDailyConsumeAsync(item);
        }

        void client_UpdateDailyConsumeCompleted(object sender, FamilyServiceReference.UpdateDailyConsumeCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.DeleteConsumeByDailyIDCompleted += new EventHandler<FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs>(client_DeleteConsumeByDailyIDCompleted);
                client.DeleteConsumeByDailyIDAsync(dailyID);
            }
        }

        void client_DeleteConsumeByDailyIDCompleted(object sender, FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                List<FamilyServiceReference.Consume> items = GetConsumes(dailyID);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.BatchAddConsumesCompleted += new EventHandler<FamilyServiceReference.BatchAddConsumesCompletedEventArgs>(client_BatchAddConsumesCompleted);
                client.BatchAddConsumesAsync(items);
            }
        }
        /// <summary>
        /// According to user's input to get daily consume.
        /// </summary>
        /// <returns></returns>
        private FamilyServiceReference.DailyConsume GetUserInputDailyConsume(int dailyID)
        {
            FamilyServiceReference.DailyConsume item = new FamilyServiceReference.DailyConsume();
            item.amount = decimal.Parse(DailyAmountLabel.Text.Trim());
            item.date = DateTime.Parse(DateTextBox.Text.Trim());
            item.dailyID = dailyID;
            return item;
        }
        #endregion

        #region Add daily consume
        private void AddOneDailyConsume()
        {
            FamilyServiceReference.DailyConsume item = new FamilyServiceReference.DailyConsume();
            item.amount = decimal.Parse(DailyAmountLabel.Text.Trim());
            item.date = DateTime.Parse(DateTextBox.Text.Trim());
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.AddOneDailyConsumeCompleted += new EventHandler<FamilyServiceReference.AddOneDailyConsumeCompletedEventArgs>(client_AddOneDailyConsumeCompleted);
            client.AddOneDailyConsumeAsync(item);
        }

        void client_AddOneDailyConsumeCompleted(object sender, FamilyServiceReference.AddOneDailyConsumeCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                List<FamilyServiceReference.Consume> items = GetConsumes(dailyID);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.BatchAddConsumesCompleted += new EventHandler<FamilyServiceReference.BatchAddConsumesCompletedEventArgs>(client_BatchAddConsumesCompleted);
                client.BatchAddConsumesAsync(items);
            }
        }

        void client_BatchAddConsumesCompleted(object sender, FamilyServiceReference.BatchAddConsumesCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "save_successfully", "saveSuccessfully();", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "fail_to_save", "alert('Failed to save.');", true);
            }
        }
        #endregion

        private List<FamilyServiceReference.Consume> GetConsumes(int dailyID)
        {
            List<FamilyServiceReference.Consume> items = new List<FamilyServiceReference.Consume>();
            List<FamilyPortal.Common.NewConsume> newItems = (List<FamilyPortal.Common.NewConsume>)ViewState["Consumes"];
            if (newItems.Count() > 0)
            {
                foreach (FamilyPortal.Common.NewConsume newItem in newItems)
                {
                    items.Add(new FamilyServiceReference.Consume
                    {
                        amount=newItem.Amount,
                        typeID=newItem.TypeID,
                        description=newItem.Description,
                        dailyID=dailyID
                    });
                }
            }
            return items;
        }
    }
}