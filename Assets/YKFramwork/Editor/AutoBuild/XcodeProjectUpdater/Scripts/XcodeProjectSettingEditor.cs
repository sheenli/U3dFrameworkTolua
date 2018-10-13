
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(XcodeProjectSetting))]
public class XcodeProjectSettingEditor : Editor
{
    public XcodeProjectSetting Instance
    {
        get
        {
            return target as XcodeProjectSetting;
        }
    }
    private bool mFrameworkListFlag = true;

    private bool mLinkerFlagArrayFlag = true;

    private bool mCompilerFlagsSetListFlag = true;

    private bool mApplicationQueriesSchemesFlag = true;

    private bool mURLIdentifierDicFlag = true;

    private bool AddCodeListFlag = true;

    private bool mAddLibsFlag = true;

	private bool mAddToPlistStringKey = true;

    private bool mCapabilitysFlag = true;

    public override void OnInspectorGUI()
    {
        GUIStyle textAreaWordWrap = new GUIStyle(EditorStyles.textArea);
        textAreaWordWrap.wordWrap = true;
        EditorGUILayout.BeginVertical();
        {

#region 目录相关
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("要拷贝的文件根目录：" + Instance.CopyDirectoryPath);
                if (GUILayout.Button(" 选择 "))
                {
                    string path = UnityEditor.EditorUtility.OpenFolderPanel("选择根目录", Instance.CopyDirectoryPath, "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        Instance.CopyDirectoryPath = FileUtil.GetProjectRelativePath(path);
                    }
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
#endregion

#region FrameworkLists
            GUILayout.Space(10);
            mFrameworkListFlag = EditorGUILayout.Foldout(mFrameworkListFlag, "所有要添加到工程里面的Frameworks");
            GUILayout.Space(5);
            if (mFrameworkListFlag)
            {
                for (int i = 0; i < Instance.FrameworkList.Count; i++)
                {
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("{0}.", i));
                        Instance.FrameworkList[i] = EditorGUILayout.TextField(Instance.FrameworkList[i]);
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.FrameworkList.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.FrameworkList.Add("");
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion

#region Libs
            GUILayout.Space(10);
            mAddLibsFlag = EditorGUILayout.Foldout(mAddLibsFlag, "所有要添加到工程里面的Libs");
            GUILayout.Space(5);
            if (mAddLibsFlag)
            {
                for (int i = 0; i < Instance.LibList.Count; i++)
                {

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("{0}.", i));
                        Instance.LibList[i] = EditorGUILayout.TextField(Instance.LibList[i]);
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.LibList.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.LibList.Add("");
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion

#region 所有链接编译符
            GUILayout.Space(10);
            mLinkerFlagArrayFlag = EditorGUILayout.Foldout(mLinkerFlagArrayFlag, "所有链接编译符");
            GUILayout.Space(5);
            if (mLinkerFlagArrayFlag)
            {
                for (int i = 0; i < Instance.LinkerFlagArray.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("{0}.", i));
                        Instance.LinkerFlagArray[i] = EditorGUILayout.TextField(Instance.LinkerFlagArray[i]);
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.LinkerFlagArray.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.LinkerFlagArray.Add("");
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion

#region 需要导出的文件以及文件需要特殊的flags
            GUILayout.Space(10);
            mCompilerFlagsSetListFlag = EditorGUILayout.Foldout(mCompilerFlagsSetListFlag, "需要导出的文件以及文件需要特殊的flags");
            GUILayout.Space(5);
            if (mCompilerFlagsSetListFlag)
            {
                for (int i = 0; i < Instance.CompilerFlagsSetList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("连接符{0}:", i));
                        Instance.CompilerFlagsSetList[i].Flags = EditorGUILayout.TextField(Instance.CompilerFlagsSetList[i].Flags);
                        GUI.color = Color.cyan;
                        if (GUILayout.Button("Add"))
                        {
                            string path = EditorUtility.OpenFilePanel("选择要添加的文件", Instance.CopyDirectoryPath,"*.*");
                            if (!string.IsNullOrEmpty(path))
                            {
                                path = FileUtil.GetProjectRelativePath(path).Replace(Instance.CopyDirectoryPath+"/","");
                                if (!path.EndsWith(".meta") && !Instance.CompilerFlagsSetList[i].TargetPathList.Contains(path))
                                {
                                    Instance.CompilerFlagsSetList[i].TargetPathList.Add(path);
                                }
                            }
                        }
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.CompilerFlagsSetList.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    for (int j = 0;j< Instance.CompilerFlagsSetList[i].TargetPathList.Count;j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(50);
                            GUILayout.Label(string.Format("文件{0}：", j));
                            Instance.CompilerFlagsSetList[i].TargetPathList[j] = EditorGUILayout.TextField(Instance.CompilerFlagsSetList[i].TargetPathList[j]);
                            GUI.color = Color.red;
                            if (GUILayout.Button("X"))
                            {
                                Instance.CompilerFlagsSetList[i].TargetPathList.RemoveAt(j);
                            }
                            GUI.color = Color.white;
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.CompilerFlagsSetList.Add(new XcodeProjectSetting.CompilerFlagsSet("",new System.Collections.Generic.List<string>()));
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion
            
#region 白名单设置 
            GUILayout.Space(10);
            mApplicationQueriesSchemesFlag = EditorGUILayout.Foldout(mApplicationQueriesSchemesFlag, "白名单设置 ApplicationQueriesSchemes");
            GUILayout.Space(5);
            if (mApplicationQueriesSchemesFlag)
            {
                for (int i = 0; i < Instance.ApplicationQueriesSchemes.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("{0}.", i));
                        Instance.ApplicationQueriesSchemes[i] = EditorGUILayout.TextField(Instance.ApplicationQueriesSchemes[i]);
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.ApplicationQueriesSchemes.RemoveAt(i);
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.ApplicationQueriesSchemes.Add("");
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion

#region 需要导出的文件以及文件需要特殊的flags
            GUILayout.Space(10);
            mURLIdentifierDicFlag = EditorGUILayout.Foldout(mURLIdentifierDicFlag, "URLidentifier名字 以及每个的内容");
            GUILayout.Space(5);
            if (mURLIdentifierDicFlag)
            {
                for (int i = 0; i < Instance.URLIdentifierList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("CFBundleURLName{0}:", i));
                        Instance.URLIdentifierList[i].name = EditorGUILayout.TextField(Instance.URLIdentifierList[i].name);

                        GUI.color = Color.cyan;
                        if (GUILayout.Button("Add"))
                        {
                            Instance.URLIdentifierList[i].URLSchemes.Add("");
                        }GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.URLIdentifierList.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    for (int j = 0; j < Instance.URLIdentifierList[i].URLSchemes.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(50);
                            GUILayout.Label(string.Format("URLScheme{0}：", j));
                            Instance.URLIdentifierList[i].URLSchemes[j] = EditorGUILayout.TextField(Instance.URLIdentifierList[i].URLSchemes[j]);
                            GUI.color = Color.red;
                            if (GUILayout.Button("X"))
                            {
                                Instance.URLIdentifierList[i].URLSchemes.RemoveAt(j);
                            }
                            GUI.color = Color.white;
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个")) 
                    {
                        Instance.URLIdentifierList.Add(new XcodeProjectSetting.URLIdentifierData());
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion

#region 需要特殊添加到plist里面到key

			GUILayout.Space(10);
			mAddToPlistStringKey = EditorGUILayout.Foldout(mAddToPlistStringKey, "需要特殊添加到Plist到KEY");
			GUILayout.Space(5);
			if (mAddToPlistStringKey)
			{
				for (int i = 0; i < Instance.addStringKeyToPlist.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					{
						GUILayout.Label("需要添加的key:");
						Instance.addStringKeyToPlist[i].Key = EditorGUILayout.TextField(Instance.addStringKeyToPlist[i].Key);
						GUILayout.Label("需要添加的Value:");
						Instance.addStringKeyToPlist[i].value = EditorGUILayout.TextField(Instance.addStringKeyToPlist[i].value);
						GUI.color = Color.red;
						if (GUILayout.Button("X"))
						{
							Instance.addStringKeyToPlist.RemoveAt(i);
							//GUILayout.EndHorizontal();
							break;
						}
						GUI.color = Color.white; 
						GUILayout.FlexibleSpace();
					}
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(2);
				}

				GUILayout.Space(3);
				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Space(10);
					GUI.color = Color.green;
					if (GUILayout.Button("添加一个"))
					{
						Instance.addStringKeyToPlist.Add(new XcodeProjectSetting.AddToInfoStringList());
					}
					GUI.color = Color.white;
					GUILayout.Space(10);
				}
				EditorGUILayout.EndHorizontal();
			}
#endregion

#region 添加代码相关

//             GUILayout.Space(10);
//             EditorGUILayout.BeginHorizontal();
//             {
//                 GUILayout.Label("在UnityAppController开头要加入的代码:");
//                 GUI.color = new Color(255, 255, 0);
//                 Instance.UnityAppControllerStartAddCode = GUILayout.TextArea(Instance.UnityAppControllerStartAddCode);
//                 GUI.color = Color.white;
//                 GUILayout.FlexibleSpace();
//             }
//             EditorGUILayout.EndHorizontal();
#endregion

#region 需要添加的插入代码的列表
            GUILayout.Space(10);
            AddCodeListFlag = EditorGUILayout.Foldout(AddCodeListFlag, "需要添加的插入代码的列表");
            GUILayout.Space(5);
            if (AddCodeListFlag)
            {
                for (int i = 0; i < Instance.AddCodeList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("文件名:");
                        Instance.AddCodeList[i].FileName = EditorGUILayout.TextField(Instance.AddCodeList[i].FileName, GUILayout.MinWidth(150));
                        GUI.color = Color.cyan;
                        if (GUILayout.Button("添加一个追加"))
                        {
                            Instance.AddCodeList[i].addCodes.Add(new XcodeProjectSetting.AddCodeSet());
                        }
                        if (GUILayout.Button("添加一个替换"))
                        {
                            Instance.AddCodeList[i].replaceCode.Add(new XcodeProjectSetting.ReplaceCodeSet());
                        }
                        GUI.color = Color.red;
                        if (GUILayout.Button("X"))
                        {
                            Instance.AddCodeList.RemoveAt(i);
                            break;
                        }
                        GUI.color = Color.white;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    GUILayout.Label("----------------------需要在开头添加的内容--------------------");
                    GUI.color = new Color(255, 255, 0);
                    Instance.AddCodeList[i].StartAddCode = EditorGUILayout.TextArea(Instance.AddCodeList[i].StartAddCode, textAreaWordWrap, GUILayout.MinWidth(150));
                    GUI.color = Color.white;
                    GUILayout.Space(5);
                    GUILayout.Label("----------------------需要追加的内容--------------------");
                    for (int j = 0; j < Instance.AddCodeList[i].addCodes.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(50);
                            GUILayout.Label("要插入代码的前一行:");
                            GUI.color = new Color(255, 255, 0);
                            Instance.AddCodeList[i].addCodes[j].addFlag = EditorGUILayout.TextArea(Instance.AddCodeList[i].addCodes[j].addFlag, textAreaWordWrap, GUILayout.MinWidth(150));
                            GUI.color = Color.white;
                            GUILayout.Label("要插入的代码:");
                            GUI.color = new Color(255, 255, 0);
                            Instance.AddCodeList[i].addCodes[j].AddCode = EditorGUILayout.TextArea(Instance.AddCodeList[i].addCodes[j].AddCode, textAreaWordWrap, GUILayout.MinWidth(150));
                            

                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.Width(50), GUILayout.ExpandHeight(false)))
                            {
                                Instance.AddCodeList[i].addCodes.RemoveAt(j);
                                break;
                            }
                            GUI.color = Color.white;
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            
                            GUILayout.Space(50);
                            
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                    GUILayout.Space(5);
                    GUILayout.Label("----------------------需要替换的内容--------------------");
                    for (int j = 0; j < Instance.AddCodeList[i].replaceCode.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(50);
                            GUILayout.Label("替换前的内容:");
                            GUI.color = new Color(255, 255, 0);
                            
                            Instance.AddCodeList[i].replaceCode[j].oldCode = EditorGUILayout.TextArea(Instance.AddCodeList[i].replaceCode[j].oldCode, textAreaWordWrap, GUILayout.MaxWidth(150));
                            GUI.color = Color.white;

                            GUILayout.Label("替换后的内容:");
                            GUI.color = new Color(255, 255, 0);
                            Instance.AddCodeList[i].replaceCode[j].newCode = EditorGUILayout.TextArea(Instance.AddCodeList[i].replaceCode[j].newCode, textAreaWordWrap, GUILayout.MaxWidth(150));
                            
                            GUI.color = Color.white;

                            GUI.color = Color.red;
                            if (GUILayout.Button("X",GUILayout.Width(50), GUILayout.ExpandHeight(false)))
                            {
                                Instance.AddCodeList[i].replaceCode.RemoveAt(j);
                                break;
                            }
                            GUI.color = Color.white;
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                    GUILayout.Space(2);
                }

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(10);
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加一个"))
                    {
                        Instance.AddCodeList.Add(new XcodeProjectSetting.AddCodeData());
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(10);
                }
                EditorGUILayout.EndHorizontal();
            }
#endregion

            GUILayout.Space(10);
            Instance.EnableBitCode = EditorGUILayout.Toggle("EnableBitCode", Instance.EnableBitCode);
            Instance.EnableATS = EditorGUILayout.Toggle("NSAppTransportSecurity", Instance.EnableATS);
            Instance.EnableStatusBar = EditorGUILayout.Toggle("EnableStatusBar", Instance.EnableStatusBar);
            Instance.NeedToDeleteLaunchiImagesKey = EditorGUILayout.Toggle("NeedToDeleteLaunchiImagesKey", Instance.NeedToDeleteLaunchiImagesKey);
            GUILayout.Space(10);

            GUI.color = Color.green;

#if UNITY_IPHONE
            if (GUILayout.Button("生成版本", GUILayout.Height(80), GUILayout.ExpandWidth(true)))
            {
				string path = EditorUtility.SaveFolderPanel("选择你要保存路径", Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    PlayerPrefs.SetString(XcodeProjectUpdater.SETTING_DATA_PATHKEY, AssetDatabase.GetAssetPath(Instance));
                    PlayerPrefs.Save();
                    BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.iOS, BuildOptions.None);
                }
            }
#endif
            GUI.color = Color.white;
        }



        EditorGUILayout.EndVertical();
        serializedObject.Update();
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        //base.OnInspectorGUI();
    }

    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }
}