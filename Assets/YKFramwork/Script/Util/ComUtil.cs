using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using zlib;

public class ComUtil
{
    #region 节点创建

    /// <summary>
    /// 创建或者获取组件，如果target对象已存在组件，直接返回，否则增加组件后返回组件
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T CreateOrGetComponent<T>(GameObject target) where T : Component
    {
        if (null == target)
        {
            return null;
        }

        T comp = target.GetComponent<T>();
        if (null == comp)
        {
            comp = target.AddComponent<T>();
        }

        return comp;
    }



    /// <summary>
    /// 把transChild加到transParent里，并把child的Transform的localPosition,localRotation,localScale的值重置
    /// </summary>
    /// <param name="transChild"></param>
    /// <param name="transParent"></param>
    public static void AddGameObjectToZeroPos(Transform transChild, Transform transParent)
    {
        if (null == transChild || null == transParent)
        {
            return;
        }

        transChild.parent = transParent;
        transChild.localPosition = Vector3.zero;
        transChild.localRotation = Quaternion.identity;
        transChild.localScale = Vector3.one;
    }

    /// <summary>
    /// 把transChild加到transParent里，使得transChild为parentObj的孩子
    /// 会重新计算孩子的位置
    /// </summary>
    public static void AddGameObjectTo(Transform transChild, Transform transParent)
    {
        if (null == transChild || null == transParent || transChild.parent == transParent)
        {
            return;
        }

        transChild.parent = transParent;
        Transform trans = transParent;
        Vector3 scaleParam = new Vector3();
        while (null != trans)
        {
            scaleParam = trans.localScale;
            scaleParam.Scale(transChild.localPosition);
            transChild.localPosition = trans.localPosition + scaleParam;
            transChild.localRotation = trans.localRotation * transChild.localRotation;

            scaleParam = transChild.localScale;
            scaleParam.Scale(trans.localScale);
            transChild.localScale = scaleParam;

            trans = trans.parent;
        }
    }

    #endregion

    #region GetUTF8StringEX

    private static StringBuilder strBuilder = new StringBuilder(4048);

