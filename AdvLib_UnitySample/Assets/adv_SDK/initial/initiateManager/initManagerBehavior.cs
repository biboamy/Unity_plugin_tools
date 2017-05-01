using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public class initManagerBehavior : MonoBehaviour, initManager
	{
		static List<string> type = new List<string>();
		static List<string> waitList = new List<string>();

		public void addInitiateList(string _type, params object[] args)
		{
			type.Add(_type);

		}

		public IEnumerator LoadList(string dir)
		{
			return null;
		}
	}
}