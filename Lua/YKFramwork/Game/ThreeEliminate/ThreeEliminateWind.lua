local YK = require("Core.YK")
local fgui = require("FGUI.FairyGUI")
local basewind = require("Core.WindowBase")

---@class ThreeEliminateWindObj
---@field n2 test
---@field BtnEntranceEditor FairyGUI.GButton

---@class ThreeEliminateWind : WindowBase
---@field Objs ThreeEliminateWindObj
local this = YK.wind_class(basewind)
function this:ctor()
    self.packName = "Main"
    self.resName = "Main"
    self.isDotDel = false
    local extUrl = "ui://Main/Extcom"
    fgui.register_extension(extUrl,require("Game.ext.Test"))
end

function this:OnInit()

    self.Objs.n2:test("test Extcom")
end


function this:OnShown(param)

end

function this:OnBtnClick(btn)
    print("click btn ----------------")
end

return this