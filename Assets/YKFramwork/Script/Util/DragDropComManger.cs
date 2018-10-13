using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FairyGUI
{
    /// <summary>
    /// Helper for drag and drop.
    /// 这是一个提供特殊拖放功能的功能类。与GObject.draggable不同，拖动开始后，他使用一个替代的图标作为拖动对象。
    /// 当玩家释放鼠标/手指，目标组件会发出一个onDrop事件。
    /// </summary>
    public class DragDropComManger
    {
        public bool dropTarget = false;
        private Func<GObject,bool> mDropTargetFunc;
        private GObject mAgent;
        public GObject dragAgent
        {
            get { return mAgent; }
        }
        public DragDropComManger(GObject agent, Func<GObject, bool> dropTarget)
        {
            
            mDropTargetFunc = dropTarget;
            mAgent = agent;
            mAgent.visible = false;
            mAgent.SetPivot(0.5f, 0.5f, true);
            ///mAgent.sortingOrder = int.MaxValue;
            mAgent.onDragEnd.Add(__dragEnd);
        }
        public void Cancel()
        {
            if (mAgent.visible)
            {
                mAgent.StopDrag();
                mAgent.visible = false;
            }
        }
        public void StartDrag(int touchPointID = -1)
        {
            dropTarget = false;
            mAgent.touchable = false;
            mAgent.visible = true;
            mAgent.xy = mAgent.parent.GlobalToLocal(Stage.inst.GetTouchPosition(touchPointID));
            mAgent.StartDrag(touchPointID);
        }
        void __dragEnd(EventContext evt)
        {
            mAgent.visible = false;

            GObject obj = GRoot.inst.touchTarget;
           
            while (obj != null)
            {
                if (obj is GComponent)
                {
                    bool flag = true;
                    if (mDropTargetFunc != null)
                    {
                        flag = mDropTargetFunc(obj);
                    }
                    if (flag)
                    {
                        dropTarget = true;
                        if (!((GComponent)obj).onDrop.isEmpty)
                        {
                            obj.RequestFocus();
                            ((GComponent)obj).onDrop.Call(mAgent);
                        }
                        mAgent.visible = false;
                        return;
                    }
                   
                }

                obj = obj.parent;
            }
            
        }
    }
}
