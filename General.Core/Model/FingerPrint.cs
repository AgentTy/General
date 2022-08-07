using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General
{
    [Serializable]
    public struct FingerPrint
    {
        public int CreatedBy;
        public string CreatedByName;
        public DateTime CreatedOn;

        public int ModifiedBy;
        public string ModifiedByName;
        public DateTime ModifiedOn;

        public static FingerPrint Null
        {
            get
            {
                FingerPrint obj = new FingerPrint();
                obj.CreatedBy = -1;
                obj.CreatedOn = DateTime.MinValue;
                obj.ModifiedBy = -1;
                obj.ModifiedOn = DateTime.MinValue;
                return obj;
            }
        }
    }
}
