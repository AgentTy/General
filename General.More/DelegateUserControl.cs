using System;
using System.Web.UI;
using General;

namespace General
{
	/// <summary>
	/// Summary description for DelegateUserControl.
	/// </summary>
	public abstract class DelegateUserControl : UserControl
	{

		#region Private Variables
		private ControlBroadcastDelegate Listeners;
		#endregion

		#region Protected Objects
		public delegate void ControlBroadcastDelegate(object objData);
		#endregion

		#region Constructors

		public DelegateUserControl()
		{
			//Register Listeners
			/*
			foreach(Control c in this.Controls)
			{
				
				try
				{
					DelegateUserControl duc = (DelegateUserControl) c;
					duc.Listen(new ControlBroadcastDelegate(Update));
				}
				catch
				{
					//Fail Silently
				}
				
			}
			*/
		}

		#endregion

		#region Public Properties

		#endregion 

		#region Public Methods
		public void Listen(ControlBroadcastDelegate objListener)
		{
			Listeners += objListener;
		}
		#endregion

		#region Protected Methods
		protected void Broadcast(object objData)
		{
			if(Listeners != null)
				Listeners(objData);
		}
		#endregion

		#region Abstract Methods
		//protected abstract void Update(object objData);
		#endregion

		#region Private Functions

		#endregion

	}
}
