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

	public	void	onBtnClose()
	{
		OmniSdk.Destroy();
	}	
}
