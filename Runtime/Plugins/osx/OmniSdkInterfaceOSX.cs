using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace omnislash_sdk
{
	enum Color { red, green, blue, black, white, yellow, orange };

	public	class	OmniSdkInterfaceOSX
	{
		public	delegate void debugCallback(IntPtr request, int color, int size);

		[DllImport("OmniSdk")]
		public	static extern void RegisterDebugCallback(debugCallback cb);
		
		[DllImport("OmniSdk")]
		public static extern void init();

		[DllImport("OmniSdk")]
		public static extern void close();
	}
}