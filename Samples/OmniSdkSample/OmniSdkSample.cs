using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using omnislash_sdk;

public	class	OmniSdkSample : MonoBehaviour
{
	public	omnislash_sdk.LogLevel	m_logLevel = omnislash_sdk.LogLevel.Info;
	public	string					m_developerKey = "testkey";
	public	string					m_gameKey = "youriding";

	public	void	onBtnInit()
	{
		OmniSdk.Init(this.m_developerKey, this.m_gameKey, this.m_logLevel);
	}

	public	void	onBtnInMenuSettings()
	{
		OmniSdk.InMenu("settings");
	}

	public	void	onBtnInMenuShop()
	{
		OmniSdk.InMenu("shop");
	}

	public	void	onBtnInMenuPlay()
	{
		OmniSdk.InMenu("play");
	}

	public	void	onBtnScreenshotAverage()
	{
		OmniSdk.Screenshot(20, "Average screenshot", new Dictionary<string, object> {
			{"filter:wave", "pipeline"},
			{"filter:action", "maneuver"},
			{"filter:maneuver", "air_forward"},
			{"points", 14900},
		});
	}

	public	void	onBtnScreenshotGood()
	{
		OmniSdk.Screenshot(50, "Good screenshot", new Dictionary<string, object> {
			{"filter:wave", "cloud9"},
			{"filter:action", "maneuver"},
			{"filter:maneuver", "backflip"},
			{"points", 22000},
		});
	}

	public	void	onBtnScreenshotAmazing()
	{
		OmniSdk.Screenshot(90, "Amazing screenshot", new Dictionary<string, object> {
			{"filter:wave", "skeletonbay"},
			{"filter:action", "maneuver"},
			{"filter:maneuver", "air_reverse"},
			{"points", 32500},
		});
	}

	public	void	onBtnScreenshotPast()
	{
		OmniSdk.ScreenshotPast(5000, 70, "Back in time screenshot", new Dictionary<string, object> {
			{"filter:wave", "supertubos"},
			{"filter:action", "maneuver"},
			{"filter:maneuver", "forward_spin"},
			{"points", 12450},
		});
	}

	public	void	onBtnClose()
	{
		OmniSdk.Destroy();
	}	
}
