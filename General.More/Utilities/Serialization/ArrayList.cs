using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Utilities.Serialization
{
    public class ArrayList
    {
        public static string SerializeArrayList(System.Collections.ArrayList input, string strDelimiter, bool blnStrongDataType)
        {
            string strList = string.Empty;
            foreach (object o in input)
                if(blnStrongDataType)
                    strList += Serialization.SerializationTools.SerializeObject(o) + strDelimiter;
                else
                    strList += o.ToString() + strDelimiter;

            if (strList != string.Empty)
                strList = StringFunctions.Shave(strList, strDelimiter.Length);

            return strList;
        }

        public static System.Collections.ArrayList DeserializeArrayList(string input, string strDelimiter)
        {
            System.Collections.ArrayList objList = new System.Collections.ArrayList();
            string[] aryDelimiter = new string[1];
            aryDelimiter[0] = strDelimiter;
            foreach (string o in input.Split(aryDelimiter, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    objList.Add(SerializationTools.DeserializeObject(o));
                }
                catch
                {
                    try
                    {
                        objList.Add(int.Parse(o));
                    }
                    catch { }
                }
            }
            return objList;
        }
    }
}
