using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Drawing.Contracts
{
    class MyApp
    {

        int intClientID;
        private void MakeAContract()
        {

            #region MakeAContract
            Contracts.IContractWriter objWriter = null;
            if(intClientID == 1)
                objWriter = new Contracts.ContractWriter_BridalSpec();
            else if(intClientID == 2)
                objWriter = new Contracts.ContractWriter_AnotherClient();

            objWriter.MakeContract();
            #endregion


        }
    }
}
