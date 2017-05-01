using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cqplayart.adv.RWControl;

public class RWControlControl : MonoBehaviour {

	controlRW myControl;
	controlRWCallback mycallback = new RWControlCallback();

	Text message;

	// Use this for initialization
	void Start () {

		message = GameObject.Find("message").GetComponent<Text>();
		myControl = new RWControlBehavior(mycallback);
	}

	//write message to file (this method will replace the old text)
	//base path is persistentDataPath
	public void write()
	{
		myControl.writeFile("mytest", "test.txt", "hi i am amy");
	}

	//write message to file (this method will append on the old text)
	//base path is persistentDataPath
	public void writeFile_Append()
	{
		myControl.writeFile_Append("mytest/second", "test.txt", "hi i am amy");
	}

	//read the file content (read whole content)
	//base path is persistentDataPath
	public void readFile()
	{
		message.text = myControl.readFile("mytest", "test.txt");
	}

	//read the file content (read one line)
	//base path is persistentDataPath
	public void readLine()
	{
		message.text = myControl.readLine("mytest/second", "test.txt", 2);
	}

	//delete the file
	public void deleteFile()
	{
		myControl.deleteFile(Application.persistentDataPath + "/mytest", "test.txt");
	}

	//create the directory
	public void createDir()
	{
		myControl.createDir(Application.persistentDataPath + "/new dir");
	}

	//elete the directory
	public void deleteDir()
	{
		myControl.deleteDir(Application.persistentDataPath + "/new dir");
	}

	//check whether the file exists
	public void checkFile()
	{
		message.text = myControl.checkFile(Application.persistentDataPath + "/mytest/test.txt").ToString();
	}

	//check whether the directory exits
	public void checkDir()
	{
		message.text = myControl.checkDir(Application.persistentDataPath + "/mytest").ToString();
	}
}
