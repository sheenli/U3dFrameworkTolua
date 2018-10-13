---@class FairyGUI.UIPackage : object
---@field public id string
---@field public name string
---@field public assetPath string
---@field public customId string
local m = {}
---@param id string
---@return FairyGUI.UIPackage
function m.GetById(id) end
---@param name string
---@return FairyGUI.UIPackage
function m.GetByName(name) end
---@param bundle UnityEngine.AssetBundle
---@return FairyGUI.UIPackage
function m.AddPackage(bundle) end
---@param packageIdOrName string
function m.RemovePackage(packageIdOrName) end
function m.RemoveAllPackages() end
---@return table
function m.GetPackages() end
---@param pkgName string
---@param resName string
---@return FairyGUI.GObject
function m.CreateObject(pkgName, resName) end
---@param url string
---@return FairyGUI.GObject
function m.CreateObjectFromURL(url) end
---@param pkgName string
---@param resName string
---@param callback FairyGUI.UIPackage.CreateObjectCallback
function m.CreateObjectAsync(pkgName, resName, callback) end
---@param pkgName string
---@param resName string
---@return object
function m.GetItemAsset(pkgName, resName) end
---@param url string
---@return object
function m.GetItemAssetByURL(url) end
---@param pkgName string
---@param resName string
---@return string
function m.GetItemURL(pkgName, resName) end
---@param url string
---@return FairyGUI.PackageItem
function m.GetItemByURL(url) end
---@param url string
---@return string
function m.NormalizeURL(url) end
---@param source FairyGUI.Utils.XML
function m.SetStringsSource(source) end
---@param itemId string
---@return FairyGUI.PixelHitTestData
function m:GetPixelHitTestData(itemId) end
---@return table
function m:GetItems() end
---@param itemId string
---@return FairyGUI.PackageItem
function m:GetItem(itemId) end
---@param itemName string
---@return FairyGUI.PackageItem
function m:GetItemByName(itemName) end
return m