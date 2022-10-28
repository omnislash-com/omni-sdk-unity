using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;

namespace omnislash_sdk
{
	public	class	OmniSdk
	{
		public	OmniSdk()
		{
			try
			{
				Debug.Log("1. Set up callback");
				OmniSdkInterfaceOSX.RegisterDebugCallback(OnDebugCallback);

				Debug.Log("3. Done");
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.cosntructor: Exception caught.");
				Debug.LogError(e.Message);
			}
		}

		public	void	init()
		{
			try
			{
				Debug.Log("1. Init");
				OmniSdkInterfaceOSX.init();

				Debug.Log("2. Done");
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.init: Exception caught.");
				Debug.LogError(e.Message);
			}			
		}

		public	void	close()
		{
			try
			{
				Debug.Log("1. Closing");
				OmniSdkInterfaceOSX.close();
				Debug.Log("2. Done");
			}
			catch(Exception e)
			{
				Debug.LogError("OmniSdk.close: Exception caught.");
				Debug.LogError(e.Message);
			}
		}

		~OmniSdk()
		{
			this.close();
		}

		[MonoPInvokeCallback(typeof(OmniSdkInterfaceOSX.debugCallback))]
		static	void	OnDebugCallback(IntPtr request, int color, int size)
		{
			//Ptr to string
			string debug_string = Marshal.PtrToStringAnsi(request, size);

			//Add Specified Color
			debug_string =
				String.Format("{0}{1}{2}{3}{4}",
				"<color=",
				((Color)color).ToString(),
				">",
				debug_string,
				"</color>"
				);

			Debug.Log(debug_string);
		}
	}
}