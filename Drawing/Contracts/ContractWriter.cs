using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace General.Drawing.Contracts
{
    /// <summary>
    /// This base class will help me populate a PDF with the variables to fill in a clients contract
    /// </summary>
    public class ContractWriter
    {

        #region Constructor
        public ContractWriter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region HelpMeOut
        protected void HelpMeOut(string s)
        {

        }
        #endregion

    }


    public interface IContractWriter
    {
        void MakeContract();
        //void DoThis();
    }
}