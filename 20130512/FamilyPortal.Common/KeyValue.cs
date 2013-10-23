using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
{
    [Serializable]
    public class KeyValue
    {
        private object key;

        public object Key
        {
            get { return key; }
            set { key = value; }
        }
        private object value;

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}

