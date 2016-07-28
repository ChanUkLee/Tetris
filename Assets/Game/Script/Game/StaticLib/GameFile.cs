using UnityEngine;
using System.IO;
using System.Collections;

namespace GameFile
{
    public static class Resource
    {
        public static bool Find()
        {
            return false;
        }

        public static bool Read(string src, out string output)
        {
            output = null;
            TextAsset textAsset = Resources.Load(src) as TextAsset;
            if (textAsset != null)
            {
                output = textAsset.text;
                return true;
            }

            return false;
        }

        public static bool Read(string src, out byte[] output)
        {
            output = null;

            TextAsset textAsset = Resources.Load(src) as TextAsset;
            if (textAsset != null)
            {
                Stream stream = new MemoryStream(textAsset.bytes);
                BinaryReader reader = new BinaryReader(stream);

                reader.BaseStream.Position = 0;

                int length = System.Convert.ToInt32(reader.BaseStream.Length);
                output = reader.ReadBytes(length);
                return true;
            }

            return false;
        }
    }
}
