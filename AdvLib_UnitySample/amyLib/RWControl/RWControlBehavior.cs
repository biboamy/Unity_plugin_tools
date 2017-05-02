using System;
using UnityEngine;
using System.IO;

namespace amy.RWControl
{
	public class RWControlBehavior : MonoBehaviour, controlRW
	{
		/// <summary>
		/// Status callback
		/// </summary>
		controlRWCallback mycallback;

		/// <summary>
		/// Init the script.
		/// </summary>
		/// <returns>The script.</returns>
		/// <param name="callback">The callback.</param>
		public RWControlBehavior (controlRWCallback callback)
		{
			mycallback = callback;
		}

		/// <summary>
		/// Write message to the file (will replace the old content).
		/// </summary>
		/// <param name="dir">Directory path.</param>
		/// <param name="filename">Filename.</param>
		/// <param name="message">message.</param>
		public void writeFile(string dir, string filename, string message)
		{
			StreamWriter write = null;

			string dirPath = Application.persistentDataPath + "/" + dir;
			string filePath = dirPath + "/" + filename;
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}

			if (!File.Exists(filePath))
			{
				FileInfo file = new FileInfo(filePath);
				write = file.CreateText();
			}
			else
			{
				write = new StreamWriter(filePath, false);
			}

			write.WriteLine(message);
			write.Close();

			mycallback.RWStatus("Write to file success.");
		}

		/// <summary>
		/// Write message to the file (will append on the old content).
		/// </summary>
		/// <param name="dir">Directory path.</param>
		/// <param name="filename">Filename.</param>
		/// <param name="message">Message.</param>
		public void writeFile_Append(string dir, string filename, string message)
		{
			StreamWriter write = null;

			string dirPath = Application.persistentDataPath + "/" + dir;
			string filePath = dirPath + "/" + filename;
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}

			if (!File.Exists(filePath))
			{
				FileInfo file = new FileInfo(filePath);
				write = file.CreateText();
				write.Write(1);
				write.Write("\n" + "1 " + message);
				write.Close();
			}
			else
			{
				var lines = File.ReadAllLines(filePath);
				lines[0] = (int.Parse(lines[0]) + 1).ToString();
				File.WriteAllLines(filePath, lines);

				using (write = new StreamWriter(filePath, true))
				{
					write.Write(lines[0] + " " + message);
					write.Close();
				}
			}
			mycallback.RWStatus("Write to file success.");
		}

		/// <summary>
		/// Read the file (will read the whole content).
		/// </summary>
		/// <returns>The content.</returns>
		/// <param name="dir">Directory path.</param>
		/// <param name="filename">Filename.</param>
		public string readFile(string dir, string filename)
		{
			string dirPath = Application.persistentDataPath + "/" + dir;
			string filePath = dirPath + "/" + filename;
			if (!Directory.Exists(dirPath))
			{
				mycallback.RWStatus("Directory not found.");
			}

			if (File.Exists(filePath))
			{
				FileInfo file = new FileInfo(filePath);
				StreamReader reader = file.OpenText();
				string info = reader.ReadToEnd();

				reader.Close();
				return info;
			}
			else
			{
				mycallback.RWStatus("File not found.");
			}

			return null;
		}

		/// <summary>
		/// Read the file (will read only one line).
		/// </summary>
		/// <returns>The content.</returns>
		/// <param name="dir">Directory path.</param>
		/// <param name="filename">Filename.</param>
		/// <param name="line">Which line you want to read.</param>
		public string readLine(string dir, string filename, int line)
		{
			string dirPath = Application.persistentDataPath + "/" + dir;
			string filePath = dirPath + "/" + filename;
			if (!Directory.Exists(dirPath))
			{
				mycallback.RWStatus("Directory not found.");
			}

			if (File.Exists(filePath))
			{
				try
				{
					var lines = File.ReadAllLines(filePath);
					return lines[line];
				}
				catch (System.Exception)
				{
					mycallback.RWStatus("Line not found");
					return null;
				}
			}
			else
			{
				mycallback.RWStatus("File not found.");
			}

			return null;
		}

		/// <summary>
		/// Delete the file.
		/// </summary>
		/// <param name="dirPath">Directory path.</param>
		/// <param name="filename">Filename.</param>
		public void deleteFile(string dirPath, string filename)
		{
			string filePath = dirPath + "/" + filename;
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				mycallback.RWStatus("File delete success.");
			}
			else
				mycallback.RWStatus("File not found.");
		}

		/// <summary>
		/// Create the directory.
		/// </summary>
		/// <param name="dirPath">Directory path.</param>
		public void createDir(string dirPath)
		{
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
				mycallback.RWStatus("Create directory success.");
			}
			else
			{
				mycallback.RWStatus("Directory has already existed.");
			}
		}

		/// <summary>
		/// Delete the directory.
		/// </summary>
		/// <param name="dirPath">Directory path.</param>
		public void deleteDir(string dirPath)
		{
			if (Directory.Exists(dirPath))
			{
				Directory.Delete(dirPath);
				mycallback.RWStatus("Directory delete success.");
			}
			else
				mycallback.RWStatus("Directory not found.");
		}

		/// <summary>
		/// Check whether the file exists or not.
		/// </summary>
		/// <returns><c>true</c>If file exists, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public bool checkFile(string path)
		{
			return File.Exists(path);
		}

		/// <summary>
		/// Check whether the directory exists or not.
		/// </summary>
		/// <returns><c>true</c>If directory exists, <c>false</c> otherwise.</returns>
		/// <param name="dirPath">Dir path.</param>
		public bool checkDir(string dirPath)
		{
			return Directory.Exists(dirPath);
		}
	}
}
