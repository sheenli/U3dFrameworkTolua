using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WXConstant
{
    public static Dictionary<int, string> WXAPPIDS = new Dictionary<int, string>()
    {
        { 8002,"wxdb309de7bcf1dacf"},
        { 8003,"wxd9794b410b380783"},
        { 8004,"wxe413c4b2a8ab01e0"}
    };

    public static Dictionary<int, string> TransferWXAPPIDS = new Dictionary<int, string>()
    {
        { 8002,"wx2d325832fd0ccb65"},
        { 8003,"wxd9794b410b380783"},
        { 8004,"wxa29b0a0f661566c9"}
    };
    public static Dictionary<int, string> XLAppID = new Dictionary<int, string>()
    {
        { 8002,"dlpyBaO3xcQLz5Tq"},
        { 8003,"00165c3d2308c9354c2dde83cd3e94f5"},
        { 8004,"pxlRZDLWT2NhFBJz"}
    };

#if UNITY_ANDROID
    public static Dictionary<int, string> UMengAppID = new Dictionary<int, string>()
    {
        { 8002,"5a4353958f4a9d349c00000d"},
        { 8003,"5aa10cd18f4a9d6f41000087"},
        { 8004,"5aa10bbff43e4803c10003a1"}
    };
    
    public static Dictionary<int, string> UMengPushSecret = new Dictionary<int, string>()
    {
        { 8002,"1085e0d77f231efb3b24682c96c531e5"},
        { 8003,"00165c3d2308c9354c2dde83cd3e94f5"},
        { 8004,"22b1dd898ae26202e30366b195ee6eaf"}
    };

#elif UNITY_IOS
    public static Dictionary<int, string> UMengAppID = new Dictionary<int, string>()
    {
        { 8002,"5a4353f3f43e486c3a00000d"},
        { 8003,"5a9e6022f29d98628d0001bd"},
        { 8004,"5a9e5da2f43e4847ac0000df"}
    };

    public static Dictionary<int, string> UMengPushSecret = new Dictionary<int, string>()
    {
        { 8002,"yididxv0a4v7udghqev8pjxvj9dlafwn"},
        { 8003,"zpacingrhrwkn9lid37a6owbrzhcp7cf"},
        { 8004,"ypxblzk5jl3uh8szkawsvuunvii1namb"}
    };
#else
    public static Dictionary<int, string> UMengAppID = new Dictionary<int, string>()
    {
        { 8002,"5a4353958f4a9d349c00000d"},
        { 8003,"5aa10cd18f4a9d6f41000087"},
        { 8004,"5aa10bbff43e4803c10003a1"}
    };

    public static Dictionary<int, string> UMengPushSecret = new Dictionary<int, string>()
    {
        { 8002,"dy954pn4vzvje9ow9vyfezclnxrs8kto"},
        { 8003,"gyhu2c9dephxssgr0cdqwohfv2t1hc3zgyhu2c9dephxssgr0cdqwohfv2t1hc3z"},
        { 8004,"sgb2sdrzovbmnznevb6oaib9x5qtcf53"}
    };
#endif

}
