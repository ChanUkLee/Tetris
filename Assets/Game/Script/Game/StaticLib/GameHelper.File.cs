using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;

public partial class GameHelper {
	public static string CombinePath (string pathA, string pathB) {
		string combine = Path.Combine (pathA, pathB);
		combine = combine.Replace ("\\", "/");
		return combine;
	}

	public static string DocumentsPath() {
		#if UNITY_EDITOR
		string path = Application.dataPath;
		path = path.Substring(0, path.LastIndexOf('/'));
		return path;
		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR
		string iOSPath = Application.persistentDataPath;
		iOSPath = iOSPath.Substring(0, iOSPath.LastIndexOf('/'));
		return GameFile.CombinePath(iOSPath, "Documents");
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		string androidPath = Application.persistentDataPath;
		androidPath = androidPath.Substring(0, androidPath.LastIndexOf('/'));
		return androidPath;
		#endif		
	}

	public class Resource {
		public static byte[] Read(string fullPath) {
			TextAsset textAsset = Resources.Load (fullPath) as TextAsset;
			if (textAsset != null) {
				MemoryStream stream = new MemoryStream (textAsset.bytes);
				if (stream != null) {
					return stream.ToArray ();
				}
			}

			return null;
		}
	}

	public class Document {
		public static bool CheckDirectory(string dir) {
			string path = CombinePath (DocumentsPath (), dir);
			return Directory.Exists (path);
		}

		public static void CreateDirectory(string dir) {
			if (CheckDirectory (dir) == false) {
				string path = CombinePath (DocumentsPath (), dir);
				Directory.CreateDirectory (path);
			}
		}

		public static void Write(XmlDocument xmlDocument, string fileName) {
			StringWriter stringWriter = new StringWriter ();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			//xmlTextWriter.Settings.NewLineOnAttributes = true;
			//xmlTextWriter.Settings.Indent = true;
			xmlTextWriter.Formatting = Formatting.Indented;

			xmlDocument.WriteTo (xmlTextWriter);
			Write (stringWriter.ToString(), fileName);
		}

		public static void Write(string data, string fileName) {
			string path = CombinePath (DocumentsPath(), fileName);

			FileStream fileStream = new FileStream (path, FileMode.Create, FileAccess.Write);

			StreamWriter streamWriter = new StreamWriter (fileStream);
			streamWriter.WriteLine (data);
			streamWriter.Close ();

			fileStream.Close ();
		}

		public static string Read(string fileName) {
			string path = CombinePath (DocumentsPath(), fileName);

			if (File.Exists (path) == true) {
				FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

				StreamReader streamReader = new StreamReader(fileStream);
				string data = streamReader.ReadToEnd ();
				streamReader.Close();

				fileStream.Close();
				return data;
			}

			return string.Empty;
		}
	}
}
