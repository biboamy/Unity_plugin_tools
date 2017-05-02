using UnityEngine;
using amy.RWControl;

public class RWControlCallback : controlRWCallback 
{
	public void RWStatus(string status)
	{
		Debug.Log(status);
	}
}
