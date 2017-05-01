using UnityEngine;
using cqplayart.adv.RWControl;

public class RWControlCallback : controlRWCallback 
{
	public void RWStatus(string status)
	{
		Debug.Log(status);
	}
}
