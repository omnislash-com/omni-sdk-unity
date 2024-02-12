using System.Runtime.InteropServices;
using System;
using System.Text;
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

		public	static	bool	IsSupported()
		{
			#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				#if UNITY_64
					return true;
				#else
					return false;
				#endif
			#else
				return false;
			#endif
		}

		public	static	bool	Init(string _gamePublicKey, string _gameCode, LogLevel _logLevel = LogLevel.Info)
		{
			// save the game code
			OmniSdk.gameCode = _gameCode;

			if (OmniSdk.IsSupported() == false)
				return false;

			// set up logs
			bool	ok = OmniSdk.SetUpLogs(_logLevel);
			if (ok == false)
				return false;

			// get the folder where to save the log file
			string	path = Application.persistentDataPath + "/Omnislash/";
			Debug.Log("Log path: " + path);

			// init
			ok = OmniSdk.Instance.init(_gamePublicKey, _gameCode, path);
			if (ok == false)
				return false;

			return true;		
		}

		public	static	int	InMenu(string _description)
		{
			if (OmniSdk.IsSupported() == false)
				return -101;
				
			return OmniSdk.Instance.inMenu(_description);
		}

		public	static	int	SetUserId(string _value)
		{
			if (OmniSdk.IsSupported() == false)
				return -101;
				
			return OmniSdk.Instance.setUserId(_value);
		}

		public	static	int	SetUsername(string _value)
		{
			if (OmniSdk.IsSupported() == false)
				return -101;
				
			return OmniSdk.Instance.setUsername(_value);
		}

		public	static	int	SetMetadata(Dictionary<string, object> _metaData = null)
		{
			if (OmniSdk.IsSupported() == false)
				return -101;

			return OmniSdk.Instance.setMetadata(_metaData);
		}

		public	static	int	EmitTrigger(int _moment = 0, string _caption = "", Dictionary<string, object> _metaData = null, List<string> _tags = null)
		{
			if (OmniSdk.IsSupported() == false)
				return -101;

			return OmniSdk.Instance.emitTrigger(_moment, _caption, _tags, _metaData);
		}

		public	static	bool	IsInstalled()
		{
			if (OmniSdk.IsSupported() == false)
				return false;

			return OmniSdk.Instance.isInstalled();
		}

		public	static	bool	IsRunning()
		{
			if (OmniSdk.IsSupported() == false)
				return false;
				
			return OmniSdk.Instance.isRunning();
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

		private	static	string				gameCode = "";
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

		private	bool	init(string _gamePublicKey, string _gameCode, string _localEventsPath)
		{
			try
			{
				// create the instance
				OmniSdkInterface.create();

				// init the SDK
				OmniSdkInterface.init(_gamePublicKey, _gameCode, _localEventsPath);

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

		private	int	setUserId(string _value)
		{
			try
			{
				// call the SDK
				return OmniSdkInterface.setUserId(_value);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.setUserId: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
			}				
		}

		private	int	setUsername(string _value)
		{
			try
			{
				// call the SDK
				return OmniSdkInterface.setUsername(_value);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.setUsername: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
			}				
		}

		private	int	setMetadata(Dictionary<string, object> _metaData)
		{
			try
			{
				// ensure arrays are good
				if (_metaData == null)
					_metaData = new Dictionary<string, object>();

				// convert parameters
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
				return OmniSdkInterface.setMetadata(metaDataKeys, metaDataValues, metaDataCount);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.setMetadata: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
			}				
		}

		private	int	emitTrigger(int _moment, string _caption, List<string> _tags, Dictionary<string, object> _metaData)
		{
			try
			{
				// cap moment
				if (_moment < 0)
					_moment = 0;
				else if (_moment > 100)
					_moment = 100;

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
				return OmniSdkInterface.emitTrigger(_moment, _caption, tags, tagsCount, metaDataKeys, metaDataValues, metaDataCount);
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.emitTrigger: Exception caught.");
				Debug.LogError(e.Message);

				return -100;
			}				
		}

		private	bool	isInstalled()
		{
			try
			{
				return OmniSdkInterface.isInstalled();
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.isInstalled: Exception caught.");
				Debug.LogError(e.Message);

				return false;
			}			
		}

		private	bool	isRunning()
		{
			try
			{
				return OmniSdkInterface.isRunning();
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.isRunning: Exception caught.");
				Debug.LogError(e.Message);

				return false;
			}			
		}

		private	string	getUrl(UrlType _type, Dictionary<string, string> _params = null)
		{
			try
			{
				// make sure we have something in the params
				if (_params == null)
					_params = new Dictionary<string, string>();

				int				strLen = 500;
				StringBuilder	str = new StringBuilder(strLen);
				int				paramsCount = _params.Count;
				string[]		paramsKeys = new string[paramsCount];
				string[]		paramsValues = new string[paramsCount];
				int 			i = 0;
				foreach(var pair in _params)
				{
					paramsKeys[i] = pair.Key;
					paramsValues[i] = pair.Value;
					i++;
				}

				// call it
				OmniSdkInterface.getUrl((int) _type, paramsKeys, paramsValues, paramsCount, str, strLen);

				return str.ToString();
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.getUrl: Exception caught.");
				Debug.LogError(e.Message);

				return "";
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