using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace omnislash_sdk
{
	enum Color { red, green, blue, black, white, yellow, orange };

	public	class	OmniSdkInterface
	{
		// delegate callback definition
		public	delegate	void	debugCallback(IntPtr _request, int _color, int _size);

		[DllImport("OmniSdk")]
		public	static	extern	void	registerDebugCallback(debugCallback _cb);

		[DllImport("OmniSdk")]
		public	static	extern	void	setLogLevel(int _logLevel);

		[DllImport("OmniSdk")]
		public	static	extern	void	create();

		[DllImport("OmniSdk")]
		public	static	extern	void	init(string _developerKey, string _gameKey, string _localEventsPath);

		[DllImport("OmniSdk")]
		public	static	extern	int		inMenu(string _description);

		[DllImport("OmniSdk")]
		public	static	extern	int		screenshot(int _moment, int _delayMSec, string _caption, string[] _tags, int _tagsCount, string[] _metaDataKeys, String[] _metaDataValues, int _metaDataCount);

		[DllImport("OmniSdk")]
		public	static	extern	bool	isInstalled();

		[DllImport("OmniSdk")]
		public	static	extern	bool	isRunning();

		[DllImport("OmniSdk")]
		public	static	extern	void	getUrl(int _type, string[] _paramKeys, string[] _paramValues, int _paramCount, StringBuilder _str, int _strLen);

		[DllImport("OmniSdk")]
		public	static	extern	void	destroy();
	}
}