    /// <summary>
    /// 非线程安全
    /// </summary>
    /// <returns>
    /// The UT f8 string E.
    /// </returns>
    /// <param name='buffer'>
    /// Buffer.
    /// </param>
    public static string GetUTF8StringTrimZero(byte[] buffer)
    {
        if (null == buffer || 0 == buffer.Length)
        {
            return "";
        }
        strBuilder.Remove(0, strBuilder.Length);
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] != 0)
            {
                strBuilder.Append((char)buffer[i]);
            }
            else
            {
                break;
            }
        }
        return strBuilder.ToString();
    }

    #endregion

    #region 文本处理

    /// <summary>
    /// replace all "\n" with '\n'
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string GetVerticalString(string val)
    {
        string ret = null;
        if (null != val)
        {
            string n = "" + '\n';
            ret = val.Replace("[/n]", n);
        }
        return ret;
    }

    #endregion

    #region 节点查找

    public static bool IsSubClassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    public static bool IsSubClassOf(Type baseType, Type toCheck)
    {
        return baseType.IsAssignableFrom(toCheck);
    }

    /// <summary>
    /// 用于从一个节点开始将其与其下所有T类型的并继承自Component的返回回来
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="root"></param>
    /// <param name="enable"></param>
    /// <returns></returns>
    public static List<T> FindAllComponentsUnderRoot<T>(Transform root) where T : Component
    {
        List<T> retList = new List<T>();

        if (root.childCount != 0)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                retList.AddRange(FindAllComponentsUnderRoot<T>(root.GetChild(i)));
            }
        }

        T t = root.GetComponent<T>();
        if (t != null)
        {
            retList.Add(t);
        }
        return retList;

    }

    #endregion

    #region 节点排序

    /// <summary>
    /// 排序root下tag不为tag的节点
    /// </summary>
    /// <param name="root"></param>
    /// <param name="tag"></param>
    public static List<Transform> SortRoutePointExcludeTag(GameObject root, string tag)
    {
        return SortRoutePoint(root, tag, true);
    }

    /// <summary>
    /// 排序root下tag为tag的节点
    /// </summary>
    /// <param name="root"></param>
    /// <param name="tag"></param>
    public static List<Transform> SortRoutePointWithTag(GameObject root, string tag)
    {
        return SortRoutePoint(root, tag, false);
    }

    private static List<Transform> SortRoutePoint(GameObject root, string tag, bool isExclude)
    {
        ArrayList routePointNames = new ArrayList();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            Transform trans = root.transform.GetChild(i);
            bool isAdd = isExclude ? trans.tag != tag : trans.tag == tag;
            if (isAdd)
            {
                routePointNames.Add(trans.name);
            }
        }
        routePointNames.Sort();

        List<Transform> ret = new List<Transform>();
        foreach (string na in routePointNames)
        {
            for (int i = 0; i < root.transform.childCount; i++)
            {
                Transform trans = root.transform.GetChild(i);
                if (trans.name == na)
                {
                    ret.Add(trans);
                    break;
                }
            }
        }
        return ret;
    }

    #endregion

    #region 通过指定路径获取组件
    /// <summary>通过指定路径获取组件</summary>
    public static T GetComponentByLocalPath<T>(GameObject go, string localPath) where T : Component
    {
        GameObject obj = null;
        if (go == null)
        {
            obj = GameObject.Find(localPath) as GameObject;
        }
        else if (go.transform.childCount > 0)
        {
            Transform tran = go.transform.Find(localPath);
            if (tran != null)
            {
                obj = tran.gameObject;
            }
        }
        if (obj == null)
        {
            Debug.LogWarning(string.Format("GetComponentByLocalPath Null={0}", localPath));
            return null;
        }
        else
        {
            return obj.GetComponent<T>();
        }
    }
    #endregion

    #region 通过指定路径获取游戏物体
    /// <summary>通过指定路径获取组件</summary>
    public static GameObject FindGameObjectByLocalPath(GameObject go, string localPath)
    {
        GameObject obj = null;
        if (go == null)
        {
            obj = GameObject.Find(localPath) as GameObject;
        }
        else if (go.transform.childCount > 0)
        {
            Transform tran = go.transform.Find(localPath);
            if (tran != null)
            {
                obj = tran.gameObject;
            }
        }
        if (obj == null)
        {
            Debug.LogWarning(string.Format("GetComponentByLocalPath Null={0}", localPath));
            return null;
        }
        else
        {
            return obj;
        }
    }
    #endregion

    #region 随机数
    /// <summary>
    /// 返回一个0 - 1之间的随机数,参数是种子
    /// </summary>
    /// <returns></returns>
    public static float GetRandIn0_1(int seed = 3)
    {
        //UnityEngine.Random.seed = seed;
        return UnityEngine.Random.value;
    }
    #endregion

    #region 根据名字获取该游戏对象下子物体

    /// <summary>
    ///  根据名字获取该游戏对象下子物体
    /// </summary>
    /// <param name="tan">主要对象</param>
    /// <param name="name">当前的名字</param>
    /// <returns></returns>
    public static Transform FindTransformInChild(Transform tan, string name, bool oneLevel = false)
    {
        //tan.gameObject.transform.localEulerAngles = new Vector3(0,0,0);
        List<Transform> ret = FindTransformInChild(tan, new List<string>() { name });
        if (ret.Count > 0)
        {
            return ret[0];
        }
        return null;
    }

    public static List<string> GetOneListStr()
    {
        return new List<string>();
    }
    public static List<Transform> GetOneListTran()
    {
        return new List<Transform>();
    }
    /// <summary>
    /// 根据名字获取该游戏对象下子物体并根据列表进行排序
    /// </summary>
    /// <param name="tan"></param>
    /// <param name="names"></param>
    /// <param name="oneLevel"></param>
    /// <returns></returns>
    public static List<Transform> FindTransformInChildSort(Transform tan, List<string> names, bool oneLevel = true)
    {
        var list = FindTransformInChild(tan, names, oneLevel);
        //list.Count
        list.Sort((a,b)=> 
        {
            int aIndex = names.FindIndex(x => x == a.name);
            int bIndex = names.FindIndex(x => x == b.name);
            return aIndex.CompareTo(bIndex);
        });

        return list;
    }

    /// <summary>
    /// 获取物体下该名字的物体
    /// </summary>
    /// <param name="tan"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<Transform> FindTransformNameAll(Transform tan,string name)
    {
        List<Transform> list = new List<Transform>();
        var all = tan.GetComponentsInChildren<Transform>(true);
        foreach(Transform tran in all)
        {
            if(tran.name == name)
            {
                list.Add(tran);
            }
        }
        return list;
    }
    /// <summary>
    /// 根据名字获取该游戏对象下子物体
    /// </summary>
    /// <param name="tan"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    public static List<Transform> FindTransformInChild(Transform tan, List<string> names, bool oneLevel = false)
    {
        List<Transform> ret = new List<Transform>();

        if (tan == null)
        {
            Log.Error("ComUtil.FindTransformInChild -> tan Is Null");
            return ret;
        }

        if (tan.childCount != 0)
        {
            for (int i = 0; i < tan.childCount; i++)
            {
                if (oneLevel)
                {
                    if (names.Contains(tan.GetChild(i).name))
                    {
                        ret.Add(tan);
                    }
                }
                else
                {
                    ret.AddRange(FindTransformInChild(tan.GetChild(i), names));
                }
            }
        }

        if (names.Contains(tan.name))
        {
            ret.Add(tan);
        }

        return ret;
    }

    #endregion

    #region 根据名字获取该游戏对象下子物体的组件
    /// <summary>
    ///  根据名字获取该游戏对象下子物体
    /// </summary>
    /// <param name="tan">主要对象</param>
    /// <param name="name">当前的名字</param>
    /// <returns></returns>
    public static T FindComponentInChild<T>(Transform tan, string name) where T : Component
    {
        List<Transform> ret = FindTransformInChild(tan, new List<string>() { name });
        if (ret.Count > 0)
        {
            return ret[0].GetComponent<T>();
        }
        return null;
    }


    #endregion

    #region 获取物体下所有活动的物体
    public static List<Transform> FindTransformAllActiveChild(Transform tran)
    {
        List<Transform> list = new List<Transform>();
        var all = tran.GetComponentInChildren<Transform>();
        foreach(Transform t in all)
        {
            if (t.gameObject.activeSelf)
            {
                list.Add(t);
            }
        }
        return list;
    }
    #endregion

    #region 时间戳
    /// <summary>
    /// 时间戳转换成时间
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static string TimestampToTime(long unixTimeStamp)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        DateTime dt = startTime.AddSeconds(unixTimeStamp);
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }
    #endregion

    /// <summary>
    /// 获取数字位数
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetNumberLen(int number)
    {
        int tmp = number;
        int ret = 0;
        while (tmp > 0)
        {
            ret += 1;
            tmp /= 10;
        }
        return ret;
        
    }

    /// <summary>
    /// 寻找自物体下的游戏物体
    /// </summary>
    /// <param name="listName">要寻找的物体列表</param>
    /// <param name="tran">父对象</param>
    /// <param name="trans">返回的结果</param>
    public static void GetTransformInChilds(List<string> listName, Transform tran, ref List<Transform> trans)
    {
        if (listName.Contains(tran.name))
        {
            trans.Add(tran);
        }
        if (trans.Count == listName.Count)
        {
            return;
        }
        for (int i = 0; i < tran.childCount; i++)
        {
            GetTransformInChilds(listName, tran.GetChild(i), ref trans);
        }
    }


    /// <summary>
    /// HTTP Get请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="param"></param>
    /// <param name="encode"></param>
    /// <returns></returns>
    public static string HTTPGetData(string url, Encoding encode)
    {
        if (!string.IsNullOrEmpty(url))
        {

        }
        var result = "";
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=gb2312";

            var response = (HttpWebResponse)request.GetResponse();
            var rs = response.GetResponseStream();
            var sr = new StreamReader(rs, encode);
            result = sr.ReadToEnd();
            sr.Close();
            rs.Close();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        return result;
    }

    #region 扑克锚点
    /// <summary> 扑克锚点 </summary>
    public static float GetPosX(float tw, int allNum, int index, float thresholdMin = 0.23f, float thresholdMax = 0.8f)
    {
       
        //float test = tw * ((float)1080 / (float)640);
        float w = Screen.width / ((float)Screen.height / (float)720); 
        return GetFixedSizeIndexPos(w, tw, allNum, index, thresholdMin, thresholdMax);
    }

    /// <summary>
    /// 根据固定的大小算出某个指定单元要放的位置
    /// </summary>
    /// <param name="size">总大小</param>
    /// <param name="unitSize">单元格的大小</param>
    /// <param name="allNum">总数量</param>
    /// <param name="index">第几个</param>
    /// <param name="thresholdMin">最小间隙</param>
    /// <param name="thresholdMax">最大间隙</param>
    /// <returns></returns>
    public static float GetFixedSizeIndexPos(float size,float unitSize, int allNum, int index, float thresholdMin = 0.23f, float thresholdMax = 0.8f)
    {
        float offset = (size - unitSize) / (allNum );
        float minOffset = (unitSize * thresholdMin);
        float maxOffset = (unitSize * thresholdMax);
        offset = offset > maxOffset ? maxOffset : offset;
//         offset = offset < minOffset ? minOffset : offset;
        float satrtPos = -((allNum - 1) * (offset / 2));
        return satrtPos + offset * index;
    }

    /// <summary>
    /// 获取这个游戏物体为空
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool GetActive(MonoBehaviour mb)
    {
        return mb && mb.enabled && mb.gameObject.activeInHierarchy;
    }
    #endregion

    #region 获取地址

    /// <summary>
    /// 获取地址 根据ip
    /// </summary>
    /// <param name="ip">ip</param>
    /// <returns></returns>
    public static void GetAddressByIp(string ip,Action<string> callBack)
    {
        string url = string.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", ip);
        Action<string> wwwFinished = str =>
        {
            string Address = "";
            if (!string.IsNullOrEmpty(str))
            {
                str = UniconToString(str);
                AddressResultData _AddressInfo = JsonUtility.FromJson<AddressResultData>(str);
                if (_AddressInfo.code == 0)
                {
                    Address += _AddressInfo.data.region;
                    Address += _AddressInfo.data.city;
                }
            }
            if (callBack != null)
            {
                callBack(Address);
            }
        };
        ResMgr.Intstance.StartCoroutine(GetWWWGetData(url, wwwFinished));
    }

    public static IEnumerator GetWWWGetData(string url, Action<string> callBack)
    {
        

        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (callBack != null)
            {
                callBack(www.text);
            }
        }
        else
        {
            if (callBack != null)
            {
                callBack("");
            }
        }
        www.Dispose();
    }

   
    /// <summary>
    /// 将Unicon字符串转成汉字String
    /// </summary>
    /// <param name="str">Unicon字符串</param>
    /// <returns>汉字字符串</returns>
    public static string UniconToString(string SourceString)
    {
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        return reg.Replace(SourceString, delegate (Match m)
        {
            return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString();
        });
    }
    #endregion

    #region 获取牌的位置
    /// <summary>
    /// 获取牌的x轴位置
    /// </summary>
    /// <param name="startX">起始位置x</param>
    /// <param name="offset">间隙</param>
    /// <param name="index">序号</param>
    /// <returns></returns>
    public static float GetCardX(float startX, float offset, int index)
    {
        return startX + offset * index;
    }

    /// <summary>
    /// 获取牌的中心位置
    /// </summary>
    /// <param name="startpos">开始位置</param>
    /// <param name="offset">间隙</param>
    /// <param name="allNum">总牌数</param>
    /// <returns></returns>
    public static float GetCardCenter(float startpos, float offset, int allNum)
    {
        return (startpos + (offset * (allNum -1))) / 2;
    }
    #endregion

    #region 坐标转换

    public static int ServerToLocal_5(int server, int selfServer)
    {
        return ServerToLocal(server, selfServer, 5);
    }

    /// <summary>
    /// 从服务器转本地坐标
    /// </summary>
    /// <param name="server"></param>
    /// <param name="selfServer"></param>
    /// <param name="allNum"></param>
    /// <returns></returns>
    public static int ServerToLocal(int server, int selfServer, int allNum = 3)
    {
        int local = server - selfServer;
        if (local < 0)
        {
            local += allNum;
        }
        return local;
    }


    /// <summary>
    /// 本地转服务器坐标 五个人
    /// </summary>
    /// <param name="local"></param>
    /// <param name="selfServer"></param>
    /// <returns></returns>
    public static int LocalToServerPos_5(int local, int selfServer)
    {
        return LocalToServerPos(local,selfServer,5);
    }
    /// <summary>
    /// 服务器位置转本地位置
    /// </summary>
    /// <param name="local">本地位置</param>
    /// <param name="selfServer">自己服务器位置</param>
    /// <param name="allNum">总人数</param>
    /// <returns>服务器位置</returns>
    public static int LocalToServerPos(int local, int selfServer, int allNum = 3)
    {
        int server = local + selfServer;
        return server % allNum;
    }

    #endregion


    #region 层级获取
    /// <summary>
    /// 获取层级通过名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int GetLayer(string name)
    {
        return LayerMask.NameToLayer(name);
        
    }
    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static void SetObjLayer(string name,GameObject obj)
    {
        var all = obj.GetComponentsInChildren<Transform>(true);
        var layer = GetLayer(name);
        foreach(Transform a in all)
        {
            a.gameObject.layer = layer;
        }
    }
    #endregion

    /// <summary>
    /// www快捷加载
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callBack"></param>
    public static void WWWLoad(string url, Action<WWW> callBack)
    {
        ResMgr.Intstance.StartCoroutine(_WWWLoad(url, callBack));
    }

    public static void WWWGet(string url, Action<string> callBack)
    {
        ResMgr.Intstance.StartCoroutine(_WWWLoad(url, www=> 
        {
            if (www != null)
            {
                callBack(www.text);
            }
            else
            {
                callBack(null);
            }
        }));
    }

    /// <summary>
    /// 请求url 并且加入签名
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="callBack">返回</param>
    public static void WWWGetAndSign(string url,Action<string>callBack)
    {
        ResMgr.Intstance.StartCoroutine(_WWWLoadAddHead(url,callBack));
    }

    private static IEnumerator _WWWLoadAddHead(string url,Action<string> callBack)
    {
        WWWForm form = new WWWForm();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string subUrl = "";
        string sign = GetSign(url, out dic, out subUrl);
        Dictionary<string,string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/x-www-form-urlencoded");
        headers.Add("Cookie", "data=" + sign);
        foreach (string key in dic.Keys)
        {
            form.AddField(key, dic[key]);
        }
        WWW www = new WWW(subUrl, form.data, headers);

        yield return www;
        string str = "";

        if (string.IsNullOrEmpty(www.error))
        {
            str = www.text;
        }
        else
        {
            str = null;
            Debug.LogError("www请求出错"+www.error);
        }
        Debug.LogWarning("str="+ str+"url="+ url+ "/Cookies="+ sign);
        if (callBack != null)
        {
            callBack(str);
        }
    }


    private static IEnumerator _WWWLoad(string url , Action<WWW> callBack)
    {
        System.Random rand = new System.Random();
        int time = 30;
        WWW www = new WWW(url);
        while(!www.isDone)
        {
            
            yield return new WaitForSeconds(1);
            time -= 1;
            if (time <= 0|| !string.IsNullOrEmpty(www.error))
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    callBack(www);
                }
                else
                {
                    callBack(null);
                }
                
                yield break;
            }
        }
       
        callBack(www);
    }
    /// <summary> 获取IP地址 </summary>  
    /// <returns></returns>  
    public static string GetAddressIP()
    {
        string AddressIP = string.Empty;
#if UNITY_IPONE && !UNITY_EDITOR
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();  
        for (int i = 0; i < adapters.Length; i++)  
        {  
            if (adapters[i].Supports(NetworkInterfaceComponent.IPv4))  
            {  
                UnicastIPAddressInformationCollection uniCast = adapters[i].GetIPProperties().UnicastAddresses;  
                if (uniCast.Count > 0)  
                {  
                    for (int j = 0; j < uniCast.Count; j++)  
                    {  
                        //得到IPv4的地址。 AddressFamily.InterNetwork指的是IPv4  
                        if (uniCast[j].Address.AddressFamily == AddressFamily.InterNetwork)  
                        {  
                            AddressIP = uniCast[j].Address.ToString();  
                        }  
                    }  
                }  
            }  
        }  
#endif
#if UNITY_STANDALONE_WIN || UNITY_ANDROID
        ///获取本地的IP地址  
        for (int i = 0; i < Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length; i++)
        {
            if (Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].AddressFamily.ToString() == "InterNetwork")
            {
                AddressIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString();
            }
        }
