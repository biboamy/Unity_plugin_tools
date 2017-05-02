using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace amy.image
{
	public interface controlImage
	{
		IEnumerator screenShot(string dir, string _name);
		IEnumerator saveToGallery(string path, string callbackName);
		IEnumerator previewImage(GameObject plane, string path);
		void shareImage(string path);
		void albumPicker(string callbackName);
	}

	public interface controlImageCallback
	{
		void imageStatus(string status);
		void albumImagePath(string path);
		void screenShotPath(string path);
		void galleryPath(string path);
	}
}