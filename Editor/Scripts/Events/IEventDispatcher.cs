using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAMT.Editor
{
    public interface IEventDispatcher
    {
        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="callBack">监听回调</param>
        /// <param name="priority">监听优先级 数字越大,优先级越高</param>
        void addEventListener(string type, Action<Event> callBack, int priority = 0);

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="callBack">接受事件的回调函数</param>
        void removeEventListener(string type, Action<Event> callBack);

        /// <summary>
        /// 移除制定类型的所有事件监听
        /// </summary>
        /// <param name="type">需要移除的事件类型</param>
        /// <param name="toOtherAssembly">是否传递事件到热更模块或者主模块</param>
        void removeEventListeners(string type, bool toOtherAssembly);

        /// <summary>
        /// 移除制定类型的所有事件监听
        /// </summary>
        /// <param name="type">需要移除的事件类型</param>
        void removeEventListeners(string type);

        /// <summary>
        /// 事件派发
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="data">事件传递的参数</param>
        void dispatchEventWith(string type, object data = null, Action<Object> cb = null);

        /// <summary>
        /// 事件派发
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="data">事件传递的参数</param>
        /// <param name="toOtherAssembly">是否传递事件到热更模块或者主模块</param>
        void dispatchEventWith(string type, object data, bool toOtherAssembly, Action<Object> cb = null);

        /// <summary>
        /// 延迟一帧派发事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="data">事件传递的参数</param>
        void dispatchEventNextFrameWith(string type, object data = null, Action<Object> cb = null);

        /// <summary>
        /// 延迟一帧派发事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="data">事件传递的参数</param>
        /// <param name="toOtherAssembly">是否传递事件到热更模块或者主模块</param>
        void dispatchEventNextFrameWith(string type, object data, bool toOtherAssembly, Action<Object> cb = null);

        /// <summary>
        /// 是否有该类型事件监听
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns>有则返回true,否则返回false</returns>
        bool hasEventListener(string type);

        /// <summary>
        /// 是否有该类型事件监听
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="callBack">事件的回调函数</param>
        /// <returns>有则返回true,否则返回false</returns>
        bool hasEventListener(string type, Action<Event> callBack);
    }
}