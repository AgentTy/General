using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace General
{

    public interface IObjectBase
    {
        Int32 ID { get; set; }
    }

    [Serializable, DataContract]
    public class ObjectBase : IObjectBase
    {

        [DataMember]
        public Int32 ID
        {
            get;
            set;
        }

        [NonSerialized]
        public General.FingerPrint FingerPrint;

    }

}
