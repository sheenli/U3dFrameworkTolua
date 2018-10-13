using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class CreateLua : MonoBehaviour {

    [MenuItem("Assets/Create/Lua Script", false, 80)]
    public static void CreatNewLua()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
        ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(),
        GetSelectedPathOrFallback() + "/New Lua.lua",
        null,"Assets/YKFramwork/Editor/LuaTemplate/lua.lua");
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
class MyDoCreateScriptAsset : EndNameEditAction
{


    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        //string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        //text = Regex.Replace(text, "#NAME#", fileNameWithoutExtension);
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(pathName, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }

}