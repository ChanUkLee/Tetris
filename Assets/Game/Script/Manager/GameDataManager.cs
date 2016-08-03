using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using GameEnum;
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
						XmlNodeList directionNodeList = null;
						XmlNode directionNode = null;
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

								directionNodeList = node.SelectNodes ("direction");
								if (directionNodeList != null) {
									for (int j = 0; j < directionNodeList.Count; j++) {
										directionNode = directionNodeList [j];
										if (directionNode != null) {
											DIRECTION direction = DIRECTION.NULL;

											attribute = directionNode.Attributes.GetNamedItem ("type");
											if (attribute != null) {
												direction = GameEnumConvert.ToDirection (attribute.Value);

												if (data._direction.ContainsKey (direction) == false) {
													data._direction.Add (direction, new List<Vector3> ());

													innerNodeList = directionNode.SelectNodes("tile");
													if (innerNodeList != null)
													{
														for (int k = 0; k < innerNodeList.Count; k++)
														{
															innerNode = innerNodeList[k];
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

																data._direction [direction].Add (blockPos);
															}
														}
													}
												} else {
													GameDebug.Error ("key exception");
												}
											}
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