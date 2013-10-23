using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FamilyPortal.Common;
using System.Web.UI.DataVisualization.Charting;

namespace WebFamilyPortal.DailyConsume
{
    public partial class DailyConsume : System.Web.UI.Page
    {
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDailyConsume();//Bind daily consume information when webpage is loaded the first time(this week).
            }
        }
        /// <summary>
        /// Bind daily consume information after page index changing.
        /// </summary>
        protected void DailyConsumeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DailyConsumeGridView.PageIndex = e.NewPageIndex;
            BindDailyConsume();
        }
        /// <summary>
        /// Bind daily consume information after time period is changed.
        /// </summary>
        protected void TimePeriodDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDailyConsume();
        }
        /// <summary>
        /// Bind daily consume information.
        /// </summary>
        private void BindDailyConsume()
        {
            //Convert string enum
            Field.TimePeriod timePeriod = (Field.TimePeriod)Enum.Parse(typeof(Field.TimePeriod), TimePeriodDropDownList.SelectedItem.Value, true);
            Method.GetDates(timePeriod, ref startDate, ref endDate);
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetDailyConsumeCompleted += new EventHandler<FamilyServiceReference.GetDailyConsumeCompletedEventArgs>(client_GetDailyConsumeCompleted);
            client.GetDailyConsumeAsync(startDate, endDate);
        }
        void client_GetDailyConsumeCompleted(object sender, FamilyServiceReference.GetDailyConsumeCompletedEventArgs e)
        {
            if (e.Result.Count > 0)
            {
                //Get daily consume.
                List<NewDailyConsume> dailyConsumes = RemoveHourMinuteSecondOfDateTime(e.Result);
                DailyConsumeGridView.DataSource = dailyConsumes;
                DailyConsumeGridView.DataBind();
                //Get daily consume ids.
                List<int?> dailyConsumeIDs = GetDailyConsumeIDs(dailyConsumes);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetConsumeByDailyIDsCompleted += new EventHandler<FamilyServiceReference.GetConsumeByDailyIDsCompletedEventArgs>(client_GetConsumeByDailyIDsCompleted);
                client.GetConsumeByDailyIDsAsync(dailyConsumeIDs);
            }
            else//there is no data in selected period.
            {
                List<NewDailyConsume> dailyConsumes = new List<NewDailyConsume>();
                dailyConsumes.Add(new NewDailyConsume());
                DailyConsumeGridView.DataSource = dailyConsumes;
                DailyConsumeGridView.DataBind();
                int columnCount = 5;
                DailyConsumeGridView.Rows[0].Cells.Clear();
                DailyConsumeGridView.Rows[0].Cells.Add(new TableCell());
                DailyConsumeGridView.Rows[0].Cells[0].ColumnSpan = columnCount;
                DailyConsumeGridView.Rows[0].Cells[0].Text = "there is no data in selected period.";
                DailyConsumeGridView.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
        }

        void client_GetConsumeByDailyIDsCompleted(object sender, FamilyServiceReference.GetConsumeByDailyIDsCompletedEventArgs e)
        {
            if (e.Result.Count > 0)
            {
                List<KeyValue> consumes = GetConsumeStatistic(e.Result);
                BindColumnChart(consumes);
                BindLineChart(consumes);
                BindPieChart(consumes);
            }
        }
        #region Binding chart including column chart, line chart and pie chart.
        private void BindColumnChart(List<KeyValue> consumes)
        {
            decimal totalAmount = 0;
            Series series = ColumnChart.Series[0];
            foreach (KeyValue item in consumes)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.SetValueXY(item.Key, item.Value);
                series.Points.Add(dataPoint);
                totalAmount += decimal.Parse(item.Value.ToString());
            }
            ColumnChart.ChartAreas[0].AxisY.Title = "Monetary(￥)";
            ColumnChart.Titles[0].Text = string.Format("Consume Chart(total ￥{0})", totalAmount);
        }
        private void BindLineChart(List<KeyValue> consumes)
        {
            decimal totalAmount = 0;
            Series series = LineChart.Series[0];
            foreach (KeyValue item in consumes)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.SetValueXY(item.Key, item.Value);
                series.Points.Add(dataPoint);
                totalAmount += decimal.Parse(item.Value.ToString());
            }
            LineChart.ChartAreas[0].AxisY.Title = "Monetary(￥)";
            LineChart.Titles[0].Text = string.Format("Consume Chart(total ￥{0})", totalAmount);
        }
        private void BindPieChart(List<KeyValue> consumes)
        {
            decimal totalAmount = 0;
            Series series = PieChart.Series[0];
            foreach (KeyValue item in consumes)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.LegendText = item.Key.ToString();
                dataPoint.SetValueY(item.Value);
                series.Points.Add(dataPoint);
                totalAmount += decimal.Parse(item.Value.ToString());
            }
            PieChart.Titles[0].Text = string.Format("Consume Chart(total ￥{0})", totalAmount);
        }
        #endregion

        /// <summary>
        /// Get Consume statistic information.
        /// </summary>
        private List<KeyValue> GetConsumeStatistic(List<NewConsume> items)
        {
            var query = from a in items
                        orderby a.TypeID ascending
                        group a by a.TypeDesc into b
                        select new KeyValue
                        {
                            Key = b.Key,
                            Value = b.Sum(u => u.Amount)
                        };
            return query.ToList();
        }
        /// <summary>
        /// Get daily consume IDs.
        /// </summary>
        /// <returns></returns>
        private List<int?> GetDailyConsumeIDs(List<NewDailyConsume> dailyConsumes)
        {
            List<int?> dailyConsumeIDs = new List<int?>();
            for (int i = 0; i < dailyConsumes.Count; i++)
            {
                dailyConsumeIDs.Add(dailyConsumes[i].DailyID);
            }
            return dailyConsumeIDs;
        }
        /// <summary>
        /// Removing hour, minute and second of datetime displayed in datagrid.
        /// </summary>
        private List<NewDailyConsume> RemoveHourMinuteSecondOfDateTime(List<FamilyServiceReference.DailyConsume> list)
        {
            var query = from l in list
                        select new NewDailyConsume
                        {
                            DailyID = l.dailyID,
                            Amount = l.amount,
                            Date = Convert.ToDateTime(l.date).ToString("yyyy-MM-dd")
                        };
            return query.ToList();
        }
        #region delete daily consume
        protected void DailyConsumeGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string DailyID=e.CommandArgument.ToString();
            switch (e.CommandName)
            { 
                case "ViewDailyConsume":
                    Response.Redirect("~/DailyConsume/ViewDailyConsume.aspx?DailyID=" + DailyID);
                    break;
                case "EditDailyConsume":
                    Response.Redirect("~/DailyConsume/AddDailyConsume.aspx?DailyID=" + DailyID);
                    break;
                case "DeleteDailyConsume":
                    FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                    client.DeleteDailyConsumeByIDCompleted += new EventHandler<FamilyServiceReference.DeleteDailyConsumeByIDCompletedEventArgs>(client_DeleteDailyConsumeByIDCompleted);
                    client.DeleteDailyConsumeByIDAsync(Int32.Parse(DailyID));
                    break;
                default:
                    break;
            }
        }

        void client_DeleteDailyConsumeByIDCompleted(object sender, FamilyServiceReference.DeleteDailyConsumeByIDCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.DeleteConsumeByDailyIDCompleted += new EventHandler<FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs>(client_DeleteConsumeByDailyIDCompleted);
                client.DeleteConsumeByDailyIDAsync(dailyID);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "fail_to_delete", "alert('Failed to delete.');", true);
            }
        }

        void client_DeleteConsumeByDailyIDCompleted(object sender, FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "delete_successfully", "deleteSuccessfully();", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "fail_to_delete", "alert('Failed to delete.');", true);
            }
        }
        #endregion 
    }
}