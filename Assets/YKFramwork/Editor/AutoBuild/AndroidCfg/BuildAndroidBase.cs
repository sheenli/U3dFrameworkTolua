
using UnityEditor;
using UnityEngine;

public class BuildCfgBase : IBuildCfg
{
    public override string[] BuildScenes
    {
        get
        {
            return ProjectBuild.GetBuildScenes;
        }

        set
        {
            
        }
    }

    public override BuildOptions BuildOptions { get
        { return BuildOptions.None; } set { } }

    public override void BuildAfter(string buildPath)
    {
        Debug.LogError("生成完成");
    }

    public override void BuildBefore()
    {
        PlayerSettings.SetApplicationIdentifier(UnityEditor.BuildTargetGroup.iOS, "com.youke.test");
        PlayerSettings.SetApplicationIdentifier(UnityEditor.BuildTargetGroup.Android, "com.youke.test");
        Debug.LogError("开始生成");
    }
}
[BuildCfg(UnityEditor.BuildTarget.iOS, -1,-1,100)]
public class BuildCfgIOSBase : BuildCfgBase
{
    public override void BuildAfter(string buildPath)
    {
        base.BuildAfter(buildPath);
    }
}

[BuildCfg(UnityEditor.BuildTarget.Android, -1,-1, 100)]
public class BuildCfgAndroidBase: BuildCfgBase
{
    public override void BuildAfter(string buildPath)
    {
        base.BuildAfter(buildPath);
    }
    public override void BuildBefore()
    {
        base.BuildBefore();
        PlayerSettings.Android.useAPKExpansionFiles = false;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
    }
}
