using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using GameData;
using GameFile;

public class GameDataManager : Singleton<GameDataManager> {
    private const string PATH = "Xml/Data";

    private List<BlockData> _blockList = new List<BlockData>();

    public void LoadBlockData()
    {
        this._blockList.Clear();
        string path = Path.Combine(PATH, "block");

        string output = string.Empty;
        if (Resource.Read(path, out output) == true)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(output);

            try
            {
                XmlNode rootNode = xmlDocument.DocumentElement;
                if (rootNode != null)
                {
                    XmlNodeList nodeList = rootNode.SelectNodes("block");
                    if (nodeList != null)
                    {
                        XmlNode node = null;
                        XmlNode attribute = null;
                        XmlNodeList innerNodeList = null;
                        XmlNode innerNode = null;

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            node = nodeList[i];
                            if (node != null)
                            {
                                BlockData data = new BlockData();
                                data.Init();

                                attribute = node.Attributes.GetNamedItem("id");
                                if (attribute != null)
                                {
                                    int.TryParse(attribute.Value, out data._id);
                                }

                                attribute = node.Attributes.GetNamedItem("name");
                                if (attribute != null)
                                {
                                    data._name = attribute.Value;
                                }

                                attribute = node.Attributes.GetNamedItem("enable_direction");
                                if (attribute != null)
                                {
                                    data._enableDirection = System.Convert.ToBoolean(attribute.Value);
                                }

                                innerNodeList = node.SelectNodes("tile");
                                if (innerNodeList != null)
                                {
                                    for (int j = 0; j < innerNodeList.Count; j++)
                                    {
                                        innerNode = innerNodeList[j];
                                        if (innerNode != null)
                                        {
                                            Vector3 blockPos = Vector3.zero;

                                            attribute = innerNode.Attributes.GetNamedItem("x");
                                            if (attribute != null)
                                            {
                                                float.TryParse(attribute.Value, out blockPos.x);
                                            }

                                            attribute = innerNode.Attributes.GetNamedItem("y");
                                            if (attribute != null)
                                            {
                                                float.TryParse(attribute.Value, out blockPos.y);
                                            }

                                            data._blockList.Add(blockPos);
                                        }
                                    }
                                }

                                this._blockList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (XmlException exception)
            {
                GameDebug.Error(exception.Message);
            }
        }
    }

    public List<BlockData> GetBlockList()
    {
        return this._blockList;
    }
}