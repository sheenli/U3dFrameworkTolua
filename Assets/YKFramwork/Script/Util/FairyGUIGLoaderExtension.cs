using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class FairyGUIGLoaderExtension : FairyGUI.GLoader
{
    private static Dictionary<string, Texture> mAllTextureDic = new Dictionary<string, Texture>();

    private bool isExternal()
    {
        return !string.IsNullOrEmpty(this.url) &&(this.url.StartsWith("http://") || this.url.StartsWith("https://"));
    }
    protected override void LoadExternal()
    {
        if (isExternal())
        {
            Texture texture = null;
            if (mAllTextureDic.ContainsKey(this.url))
            {
                if(mAllTextureDic[this.url] == null)
                {
                    mAllTextureDic.Remove(this.url);
                    texture = null;
                }
                else
                {
                    texture = mAllTextureDic[this.url];
                }
            }
            if (texture != null)
            {
                this.onExternalLoadSuccess(new NTexture(texture));
            }
            else
            {
                ComUtil.WWWLoad(this.url, www =>
                {

                    if (www != null && string.IsNullOrEmpty(www.error) && www.texture != null)
                    {
                        mAllTextureDic[www.url] = www.texture;
                    }

                    if (this != null && this.displayObject != null && !this.displayObject.isDisposed)
                    {
                        if (mAllTextureDic.ContainsKey(this.url))
                        {
                            this.onExternalLoadSuccess(new NTexture(mAllTextureDic[this.url]));
                        }
                        else
                        {
                            this.onExternalLoadFailed();
                        }
                    }
                });
            }     
        }
        else
        {
            base.LoadExternal();
        }

        
    }

    protected IEnumerator TextureLoad(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error) && www.texture != null)
        {
            mAllTextureDic[this.url] = www.texture;// new FairyGUI.NTexture();
            if (this.displayObject != null && !this.displayObject.isDisposed)
            {
                this.onExternalLoadSuccess(new NTexture(mAllTextureDic[this.url]));
            }
        }
        else
        {
            if (this.displayObject != null && !this.displayObject.isDisposed)
            {
                this.onExternalLoadFailed();
            }
        }
        www.Dispose();
    }

    public override void Dispose()
    {
        if (isExternal() && mAllTextureDic.ContainsKey(this.url))
        {
            mAllTextureDic.Remove(this.url);
            if (texture != null)
            {
                texture.Dispose();
                // texture = null;
            }
        }
        base.Dispose();
    }

   
}