#endif
        return AddressIP;
    }

    //地球半径，单位米
    private const double EARTH_RADIUS = 6378137;
    /// <summary>
    /// 计算两点位置的距离，返回两点的距离，单位 米
    /// 该公式为GOOGLE提供，误差小于0.2米
    /// </summary>
    /// <param name="lat1">第一点纬度</param>
    /// <param name="lng1">第一点经度</param>
    /// <param name="lat2">第二点纬度</param>
    /// <param name="lng2">第二点经度</param>
    /// <returns></returns>
    public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        if (lat1 < 0 || lng1 < 0 || lat2 < 0 || lng2  < 0)
        {
            return -1;
        }
        double radLat1 = Rad(lat1);
        double radLng1 = Rad(lng1);
        double radLat2 = Rad(lat2);
        double radLng2 = Rad(lng2);
        double a = radLat1 - radLat2;
        double b = radLng1 - radLng2;
        double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
        return result;
    }

    /// <summary>
    /// 经纬度转化成弧度
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    private static double Rad(double d)
    {
        return (double)d * Math.PI / 180d;
    }


    /// <summary>
    /// 根据经纬度来查询地址信息
    /// </summary>
    /// <param name="lng">经度</param>
    /// <param name="lat">纬度</param>
    /// <returns></returns>
    public static void GetAddressByLocation(string lng, string lat,Action<LocationAddressResultData> callBack)
    {
        Action<string> wwwFinished = str => 
        {
            Debug.LogWarning("获取地理位置=" + str);
            if (!string.IsNullOrEmpty(str))
            {
                LocationAddressResultData _AddressInfo = JsonUtility.FromJson<LocationAddressResultData>(str);
                if (callBack != null)
                {
                    callBack(_AddressInfo);
                }
            }
        };
        string url = "http://api.map.baidu.com/geocoder/v2/?location={0},{1}&output=json&pois=0&ak=AE8c630d3ea66062ae2a4844434f868d";
        url = string.Format(url, lat, lng);
        ResMgr.Intstance.StartCoroutine(GetWWWGetData(url, wwwFinished));
    }

    /// <summary>
    /// 判断对象是否为空
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool isNull(object obj)
    {
        
        if (obj is string)
        {
            return string.IsNullOrEmpty((string)obj);
        }
        return obj == null;
    }

    /// <summary>  
    /// Base64编码  
    /// </summary>  
    public static string Base64Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>  
    /// Base64解码  
    /// </summary>  
    public static string Base64Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    public static byte[] UncompressByType(string decodeStr)
    {
        return new byte[2];
    }


    public static string Compress(string param)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
        //byte[] data = Convert.FromBase64String(param);  
        MemoryStream ms = new MemoryStream();
        Stream stream = new zlib.ZOutputStream(ms);
        try
        {
            stream.Write(data, 0, data.Length);
        }
        finally
        {
            stream.Close();
            ms.Close();
        }
        return Convert.ToBase64String(ms.ToArray());  
    }

    /// <summary>  
    /// 解压  
    /// </summary>  
    /// <param name="param"></param>  
    /// <returns></returns>  
    public static string Deompress(string param)
    {
        
        Debug.Log("11111111111111commonString:");
        string commonString = "";
        int data = 0;
        int stopByte = -1;
        byte[] buffer = Convert.FromBase64String(param);// 解base64  
        MemoryStream intms = new MemoryStream(buffer);
        zlib.ZInputStream inZStream = new zlib.ZInputStream(intms);
        int count = 1024 * 1024;
        byte[] inByteList = new byte[count];
        int i = 0;
        while (stopByte != (data = inZStream.Read()))
        {
            inByteList[i] = (byte)data;
            i++;
        }
        inZStream.Close();
        commonString = System.Text.Encoding.UTF8.GetString(inByteList, 0, inByteList.Length);
        Debug.Log(commonString);
        Debug.Log("22222222222222commonString:");
        return commonString;  
    }

    /// <summary>
    /// 设定时间缩放
    /// </summary>
    /// <param name="value"></param>
    public static void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private const string scretkey = ";-[kjfsdoojoe5634kds";
    private static string GetSign(string url,out Dictionary<string,string> querydic,out string host)
    {
        querydic = new Dictionary<string, string>();
        
        Uri uri = new Uri(url);
        host = uri.GetLeftPart(UriPartial.Path);
        string[] Queryurl = uri.Query.Replace("?", "").Split('&');
        SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
        foreach (string query in Queryurl)
        {
            int index = query.IndexOf('=');
            string name = query.Substring(0, index);
            string param = query.Substring(index+1, query.Length - index -1);
            dic.Add(name, param);
        }
        string paramStr = "";
        foreach (string key in dic.Keys)
        {
            paramStr += dic[key];
            querydic.Add(key, dic[key]);
        }
        paramStr += scretkey;
        paramStr = MD5Utils.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(paramStr));
        //paramStr = Base64Encode(paramStr);
        return paramStr;
    }

    public static string MD5Encrypt(string str)
    {
        return MD5Utils.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(str));
    }

    public static string GetHTTPsign(string url)
    {
        Uri uri = new Uri(url);
        string str = "";
        string[] Queryurl = uri.Query.Replace("?", "").Split('&');
        SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
        foreach (string query in Queryurl)
        {
            string[] xx = query.Split('=');
            dic.Add(xx[0], xx[1]);
        }
        string key = "q8#4NgzlpdLM%wryM61zSDjJM7wU*KY^";
        foreach (KeyValuePair<string, string> kv in dic)
        {
            str += kv.Key + kv.Value;
        }
        str += key;
        return MD5Utils.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(str)).ToUpper();
    }
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static string ReadAllText(string fileName)
    {
       
        if (!File.Exists(fileName))
        {
            return null;
        }
        return File.ReadAllText(fileName);
    }

    public static void WriteAllText(string fileName,string allTxt)
    {
        string dirPath = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!string.IsNullOrEmpty(allTxt) && !string.IsNullOrEmpty(fileName))
        {
            File.WriteAllText(fileName, allTxt);
        }
    }
}

