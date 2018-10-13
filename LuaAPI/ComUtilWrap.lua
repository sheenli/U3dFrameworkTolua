---@class ComUtil : object
local m = {}
---@param transChild UnityEngine.Transform
---@param transParent UnityEngine.Transform
function m.AddGameObjectToZeroPos(transChild, transParent) end
---@param transChild UnityEngine.Transform
---@param transParent UnityEngine.Transform
function m.AddGameObjectTo(transChild, transParent) end
---@param buffer table
---@return string
function m.GetUTF8StringTrimZero(buffer) end
---@param val string
---@return string
function m.GetVerticalString(val) end
---@param generic System.Type
---@param toCheck System.Type
---@return bool
function m.IsSubClassOfRawGeneric(generic, toCheck) end
---@param baseType System.Type
---@param toCheck System.Type
---@return bool
function m.IsSubClassOf(baseType, toCheck) end
---@param root UnityEngine.GameObject
---@param tag string
---@return table
function m.SortRoutePointExcludeTag(root, tag) end
---@param root UnityEngine.GameObject
---@param tag string
---@return table
function m.SortRoutePointWithTag(root, tag) end
---@param go UnityEngine.GameObject
---@param localPath string
---@return UnityEngine.GameObject
function m.FindGameObjectByLocalPath(go, localPath) end
---@param seed int
---@return float
function m.GetRandIn0_1(seed) end
---@param tan UnityEngine.Transform
---@param name string
---@param oneLevel bool
---@return UnityEngine.Transform
function m.FindTransformInChild(tan, name, oneLevel) end
---@return table
function m.GetOneListStr() end
---@return table
function m.GetOneListTran() end
---@param tan UnityEngine.Transform
---@param names table
---@param oneLevel bool
---@return table
function m.FindTransformInChildSort(tan, names, oneLevel) end
---@param tan UnityEngine.Transform
---@param name string
---@return table
function m.FindTransformNameAll(tan, name) end
---@param tran UnityEngine.Transform
---@return table
function m.FindTransformAllActiveChild(tran) end
---@param unixTimeStamp long
---@return string
function m.TimestampToTime(unixTimeStamp) end
---@param number int
---@return int
function m.GetNumberLen(number) end
---@param listName table
---@param tran UnityEngine.Transform
---@param trans table
function m.GetTransformInChilds(listName, tran, trans) end
---@param url string
---@param encode System.Text.Encoding
---@return string
function m.HTTPGetData(url, encode) end
---@param tw float
---@param allNum int
---@param index int
---@param thresholdMin float
---@param thresholdMax float
---@return float
function m.GetPosX(tw, allNum, index, thresholdMin, thresholdMax) end
---@param size float
---@param unitSize float
---@param allNum int
---@param index int
---@param thresholdMin float
---@param thresholdMax float
---@return float
function m.GetFixedSizeIndexPos(size, unitSize, allNum, index, thresholdMin, thresholdMax) end
---@param mb UnityEngine.MonoBehaviour
---@return bool
function m.GetActive(mb) end
---@param ip string
---@param callBack System.Action
function m.GetAddressByIp(ip, callBack) end
---@param url string
---@param callBack System.Action
---@return System.Collections.IEnumerator
function m.GetWWWGetData(url, callBack) end
---@param SourceString string
---@return string
function m.UniconToString(SourceString) end
---@param startX float
---@param offset float
---@param index int
---@return float
function m.GetCardX(startX, offset, index) end
---@param startpos float
---@param offset float
---@param allNum int
---@return float
function m.GetCardCenter(startpos, offset, allNum) end
---@param server int
---@param selfServer int
---@return int
function m.ServerToLocal_5(server, selfServer) end
---@param server int
---@param selfServer int
---@param allNum int
---@return int
function m.ServerToLocal(server, selfServer, allNum) end
---@param local int
---@param selfServer int
---@return int
function m.LocalToServerPos_5(local, selfServer) end
---@param local int
---@param selfServer int
---@param allNum int
---@return int
function m.LocalToServerPos(local, selfServer, allNum) end
---@param name string
---@return int
function m.GetLayer(name) end
---@param name string
---@param obj UnityEngine.GameObject
function m.SetObjLayer(name, obj) end
---@param url string
---@param callBack System.Action
function m.WWWLoad(url, callBack) end
---@param url string
---@param callBack System.Action
function m.WWWGet(url, callBack) end
---@param url string
---@param callBack System.Action
function m.WWWGetAndSign(url, callBack) end
---@return string
function m.GetAddressIP() end
---@param lat1 double
---@param lng1 double
---@param lat2 double
---@param lng2 double
---@return double
function m.GetDistance(lat1, lng1, lat2, lng2) end
---@param lng string
---@param lat string
---@param callBack System.Action
function m.GetAddressByLocation(lng, lat, callBack) end
---@param obj object
---@return bool
function m:isNull(obj) end
---@param message string
---@return string
function m.Base64Encode(message) end
---@param message string
---@return string
function m.Base64Decode(message) end
---@param decodeStr string
---@return table
function m.UncompressByType(decodeStr) end
---@param param string
---@return string
function m.Compress(param) end
---@param param string
---@return string
function m.Deompress(param) end
---@param value float
function m.SetTimeScale(value) end
---@param str string
---@return string
function m.MD5Encrypt(str) end
---@param url string
---@return string
function m.GetHTTPsign(url) end
function m.QuitGame() end
return m