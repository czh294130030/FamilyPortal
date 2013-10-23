using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.Linq;
using System.Collections.Generic;
using FamilyPortal.Data;
using FamilyPortal.Common;
using System.Web;
using System.Configuration;

namespace FamilyPortal.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FamilyService
    {
        private string FamilyDBConnectionString = ConfigurationManager.ConnectionStrings["FamilyDBConnectionString"].ToString();
        private bool isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"].ToString());
        [OperationContract]
        public string DoWork()
        {
            return "hello world!";
        }
        #region Session
        /// <summary>
        /// Set Session, the key is userID.
        /// </summary>
        /// <param name="userID"></param>
        [OperationContract]
        public void SetSession(object userID)
        {
            HttpContext.Current.Session["userID"] = userID;
        }
        /// <summary>
        /// Get Session, return to userID.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public object GetSession()
        {
            return HttpContext.Current.Session["userID"];
        }
        #endregion

        #region UserInfo
        /// <summary>
        /// According to userID to get user's information.
        /// </summary>
        /// <param name="ID"></param>
        [OperationContract]
        public UserInfo GetUserInfoByID(int userID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    UserInfo item=context.UserInfo.SingleOrDefault(u => u.userID == userID);
                    return item;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetUserInfoByID");
                    return null;
                }
            }
        }
        /// <summary>
        /// According to account and password to get user's information.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        public UserInfo GetUserInfoByAccountAndPassword(string account, string password)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    UserInfo item = context.UserInfo.SingleOrDefault(u => u.account == account & u.password == password);
                    return item;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetUserInfoByAccountAndPassword");
                    return null;
                }
            }
        }
        /// <summary>
        /// Get all users' information.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public List<UserInfo> GetAllUserInfo()
        {
            using (FamilyDataClassesDataContext context=new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    Table<UserInfo> allUserInfo = context.UserInfo;
                    return allUserInfo.ToList();
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetAllUserInfo");
                    return null;
                }
            }
        }
        #endregion

        #region ConsumeType
        /// <summary>
        /// Get all consume type.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public List<ConsumeType> GetAllConsumeType()
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    List<ConsumeType> allConsumeType = context.ConsumeType.ToList();
                    return allConsumeType;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetAllConsumeType");
                    return null;
                }
            }
        }
        /// <summary>
        /// Get consume type by id.
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        [OperationContract]
        public ConsumeType GetConsumeTypeById(int? typeID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    ConsumeType item = context.ConsumeType.SingleOrDefault(u => u.typeID == typeID);
                    return item;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetConsumeTypeById");
                    return null;
                }
            }
        }
        #endregion

        #region Consume
        /// <summary>
        /// Get consumes by consume type id.
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        [OperationContract]
        public List<Consume> GetConsumeByTypeID(int typeID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    List<Consume> items = context.Consume.Where(u => u.typeID.Equals(typeID)).ToList();
                    return items;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetAllConsumeByTypeID");
                    return null;
                }
            }
        }
        /// <summary>
        /// Delete consume by daily id.
        /// </summary>
        /// <param name="dailyID"></param>
        /// <returns></returns>
        [OperationContract]
        public int DeleteConsumeByDailyID(int dailyID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    var items = context.Consume.Where(u => u.dailyID.Equals(dailyID));
                    context.Consume.DeleteAllOnSubmit(items);
                    context.SubmitChanges();
                    return dailyID;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "DeleteConsumeByDailyID");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Get consumes by daily id.
        /// </summary>
        /// <param name="dailyID"></param>
        /// <returns></returns>
        [OperationContract]
        public List<Consume> GetConsumeByDailyID(int dailyID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    List<Consume> items = context.Consume.Where(u => u.dailyID.Equals(dailyID)).ToList();
                    return items;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetConsumeByDailyID");
                    return null;
                }
            }
        }
        /// <summary>
        /// Get consumes by daily ids.
        /// </summary>
        /// <param name="dailyIDs"></param>
        /// <returns></returns>
        [OperationContract]
        public List<NewConsume> GetConsumeByDailyIDs(int?[] dailyConsumeIDs)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    var query = from c in context.Consume 
                                join t in context.ConsumeType on c.typeID equals t.typeID into d
                                from t in d.DefaultIfEmpty()
                                where dailyConsumeIDs.Contains(c.dailyID)
                                select new NewConsume
                                {
                                    ConsumeID=c.consumeID,
                                    Description=c.description,
                                    Amount=c.amount,
                                    TypeID=c.typeID,
                                    TypeDesc=t.description,
                                    DailyID=c.dailyID
                                };
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetConsumeByDailyIDs");
                    return null;
                }
            }
        }
        /// <summary>
        /// Batch add consume.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public int BatchAddConsumes(List<Consume> items)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    context.Consume.InsertAllOnSubmit(items);
                    context.SubmitChanges();
                    return items.Count();
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "BatchAddConsumes");
                    return 0;
                }
            }
        }
        #endregion

        #region DailyConsume
        /// <summary>
        /// Add one daily consume.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [OperationContract]
        public int AddOneDailyConsume(DailyConsume item)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    context.DailyConsume.InsertOnSubmit(item);
                    context.SubmitChanges();
                    return item.dailyID;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "AddOneDailyConsume");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Delete one daily consume by id.
        /// </summary>
        /// <param name="dailyID"></param>
        /// <returns></returns>
        [OperationContract]
        public int DeleteDailyConsumeByID(int dailyID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                { 
                    DailyConsume item=context.DailyConsume.SingleOrDefault(u=>u.dailyID.Equals(dailyID));
                    context.DailyConsume.DeleteOnSubmit(item);
                    context.SubmitChanges();
                    return item.dailyID;
                }
                catch(Exception ex)
                {
                    Log.WriteLog(ex.ToString(),"DeleteDailyConsumeByID");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update daily consume.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        public int UpdateDailyConsume(DailyConsume model)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    DailyConsume item = context.DailyConsume.SingleOrDefault(u => u.dailyID.Equals(model.dailyID));
                    item.amount = model.amount;
                    item.date = model.date;
                    context.SubmitChanges();
                    return item.dailyID;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "UpdateDailyConsume");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Get daily consume.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        public List<DailyConsume> GetDailyConsume(DateTime startDate, DateTime endDate)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    var query = from d in context.DailyConsume select d;
                    query = query.Where(u => u.date >= startDate && u.date <= endDate);
                    query = query.OrderByDescending(u => u.date);
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetAllDailyConsume");
                    return null;
                }
            }
        }
        /// <summary>
        /// Get daily consume by id.
        /// </summary>
        /// <param name="dailyID"></param>
        /// <returns></returns>
        [OperationContract]
        public DailyConsume GetDailyConsumeByID(int dailyID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    DailyConsume item = context.DailyConsume.SingleOrDefault(u => u.dailyID.Equals(dailyID));
                    return item;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetDailyConsumeByID");
                    return null;
                }
            }
        }
        /// <summary>
        /// Get the daily consume according to date.
        /// </summary>
        /// <param name="date"></param>
        [OperationContract]
        public DailyConsume GetTheDailyConsumeByDate(DateTime date)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    DailyConsume item = context.DailyConsume.SingleOrDefault(u => u.date == date);
                    return item;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetOneDailyConsumeByDate");
                    return null;
                }
            }
        }
        #endregion

        #region ParaDetail
        /// <summary>
        /// Get parameter detail according to parameter information id. 
        /// </summary>
        /// <param name="paraID"></param>
        /// <returns></returns>
        [OperationContract]
        public List<ParaDetail> GetParaDetailByInfoID(int infoID)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    List<ParaDetail> items=context.ParaDetail.Where(u => u.infoID.Equals(infoID)).ToList();
                    return items;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetParaDetailByInfoID");
                    return null;
                }
            }
        }
        #endregion

        #region BankCard
        /// <summary>
        /// Add back card.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [OperationContract]
        public int AddBankCard(BankCard item)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    context.BankCard.InsertOnSubmit(item);
                    context.SubmitChanges();
                    return item.cardID;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "AddBankCard");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Update bank card.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        public int UpdateBankCard(BankCard model)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    BankCard item = context.BankCard.SingleOrDefault(u => u.cardID.Equals(model.cardID));
                    item.cardNO = model.cardNO;
                    item.amount = model.amount;
                    item.dateFrom = model.dateFrom;
                    item.dateTo = model.dateTo;
                    item.userID = model.userID;
                    item.cardTypeID = model.cardTypeID;
                    item.bankID = model.bankID;
                    item.cityID = model.cityID;
                    item.isUsing = model.isUsing;
                    context.SubmitChanges();
                    return Convert.ToInt32(item.bankID);
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "UpdateBankCard");
                    return 0;
                }
            }
        }
        /// <summary>
        /// Get back card.
        /// </summary>
        [OperationContract]
        public List<NewBankCard> GetBackCard(NewBankCard item)
        {
            using (FamilyDataClassesDataContext context = new FamilyDataClassesDataContext(FamilyDBConnectionString))
            {
                try
                {
                    var query = from a in context.BankCard
                                join b in context.UserInfo on a.userID equals b.userID into ab
                                from b in ab.DefaultIfEmpty()
                                join c in context.ParaDetail on a.cardTypeID equals c.detailID into ac
                                from c in ac.DefaultIfEmpty()
                                join d in context.ParaDetail on a.bankID equals d.detailID into ad
                                from d in ad.DefaultIfEmpty()
                                join e in context.ParaDetail on a.cityID equals e.detailID into ae
                                from e in ae.DefaultIfEmpty()
                                select new NewBankCard
                                {
                                    CardID = a.cardID,
                                    CardNO = a.cardNO,
                                    Amount = a.amount,
                                    DateFrom = a.dateFrom,
                                    DateTo = a.dateTo,
                                    UserID = a.userID,
                                    UserName = b.userName,
                                    CardTypeID = a.cardTypeID,
                                    CardType = c.description,
                                    BankID = a.bankID,
                                    Bank = d.description,
                                    CityID = a.cityID,
                                    City = e.description,
                                    IsUsing = a.isUsing
                                };
                    if (item.CardNO != null)
                    {
                        query = query.Where(u => u.CardNO.Contains(item.CardNO));
                    }
                    if (item.Amount != null)
                    {
                        query = query.Where(u => u.Amount.Equals(item.Amount));
                    }
                    if (item.UserID != null)
                    {
                        query = query.Where(u => u.UserID.Equals(item.UserID));
                    }
                    if (item.CardTypeID != null)
                    {
                        query = query.Where(u => u.CardTypeID.Equals(item.CardTypeID));
                    }
                    if (item.BankID != null)
                    {
                        query = query.Where(u => u.BankID.Equals(item.BankID));
                    }
                    if (item.CityID != null)
                    {
                        query = query.Where(u => u.CityID.Equals(item.CityID));
                    }
                    if (item.IsUsing != null)
                    {
                        query = query.Where(u => u.IsUsing.Equals(item.IsUsing));
                    }
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "GetBackCard");
                    return null;
                }
            }
        }
        #endregion 

        #region Others
        [OperationContract]
        public bool IsTest()
        {
            return isTest;
        }
        #endregion

    }
}
