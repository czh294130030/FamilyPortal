using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
{
    public class Field
    {
        #region enum
        public enum Login
        { 
            Login,
            Logout
        }
        /// <summary>
        /// Time period.
        /// </summary>
        public enum TimePeriod
        {
            Day,
            Week,
            Month,
            Quarter,
            Year
        }
        #endregion
    }
}
