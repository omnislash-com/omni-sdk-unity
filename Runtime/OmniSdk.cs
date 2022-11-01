using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;
using System.Collections.Generic;

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

			// get the folder where to save the log file
			string	path = Application.persistentDataPath + "/Omnislash/";
			Debug.Log("Log path: " + path);

			// init
			ok = OmniSdk.Instance.init(_developerKey, _gameKey, path);
			if (ok == false)
				return false;

			return true;		
		}

		public	static	int	InMenu(string _description)
		{
			return OmniSdk.Instance.inMenu(_description);
		}

		public	static	int	Screenshot(int _moment = 0, string _caption = "", Dictionary<string, object> _metaData = null, List<string> _tags = null)
		{
			return OmniSdk.Instance.screenshot(_moment, 0, _caption, _tags, _metaData);
		}

		public	static	int	ScreenshotPast(int _delayMSec, int _moment = 0, string _caption = "", Dictionary<string, object> _metaData = null, List<string> _tags = null)
		{
			return OmniSdk.Instance.screenshot(_moment, _delayMSec, _caption, _tags, _metaData);
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

		private	bool	init(string _developerKey, string _gameKey, string _localEventsPath)
		{
			try
			{
				// create the instance
				OmniSdkInterface.create();

				// init the SDK
				OmniSdkInterface.init(_developerKey, _gameKey, _localEventsPath);

				return true;
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.init: Exception caught.");
				Debug.LogError(e.Message);

				return false;
			}			
		}

		private	int	inMenu(string _description)
		{
			try
			{
				// call the SDK
				return OmniSdkInterface.inMenu(_description);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.inMenu: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
			}				
		}

		private	int	screenshot(int _moment, int _delayMSec, string _caption, List<string> _tags, Dictionary<string, object> _metaData)
		{
			try
			{
				// cap moment
				if (_moment < 0)
					_moment = 0;
				else if (_moment > 100)
					_moment = 100;

				// make sure we have a negative delay
				if (_delayMSec > 0)
					_delayMSec = - _delayMSec;

				// ensure arrays are good
				if (_tags == null)
					_tags = new List<string>();
				if (_metaData == null)
					_metaData = new Dictionary<string, object>();

				// convert parameters
				int			tagsCount = _tags.Count;
				string[]	tags = _tags.ToArray();
				int			metaDataCount = _metaData.Count;
				string[]	metaDataKeys = new string[metaDataCount];
				string[]	metaDataValues = new string[metaDataCount];
				int 		i = 0;
				foreach(var pair in _metaData)
				{
					metaDataKeys[i] = pair.Key;
					metaDataValues[i] = pair.Value.ToString();
					i++;
				}

				// init the SDK
				return OmniSdkInterface.screenshot(_moment, _delayMSec, _caption, tags, tagsCount, metaDataKeys, metaDataValues, metaDataCount);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.inMenu: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
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