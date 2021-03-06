﻿using UnityEngine;

namespace amy.RWControl
{
	public interface controlRW
	{
		string readFile(string dir, string filename);
		string readLine(string dir, string filename, int line);
		void writeFile(string dir, string filename, string message);
		void writeFile_Append(string dir, string filename, string message);
		void deleteFile(string dir, string filename);
		void createDir(string dir);
		void deleteDir(string dir);
		bool checkFile(string path);
		bool checkDir(string dir);
	}

	public interface controlRWCallback
	{
		void RWStatus(string status);
	}
}