using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cqplayart.adv.initial;

public class initiateDatasetControl : MonoBehaviour
{

	datasetInitiate myscript;
	datasetInitiateCallback mycallback = new initiateDatasetCallback();

	void Start()
	{
		myscript = GameObject.Find(name).AddComponent<initiateDatasetBehavior>();
		myscript.init(mycallback);

		myscript.loadDataset(Application.persistentDataPath + "/222.xml");

		//string[] ojName = { "oj1" };
		//myscript.loadDataset("https://app.cqplayart.com/wedding/dat/170322073/0328_for_ios2.xml", ojName);
	}
}
