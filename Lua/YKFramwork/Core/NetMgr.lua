local sprotoLoader = require "Sproto.sprotoloader"
local print_r = require "Sproto.print_r"
local json = require "cjson"
local clientSp
local isReconnect = false
local CS = require("Core.YK")
local function getClientSp()
    if clientSp then
        return clientSp
    end
    clientSp = sprotoLoader.load(PROTO_TYPE.C2S)
    assert(clientSp, "sprotoHelper load C2S proto failed")
    return clientSp
end


---@class NetMsgEventData
---@field error SystemError
---@field id    number
---@field msg

---@class NetMgr
NetMgr = {}
local this = NetMgr
local LocalErrorCode = {}
--超时
LocalErrorCode.eConnectFailed = 0
LocalErrorCode.eTimeOut = 1
--账号被挤
LocalErrorCode.DuplicateCode = 10004
--服务器超时
LocalErrorCode.ServerTimeoutCode = 10001

LocalErrorCode.ServerTimeoutCode = 10001

function NetMgr:New(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    if self.ctor then
        self.ctor()
    end
    return o
end

--- @param Init
function NetMgr:Init()
    CS.Net.Instance:AttachListener(EventDefLua.ServerError, self.OnHandleEvent, self, 99, false)
    CS.Net.Instance:AttachListener(EventDefLua.ServerConnectFinish, self.OnConnectedServered, self, 99, false)
    CS.UIMgr.Instance:AttachListener("OnApplicationPause",self.OnApplicationPause,self,0,false);
    CS.UIMgr.Instance:AttachListener("OnApplicationResume",self.OnApplicationResume,self,0,false);
    CS.Net.Instance:AttachListener(EventDefLua.ServerResponse, self.OnResponse, self, 99, false)
    sprotoLoader.save(CS.LuaMgr.Instance:GetLuaFileLuaByteBuffer("proto_sproto_spb_c2s"), PROTO_TYPE.C2S)
end

---@return NetMgr
function NetMgr.GetInstance()
    if nil == NetMgr.mInstance then
        NetMgr.mInstance = NetMgr:New()
    end
    return NetMgr.mInstance
end

function NetMgr:ConnectedServer()
    self:ColseConnect()
    GameModeMgr.GetInstance():RestData()
    CS.Net.Instance:ConnectedServer()
end

function NetMgr:ColseConnect()
    self.ConnectedServered = false
    CS.Net.Instance:Close()
end

function NetMgr:OnConnectedServered(eventData)
    self.ConnectedServered = true
    self.ReconnectNum = 0
    return false
end

function NetMgr:Send(actionId, msg)
    --if CS.LocalGameCfg.OpenLog then
    --    if protoMap.protos[ actionId ] then
    --        local pro = protoMap.protos[actionId]
    --        print(PrintColorDef.Green .. "发送消息 ID:" .. actionId .. ", 十六:"
    --                .. CS.MyLuaCall:GetSendNumber(actionId) .. ",request:" .. tostring(pro.request) .. ", 协议描述:" .. pro.desc .. PrintColorDef.End)
    --        print_r(msg)
    --    else
    --        print("不存在此协议 十六:" .. CS.MyLuaCall:GetSendNumber(actionId))
    --    end
    --end
    --
    --if msg == nil then
    --    CS.Net.Instance:Send(actionId, nil, false)
    --else
    --    local sp = getClientSp()
    --    local proto = protoMap.protos[actionId]
    --    local result = sp:encode(proto.request, msg)
    --    CS.Net.Instance:Send(actionId, result, false)
    --end
end

---@param eventData type EventDataLua
function NetMgr:OnHandleEvent(eventData)
    if eventData.name == tostring(EventDefLua.ServerError) then
        self:OnServerError(eventData)
    elseif eventData.name == tostring(EventDefLua.ServerResponse) then
        self:OnResponse(eventData)
    end
    return false
end
---@param eventData type EventDataLua
function this:OnServerError(eventData)
    print("服务器发生错误：".. tostring(self.ConnectedServered))
    local errorData = eventData.value
    if errorData.code == LocalErrorCode.ServerTimeoutCode
            or errorData.code == LocalErrorCode.eTimeOut then
        local proto = protoMap.protos[errorData.actionId]
        if proto ~= nil then
            local ev = CS.EventDataLua.New(proto.fullname)
            ev.obj = {}
            ev.obj.error = Error_Code.SystemError.timeout
            ev.obj.id = errorData.actionId
            ev.obj.msg = nil
            CS.Net.Instance:QueueEvent(ev)
        end
    else
        if not self.ConnectedServered then
            return
        end
        self:ColseConnect()
        self:ReConnect()
    end
end

function this:ReConnect(force)
    print("尝试重连ReConnect：".. tostring(isReconnect))
    if force then
        self:ColseConnect()
    end
    if isReconnect then
        return
    end
    self:ReLoginTaskStart()
end

function this:ReConnectFail(error)
    WaitWindow.Close()
    self.ReconnectNum = self.ReconnectNum or 0
    self.ReconnectNum = self.ReconnectNum + 1;
    print("尝试重连ReConnectFail：" .. tostring(isReconnect).."///"..self.ReconnectNum)
    if self.ReconnectNum >= 3 then
        MessagboxData.New(error, function()
            ReLoginTask:Start(
                    function (error)
                        if error == nil then
                            isReconnect = false
                            CS.Net.Instance:QueueEvent(EventDefLua.ReLoginEd)
                        else
                            isReconnect = false
                            YK.GoToScene(SceneNameLua.LoginScene, false, LoginScene)
                        end
                    end)
        end):Show(true)
    else
        fgui.add_timer(1,1,
                function ()
                    WaitWindow.Show()
                    self:ReLoginTaskStart()
                end);
    end
end

function this:ReLoginTaskStart(callback)
    isReconnect = true
    ReLoginTask:Start(
            function (error)
                if error == nil then
                    isReconnect = false
                    CS.Net.Instance:QueueEvent(EventDefLua.ReLoginEd)
                else
                    self:ReConnectFail(error)
                end
            end)
end

---@param eventData  EventDataLua
function this:OnTickOutPlayer(eventData)
    print("账号被挤")
    --if not self.ConnectedServered then
    --    return false
    --end
    --self.ConnectedServered = false
    self:ColseConnect()
    --TipsBox.ShowTips("账号被挤")
    function clickEnter()
        YK.GoToScene(SceneNameLua.LoginScene, false, LoginScene)
    end

    if eventData.name == tostring(protoMap.protos[M_Role.tickOutPlayer.id].fullname) then
        MessagboxData.New(eventData.obj.msg.reason, clickEnter):Show(true)
    end

    return false
end

---@param serverMsg EventDataLua
function this:OnResponse(serverMsg)

    local proto = protoMap.protos[serverMsg.value.pack.ActionId]
    local ev = CS.EventDataLua.New(proto.fullname)
    ev.obj = {}
    ev.obj.error = serverMsg.value.pack.ErrorCode
    ev.obj.id = serverMsg.value.pack.ActionId
    ev.obj.msg = nil
    if #serverMsg.value.resultData > 0 then
        local sp = getClientSp()
        print(" OnResponse ActionId>>>>>>>>>> ", serverMsg.value.pack.ActionId
        , ",十六:", CS.MyLuaCall:GetSendNumber(serverMsg.value.pack.ActionId))
        if proto.response then
            local result = sp:decode(proto.response, serverMsg.value.resultData)
            ev.obj.msg = result
        end
    end
    CS.Net.Instance:QueueEvent(ev)
    if CS.LocalGameCfg.OpenLog then
        local testData = {}
        testData.error = ev.obj.error
        testData.id = serverMsg.value.pack.ActionId
        testData.Data = ev.obj.msg
        if protoMap.protos[ testData.id ] then
            local pro = protoMap.protos[testData.id]
            print("<color='#ffff00'>接收消息 ID:" .. testData.id .. ", 十六:"
                    .. CS.MyLuaCall:GetSendNumber(testData.id) .. ",response:" .. tostring(pro.response) .. ", 协议描述:" .. pro.desc .. "</color>")
        else
            print("<color='#ff0000'>不存在此协议 十六:" .. CS.MyLuaCall:GetSendNumber(testData.id) .. "</color>")
        end
        print_r(testData)
    end
    return false
end

function this:OnApplicationPause()
    if YK.ObjectIsNull(CS.SceneMgr.Instance.currentScene)
            or CS.SceneMgr.Instance.currentScene.SceneName ~= SceneNameLua.LoginScene
            or CS.SceneMgr.Instance.currentScene.SceneName ~= "entrance"
            or CS.SceneMgr.Instance.currentScene.SceneName ~= "TransferScene"
    then
        return false
    end
    if self.ConnectedServered then
        self:GetInstance():ColseConnect()
    end
    return false;

end

function this:OnApplicationResume()
    if YK.ObjectIsNull(CS.SceneMgr.Instance.currentScene)
            or CS.SceneMgr.Instance.currentScene.SceneName ~= SceneNameLua.LoginScene
            or CS.SceneMgr.Instance.currentScene.SceneName ~= "entrance"
            or CS.SceneMgr.Instance.currentScene.SceneName ~= "TransferScene"
    then
        return false
    end

    if not self.ConnectedServered then
        self:GetInstance():ReConnect()
    end
    return false;
end

function this:OnDestroy()
    CS.Net.Instance:DetachListener(protoMap.protos[M_Role.tickOutPlayer.id].fullname, self.OnTickOutPlayer, self)
    CS.Net.Instance:DetachListener(EventDefLua.ServerError, NetMgr.OnHandleEvent, self)
    CS.Net.Instance:DetachListener(EventDefLua.ServerResponse, NetMgr.OnResponse, self)
    CS.UIMgr.Instance:DetachListener("OnApplicationPause",self.OnApplicationPause,self,0,false);

end

function this:ServerGetData(url, callback)
    local serverHttpUrl
    local serverIps = serverIpTable.build
    if GameCfg.GetIsAppstoreAuditMode() then
        serverIps = serverIpTable.test
        serverHttpUrl = serverIps[GameCfg.CollectionID]
    else
        if not GameCfg.isPublic then
            serverHttpUrl = CS.Initialization.Instance.httpUrl
        else
            serverHttpUrl = serverIps[GameCfg.CollectionID]
        end
    end

    --serverHttpUrl = serverIpTable[GameCfg.CollectionID]
    local getUrl = serverHttpUrl .. url
    print("请求www url=" .. getUrl)

    CS.ComUtil.WWWGetAndSign(getUrl, function(msg)
        if msg then
            print("www请求的返回msg=")
            print(msg)
            local data = json.decode(msg)
            if callback then
                callback(data)
            end
        else
            callback(nil)
            print("请求出错 url=" .. getUrl)
        end
    end)
end

function this:AttachListener(protocolTable, fun, obj, priority)
    assert(type(protocolTable) == "table"
            and type(fun) == "function"
            and obj, "传入参数有误" .. type(protocolTable) .. "/" .. type(fun) .. "/" .. type(obj))
    priority = priority or 0
    for name, data in pairs(protocolTable) do
        if type(data) == "table" and data.id ~= nil then
            CS.Net.Instance:AttachListener(protoMap.protos[data.id].fullname, fun, obj, priority)
        end
    end
end

function this:DetachListener(protocolTable, fun, obj)
    assert(type(protocolTable) == "table"
            and type(fun) == "function"
            and obj, "传入参数有误")
    for name, data in pairs(protocolTable) do
        if type(data) == "table" and data.id ~= nil then
            CS.Net.Instance:DetachListener(protoMap.protos[data.id].fullname, fun, obj)
        end
    end
end

