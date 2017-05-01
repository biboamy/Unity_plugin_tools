using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cqplayart.adv.image;

public class imageControlSample : MonoBehaviour 
{
	controlImage myscript;
	controlImageCallback mycallback = new imageControlCallback();

	public GameObject plane;

	void Start()
	{
		myscript = new imageControlBehavior(mycallback);
	}

	public void screenShot()
	{
		StartCoroutine(myscript.screenShot("myTest", "test.jpg"));
	}

	public void saveToGallery()
	{
		StartCoroutine(myscript.saveToGallery(Application.persistentDataPath + "/myTest/test.jpg", "callback"));
	}

	public void previewImage()
	{
		StartCoroutine(myscript.previewImage(plane, Application.persistentDataPath + "/myTest/test.jpg"));
	}

	public void shareImage()
	{
		myscript.shareImage(Application.persistentDataPath + "/myTest/test.jpg");
	}

	public void albumPicker()
	{
		myscript.albumPicker("callback");
	}
}
