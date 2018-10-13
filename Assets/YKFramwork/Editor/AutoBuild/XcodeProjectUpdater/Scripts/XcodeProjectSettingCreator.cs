/********************************************************************
	created:	2016/11/14
	created:	14:11:2016   20:11
	filename: 	C:\Users\Administrator\Desktop\XcodeProjectUpdater-master\Assets\Editor\XcodeProjectUpdater\Scripts\XcodeProjectSettingCreator.cs
	file path:	C:\Users\Administrator\Desktop\XcodeProjectUpdater-master\Assets\Editor\XcodeProjectUpdater\Scripts
	file base:	XcodeProjectSettingCreator
	file ext:	cs
	author:		Author
	
	purpose:	
*********************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>  </summary>
public class XcodeProjectSettingCreator : MonoBehaviour {
	[MenuItem("Assets/Create/XcodeProjectSetting")]
	public static void CreateAsset()
	{
        
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/XcodeProjectSetting.asset");
		XcodeProjectSetting data = ScriptableObject.CreateInstance<XcodeProjectSetting> ();
		AssetDatabase.CreateAsset(data, path);
		AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}

