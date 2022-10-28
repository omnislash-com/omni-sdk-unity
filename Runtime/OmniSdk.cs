using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;

namespace omnislash_sdk
{
	public	enum	LogLevel
	{
		Info = 1,
		Warning = 2,
		Error = 3,
		None = 4
	}

	public	class	OmniSdk
	{
		public	static	bool	Init(string _developerKey, string _gameKey, LogLevel _logLevel = LogLevel.Info)
		{
			// set up logs
			bool	ok = OmniSdk.SetUpLogs(_logLevel);
			if (ok == false)
				return false;

			// init
			ok = OmniSdk.Instance.init(_developerKey, _gameKey);
			if (ok == false)
				return false;

			return true;		
		}

		public	static	void	Destroy()
		{
			lock(OmniSdk.padlock)
			{
				if (OmniSdk.instance != null)
				{
					OmniSdk.instance.destroy();
					OmniSdk.instance = null;
				}
			}
		}



		private	static	OmniSdk				instance = null;
		private	static	readonly	object	padlock = new object();


		private	static	OmniSdk	Instance
		{
			get
			{
				lock(OmniSdk.padlock)
				{
					if (OmniSdk.instance == null)
					{
						OmniSdk.instance = new OmniSdk();
					}
					return OmniSdk.instance;
				}
			}
		}

		private	static	bool	SetUpLogs(LogLevel _logLevel = LogLevel.Info)
		{
			try
			{
				// set up the log callback
				OmniSdkInterface.registerDebugCallback(OnDebugCallback);

				// set the log level
				OmniSdkInterface.setLogLevel((int) _logLevel);

				return true;
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.cosntructor: Exception caught.");
				Debug.LogError(e.Message);

				return false;
			}			
		}

		private	OmniSdk()
		{
		}

		private	bool	init(string _developerKey, string _gameKey)
		{
			try
			{
				// create the instance
				OmniSdkInterface.create();

				// init the SDK
				OmniSdkInterface.init(_developerKey, _gameKey);

				return true;
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.init: Exception caught.");
				Debug.LogError(e.Message);

				return false;
			}			
		}

		private	void	destroy()
		{
			try
			{
				// destroy it
				OmniSdkInterface.destroy();
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.destroy: Exception caught.");
				Debug.LogError(e.Message);
			}
		}

		~OmniSdk()
		{
			this.destroy();
		}

		[MonoPInvokeCallback(typeof(OmniSdkInterface.debugCallback))]
		private	static	void	OnDebugCallback(IntPtr _request, int _color, int _size)
		{
			//Ptr to string
			string debug_string = Marshal.PtrToStringAnsi(_request, _size);

			//Add Specified Color
			debug_string =
				String.Format("{0}{1}{2}{3}{4}",
				"<color=",
				((Color)_color).ToString(),
				">",
				debug_string,
				"</color>"
				);

			Debug.Log(debug_string);
		}
	}
}