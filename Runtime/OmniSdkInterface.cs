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
		public	static	extern	void	init(string _developerKey, string _gameKey);

		[DllImport("OmniSdk")]
		public	static	extern	void	destroy();
	}
}