#region IP转地址信息
[Serializable]
public class AddressResultData
{
    public int code = 0;
    public AddressInfo data = new AddressInfo();
}

[Serializable]
public class AddressInfo
{
    /// <summary>
    /// 国家
    /// </summary>
    public string country;

    /// <summary>
    /// 省
    /// </summary>
    public string region;

    /// <summary>
    /// 市
    /// </summary>
    public string city;

    /// <summary>
    /// 运营商
    /// </summary>
    public string isp;

    /// <summary>
    /// 华南还是华北
    /// </summary>
    public string area;
}
#endregion

#region 经纬度查询地址信息
[System.Serializable]
public class LocationAddressResultData
{
    public int status = -1;
    public LocationAddress result;
}

[System.Serializable]
public class LocationAddress
{
    /// <summary>
    /// 完整地址
    /// </summary>
    public string formattedAddress;

    /// <summary>
    /// 地址详情
    /// </summary>
    public LocationAddressComponent addressComponent;
}

[System.Serializable]
public class LocationAddressComponent
{
    /// <summary>
    /// 国家
    /// </summary>
    public string country;

    /// <summary>
    /// 省
    /// </summary>
    public string province;

    /// <summary>
    /// 市
    /// </summary>
    public string city;

    /// <summary>
    /// 区
    /// </summary>
    public string district;

    /// <summary>
    /// 街道
    /// </summary>
    public string street;

    /// <summary>
    /// 街道号牌
    /// </summary>
    public string streetNumber;
}
#endregion