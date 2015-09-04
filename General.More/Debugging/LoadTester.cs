using System;
using General;
using System.Collections;
using System.Threading;

namespace General.Debugging
{
	/// <summary>
	/// Summary description for LoadTester.
	/// </summary>
	public class LoadTester
	{

		#region Private Variables
		private ArrayList _objPages;
		//private ArrayList _objResults;
		#endregion

		#region Constructors

		public LoadTester()
		{
			_objPages = new ArrayList();
		}

		#endregion

		#region Public Properties

		#endregion 

		#region Public Methods
		public string Run()
		{
			return _RunLoadTest();
		}

		public void AddPage(LoadTestPage objPage)
		{
			_objPages.Add(objPage);
		}
		#endregion

		#region Private Functions
		private string _RunLoadTest()
		{
			foreach(LoadTestPage p in _objPages)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(_RunPage));
			}

			return "";
		}

		private void _RunPage(Object stateInfo)
		{

		}
		#endregion


		public class LoadTestPage
		{
			public string Url;
			public string Method;
			public string PostData;

			public LoadTestPage(string strUrl, string strMethod, string strPostData)
			{
				Url = strUrl;
				Method = strMethod;
				PostData = strPostData;
			}
		}
	}
}
