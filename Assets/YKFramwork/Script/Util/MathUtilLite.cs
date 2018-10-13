using UnityEngine;
using System.Collections;
using System;

public class MathUtilLite
{

    #region 常量
    public static Vector3 AxisX = new Vector3(1, 0, 0);
    public static Vector3 AxisY = new Vector3(0, 1, 0);
    public static Vector3 AxisZ = new Vector3(0, 0, 1);
    public static Vector3 XYZ1 = Vector3.one;

    public static float ONE_DIV_PI = 1.0f / Mathf.PI;

    public static float COS_15 = Mathf.Cos(Mathf.Deg2Rad * 15.0f);
    public static float COS_35 = Mathf.Cos(Mathf.Deg2Rad * 35.0f);
    public static float COS_45 = Mathf.Cos(Mathf.Deg2Rad * 45.0f);
    public static float COS_75 = Mathf.Cos(Mathf.Deg2Rad * 75.0f);
    public static float COS_60 = Mathf.Cos(Mathf.Deg2Rad * 60.0f);
    public static float COS_30 = Mathf.Cos(Mathf.Deg2Rad * 30.0f);
    public static float COS_20 = Mathf.Cos(Mathf.Deg2Rad * 20.0f);

    public static Vector2 AxisX2D = new Vector2(1, 0);
    public static Vector2 AxisY2D = new Vector2(0, 1);

    public static float EPSILON = 0.001f;

    #endregion

    /// <summary>
    /// 时间戳转换成时间
    /// </summary>
    /// <param name="t">时间戳</param>
    /// <returns></returns>
    public static System.DateTime TransToDateTime(uint t)
    {
        System.DateTime dt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        long lTime = long.Parse(t.ToString() + "0000000");
        System.TimeSpan toNow = new System.TimeSpan(lTime);
        return dt.Add(toNow);
    }
    /// <summary>
    /// 计算两个三维坐标相差的距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>距离</returns>
    public static float DistancePow(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
    }

    /// <summary>
    /// 计算两个二维坐标相差的距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>长度</returns>
    public static float DistancePow(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }

    //andeeee from the Unity forum's steller Catmull-Rom class ( http://forum.unity3d.com/viewtopic.php?p=218400#218400 ):
    public static Vector3 Interp(Vector3[] pts, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 2.0f);
        int numSections = pts.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;
        Vector3 a = pts[currPt];
        Vector3 b = pts[currPt + 1];
        Vector3 c = pts[currPt + 2];
        Vector3 d = pts[currPt + 3];

        return .5f * (
            (-a + 3f * b - 3f * c + d) * (u * u * u)
            + (2f * a - 5f * b + 4f * c - d) * (u * u)
            + (-a + c) * u
            + 2f * b
        );
    }

    /// <summary>
    /// 获取两个点间的夹角
    /// </summary>
    /// <param name="form"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 form, Vector3 to)
    {
        Vector3 nVector = Vector3.zero;
        nVector.x = to.x;
        nVector.y = form.y;
        float a = to.y - nVector.y;
        float b = nVector.x - form.x;
        float tan = a / b;
        return Mathf.Atan(tan) * 180.0f * ONE_DIV_PI;
    }

    public static Vector3 ApproximateDir(Vector3 dir)
    {
        float dotX = Vector3.Dot(dir, AxisX);
        float dotZ = Vector3.Dot(dir, AxisZ);
        if(Mathf.Abs(dotX) > Mathf.Abs(dotZ))
        {
            return dotX > 0 ? AxisX : -AxisX;
        }
        else
        {
            return dotZ > 0 ? AxisZ : -AxisZ;
        }
    }

    /// <summary>
    /// normalize 并且返回 长度
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static float Normalize(ref Vector3 vec)
    {
        float length = Mathf.Sqrt((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
        if (length > 0)
        {
            float oneDivLength = 1.0f / length;
            vec.x = vec.x * oneDivLength;
            vec.y = vec.y * oneDivLength;
            vec.z = vec.z * oneDivLength;
        }
        return length;
    }

    /// <summary>
    /// 尝试到达那个点
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="cur"></param>
    /// <param name="speed"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Vector3 TryToMoveToPosWithSpeed(Vector3 dest, Vector3 cur, float speed, float time)
    {
        Vector3 dir = dest - cur;
        float dis = Normalize(ref dir);
        if (speed * time < dis)
        {
            return cur + dir * speed * time;
        }
        else
        {
            return dest;
        }
    }

    /// <summary>
    /// 移动人物制定距离相差多少的值
    /// </summary>
    /// <param name="dest">目标点</param>
    /// <param name="cur">当前坐标</param>
    /// <param name="speed"></param>
    /// <param name="time">速度</param>
    /// <returns></returns>
    public static Vector3 OffsetToMoveToPosWithSpeed(Vector3 dest, Vector3 cur, float speed, float time)
    {
        Vector3 dir = dest - cur;
        Vector3 maxOffset = dir;
        float dis = Normalize(ref dir);
        if (speed * time < dis)
        {
            return dir * speed * time;
        }
        else
        {
            return maxOffset;
        }
    }

    /// <summary>
    /// float 近似相等
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool IsEqualFloat(float a, float b)
    {
        return (Math.Abs(a - b) < 0.001f);
    }

    public static bool IsEqualFloatRaw(float a, float b)
    {
        return (Math.Abs(a - b) < 0.05f);
    }

    #region 3D空间投影到屏幕坐标

    public static Vector2 ProjectToScreen(Camera cam, Vector3 point)
    {
        point.z = 0;
        Vector3 screenPoint = cam.WorldToScreenPoint(point);
        return new Vector2(screenPoint.x, screenPoint.y);
    }

    #endregion


}
