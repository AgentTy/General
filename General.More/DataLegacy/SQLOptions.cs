using System;
using General;
using General.Configuration;

namespace General.DAO
{
	/// <summary>
	/// SQLOptions
	/// </summary>
	public class SQLOptions
	{

		#region Private Variables
		bool _boolDoMemoryCache;
		bool _boolDoFileCache;
		DateTime _dateExpiration;
		string _strConnectionString;
		bool _boolDoLog;
		bool _boolEmailLog;
		//bool _boolDoTrace;
		TextLog _objActivityLog;
        bool _boolCloseConnection;
		#endregion

		#region Constructors
		/// <summary>
		/// SQLOptions
		/// </summary>
		public SQLOptions()
		{
			_boolDoLog = false;
			_boolEmailLog = false;
			_objActivityLog = null;
			_boolDoMemoryCache = false;
			_boolDoFileCache = false;
            _boolCloseConnection = true;
			//_boolDoTrace = false;
			_strConnectionString = DBConnection.GetConnectionString();
			int intTimeoutMinutes;
			try {intTimeoutMinutes = Convert.ToInt32(GlobalConfiguration.GlobalSettings["sqlhelper_data_cache_timeout"]);} 
			catch {intTimeoutMinutes = 240;}
			_dateExpiration = DateTime.Now.AddMinutes(intTimeoutMinutes);
		}

        public SQLOptions(System.Data.SqlClient.SqlConnection objConn)
        {
            _boolDoLog = false;
            _boolEmailLog = false;
            _objActivityLog = null;
            _boolDoMemoryCache = false;
            _boolDoFileCache = false;
            _boolCloseConnection = true;
            //_boolDoTrace = false;
            if (objConn != null)
                _strConnectionString = objConn.ConnectionString;
            else
                _strConnectionString = DBConnection.GetConnectionString();
            int intTimeoutMinutes;
            try { intTimeoutMinutes = Convert.ToInt32(GlobalConfiguration.GlobalSettings["sqlhelper_data_cache_timeout"]); }
            catch { intTimeoutMinutes = 240; }
            _dateExpiration = DateTime.Now.AddMinutes(intTimeoutMinutes);
        }

		#endregion

		#region Public Methods
		/// <summary>
		/// Adds a line to the Activity Log
		/// </summary>
		public void WriteToLog(string Message)
		{
			if(_objActivityLog != null)
				_objActivityLog.Write(Message);
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Activates the Activity Log
		/// </summary>
		public bool DoLog
		{
			get { return _boolDoLog; }
			set 
			{ 
				_boolDoLog = value;
				if(_boolDoLog)
				{
					_objActivityLog = new TextLog();
				}
				else
				{
					_objActivityLog = null;
				}
			}
		}

		/// <summary>
		/// Emails the Activity Log upon completion
		/// </summary>
		public bool EmailLog
		{
			get { return _boolEmailLog; }
			set { _boolEmailLog = value; }
		}

		/// <summary>
		/// Activates Memory Caching
		/// </summary>
		public bool DoMemoryCache
		{
			get { return _boolDoMemoryCache; }
			set { _boolDoMemoryCache = value; }
		}

		/// <summary>
		/// Activates File Caching
		/// </summary>
		public bool DoFileCache
		{
			get { return _boolDoFileCache; }
			set { _boolDoFileCache = value; }
		}

        /// <summary>
        /// Should I close the SQLConnection after getting data
        /// </summary>
        public bool CloseConnection
        {
            get { return _boolCloseConnection; }
            set { _boolCloseConnection = value; }
        }

		/*
		/// <summary>
		/// Activates Tracing
		/// </summary>
		public bool DoTrace
		{
			get { return _boolDoTrace; }
			set { _boolDoTrace = value; }
		}
		*/

		/// <summary>
		/// Sets cache expiration
		/// </summary>
		public DateTime Expiration
		{
			get { return _dateExpiration; }
			set { _dateExpiration = value; }
		}

		/// <summary>
		/// Overrides ConnectionString
		/// </summary>
		public string ConnectionString
		{
			get { return _strConnectionString; }
			set { _strConnectionString = value; }
		}

		/// <summary>
		/// Gets the Activity Log
		/// </summary>
		public TextLog ActivityLog
		{
			get { return _objActivityLog; }
		}
		#endregion 

	}
}
