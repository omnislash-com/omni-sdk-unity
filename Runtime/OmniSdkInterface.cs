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
		public	static	extern	int		setUserId(string _value);

		[DllImport("OmniSdk")]
		public	static	extern	int		setUsername(string _value);

		[DllImport("OmniSdk")]
		public	static	extern	int		setMetadata(string[] _metaDataKeys, string[] _metaDataValues, int _metaDataCount);

		[DllImport("OmniSdk")]
		public	static	extern	int		emitTrigger(string _action, int _moment, string _caption, string[] _tags, int _tagsCount, string[] _metaDataKeys, string[] _metaDataValues, int _metaDataCount);

		[DllImport("OmniSdk")]
		public	static	extern	bool	isInstalled();

		[DllImport("OmniSdk")]
		public	static	extern	bool	isRunning();

		[DllImport("OmniSdk")]
		public	static	extern	void	destroy();
	}
}