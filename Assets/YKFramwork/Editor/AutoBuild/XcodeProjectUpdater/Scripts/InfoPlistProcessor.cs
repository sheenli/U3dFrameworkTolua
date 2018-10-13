#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using System.IO;

/// <summary>
/// info.plist
/// </summary>
public static class InfoPlistProcessor {

	//=================================================================================
	//内部
	//=================================================================================

	//info.plist
	private static string GetInfoPlistPath(string buildPath){
		return Path.Combine(buildPath, XcodeProjectSetting.INFO_PLIST_NAME);
	}

	//info.plist
	private static PlistDocument GetInfoPlist(string buildPath){
		string plistPath = GetInfoPlistPath(buildPath);
		PlistDocument plist = new PlistDocument();
		plist.ReadFromFile(plistPath);

		return plist;
	}

	//=================================================================================
	//外部
	//=================================================================================

	/// <summary>
	/// URLスキームの設定。既に登録されていても重複しない
	/// </summary>
	public static void SetURLSchemes(string buildPath, string urlIdentifier, List<string> schemeList){

		PlistDocument plist = GetInfoPlist(buildPath);

		//URL typesを取得、設定されていなければ作成
		PlistElementArray urlTypes;
		if(plist.root.values.ContainsKey(XcodeProjectSetting.URL_TYPES_KEY)){
			urlTypes = plist.root[XcodeProjectSetting.URL_TYPES_KEY].AsArray();
		}
		else{
			urlTypes = plist.root.CreateArray (XcodeProjectSetting.URL_TYPES_KEY);
		}

		//URL types内のitemを取得、設定されていなければ作成
		PlistElementDict itmeDict;
        //if(urlTypes.values.Count == 0){
        //	itmeDict = urlTypes.AddDict ();
        //}
        //else{
        //	itmeDict = urlTypes.values[0].AsDict();
        //}
        itmeDict = urlTypes.AddDict();

        //Document RoleとURL identifierを上書きで設定
        itmeDict.SetString (XcodeProjectSetting.URL_TYPE_ROLE_KEY,  "Editor");
		itmeDict.SetString (XcodeProjectSetting.URL_IDENTIFIER_KEY,  urlIdentifier);

		//URL Schemesを取得、設定されていなければ作成(上書きしたい場合はif無くして、CreateArrayのみでok)
		PlistElementArray schemesArray = itmeDict.CreateArray (XcodeProjectSetting.URL_SCHEMES_KEY);
		if(itmeDict.values.ContainsKey(XcodeProjectSetting.URL_SCHEMES_KEY)){
			schemesArray = itmeDict[XcodeProjectSetting.URL_SCHEMES_KEY].AsArray();
		}
		else{
			schemesArray = itmeDict.CreateArray (XcodeProjectSetting.URL_SCHEMES_KEY);
		}

		//既に設定されているものは一旦除外し、スキームを登録(多重登録防止用)
		for (int i = 0; i < schemesArray.values.Count; i++) {
			schemeList.Remove (schemesArray.values [i].AsString ());
		}

		foreach (string scheme in schemeList) {
			schemesArray.AddString (scheme);
		}

		//plist保存
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}



	/// <summary>
	/// canOpenURLで判定可能にするスキームを登録する
	/// </summary>
	public static void SetApplicationQueriesSchemes(string buildPath, List<string> _applicationQueriesSchemes){
		PlistDocument plist = GetInfoPlist (buildPath);

		//LSApplicationQueriesSchemes 白名单
		PlistElementArray queriesSchemes;
		if(plist.root.values.ContainsKey(XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY)){
			queriesSchemes = plist.root[XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY].AsArray();
		}
		else{
			queriesSchemes = plist.root.CreateArray (XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY);
		}

		//既に設定されていなければスキームを登録
		foreach (string queriesScheme in _applicationQueriesSchemes) {
			if(!queriesSchemes.values.Contains(new PlistElementString(queriesScheme))){
				queriesSchemes.AddString(queriesScheme);
			}
		}

		//plist保存
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}

	/// <summary>
	/// デフォルトで設定されているスプラッシュ画像の設定を消す
	/// </summary>
	public static void DeleteLaunchiImagesKey(string buildPath){
		PlistDocument plist = GetInfoPlist (buildPath);

		//keyが存在していれば削除
		if(plist.root.values.ContainsKey(XcodeProjectSetting.UI_LAUNCHI_IMAGES_KEY)){
			plist.root.values.Remove (XcodeProjectSetting.UI_LAUNCHI_IMAGES_KEY);
		}
		if(plist.root.values.ContainsKey(XcodeProjectSetting.UI_LAUNCHI_STORYBOARD_NAME_KEY)){
			plist.root.values.Remove (XcodeProjectSetting.UI_LAUNCHI_STORYBOARD_NAME_KEY);
		}

		//plist保存
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}

	/// <summary>
	/// ATSを有効or無効にする
	/// </summary>
	public static void SetATS(string buildPath, bool enableATS){
		PlistDocument plist = GetInfoPlist (buildPath);

		//ATSの設定
		PlistElementDict atsDict = plist.root.CreateDict (XcodeProjectSetting.ATS_KEY);
		atsDict.SetBoolean (XcodeProjectSetting.ALLOWS_ARBITRARY_LOADS_KEY, enableATS);

		//plist保存
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}

	/// <summary>
	/// ステータスバーの表示設定
	/// </summary>
	public static void SetStatusBar(string buildPath, bool enable){
		PlistDocument plist = GetInfoPlist (buildPath);

		//ステータスバーの表示設定
		plist.root.SetBoolean (XcodeProjectSetting.STATUS_HIDDEN_KEY,        enable);
		plist.root.SetBoolean (XcodeProjectSetting.STATUS_BAR_APPEARANCE_KEY, !enable);

		//plist保存
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}

	public static void AddStringKey(string buildPath , string key ,string value)
	{
		PlistDocument plist = GetInfoPlist (buildPath);
		plist.root.SetString (key,value);
		plist.WriteToFile(GetInfoPlistPath(buildPath));
	}
}
#endif