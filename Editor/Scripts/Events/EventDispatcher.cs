using System;
using System.Collections.Generic;
using UnityEngine;


namespace AAMT.Editor
{
    /// <summary>
    /// 事件派发
    /// 用于派发事件
    /// </summary>
    public class EventDispatcher : IEventDispatcher
    {
        protected struct EventObj
        {
            public string type;
            public object data;
            public Action<object> callBack;
        }

        protected class EventCBStruct
        {
            /// <summary>
            /// 优先级
            /// </summary>
            public int priority;

            /// <summary>
            /// 回调函数
            /// </summary>
            public Action<Event> callBack;

            public bool isClear;

            public EventCBStruct(Action<Event> callBack, int priority)
            {
                this.callBack = callBack;
                this.priority = priority;
                isClear       = false;
            }
        }

        protected Dictionary<string, List<EventCBStruct>> _eventCallBacks;
        protected Stack<Event> _events;
        protected Dictionary<uint, List<EventObj>> _lateEvents;

        /// <summary>
        /// 当前延迟事件派发的索引标志，主要用于区分事件是当前帧添加的还是上一帧添加的，
        /// 如果是上一帧添加的事件，则这一帧就要派发出去，否则等到下一帧才派发。
        /// </summary>
        protected uint _currentIndex = 1;

        public EventDispatcher()
        {
            _eventCallBacks = new Dictionary<string, List<EventCBStruct>>();
            _lateEvents     = new Dictionary<uint, List<EventObj>>();
            _lateEvents[1]  = new List<EventObj>();
            _lateEvents[2]  = new List<EventObj>();
            _events         = new Stack<Event>();
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="callBack">监听回调</param>
        /// <param name="priority">监听优先级 数字越大,优先级越高</param>
        public void addEventListener(string type, Action<Event> callBack, int priority = 0)
        {
            List<EventCBStruct> list = null;
            if (!hasEventListener(type))
            {
                list = new List<EventCBStruct>();
                _eventCallBacks.Add(type, list);
            }
            else if (!hasEventListener(type, callBack))
            {
                list = _eventCallBacks[type];
            }

            if (list == null) return;
            list.Add(new EventCBStruct(callBack, priority));
            sortLoadObjes(list);
        }

        private void sortLoadObjes(List<EventCBStruct> list)
        {
            list.Sort((a, b) =>
            {
                if (a.priority <= b.priority) return 1;
                return -1;
            });
        }

        public void removeEventListener(string type, Action<Event> callBack)
        {
            if (!hasEventListener(type) || _eventCallBacks[type] == null) return;
            var list = _eventCallBacks[type];
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item.callBack.Equals(callBack))
                {
                    item.isClear = true;
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        public virtual void removeEventListeners(string type, bool toOtherAssembly)
        {
            removeEventListeners(type);
        }

        public void removeAllEventListeners()
        {
            _eventCallBacks.Clear();
            _events.Clear();
        }

        public void removeEventListeners(string type)
        {
            if (!hasEventListener(type)) return;
            _eventCallBacks.Remove(type);
        }

        public virtual void dispatchEventWith(string type, object data = null, Action<object> cb = null)
        {
            if (!hasEventListener(type) || _eventCallBacks[type] == null) return;
            var e = getEvent();
            e.target   = this;
            e.type     = type;
            e.data     = data;
            e.callBack = cb;
            var list = new List<EventCBStruct>(_eventCallBacks[type]);
            try
            {
                foreach (var item in list)
                {
                    if (item.isClear) continue;
                    item.callBack(e);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            e.clear();
            if (_events != null) _events.Push(e);
        }

        public virtual void dispatchEventWith(string type, object data, bool toLua, Action<object> cb = null)
        {
            dispatchEventWith(type, data, cb);
        }

        public void dispatchEventNextFrameWith(string type, object data = null, Action<object> cb = null)
        {
            dispatchEventNextFrameWith(type, data, false, cb);
        }

        public void dispatchEventNextFrameWith(string type, object data, bool toLua, Action<object> cb = null)
        {
            var eo = new EventObj();
            eo.type     = type;
            eo.data     = data;
            eo.callBack = cb;
            _lateEvents[_currentIndex].Add(eo);
        }

        public virtual void update()
        {
            var objs = _lateEvents[_currentIndex];
            _currentIndex = _currentIndex == 1 ? 2u : 1u;
            foreach (var item in objs)
            {
                dispatchEventWith(item.type, item.data, item.callBack);
            }

            objs.Clear();
        }

        public bool hasEventListener(string type)
        {
            return _eventCallBacks.ContainsKey(type);
        }

        public bool hasEventListener(string type, Action<Event> callBack)
        {
            if (_eventCallBacks.ContainsKey(type) && _eventCallBacks[type] != null)
            {
                var list = _eventCallBacks[type];
                foreach (var item in list)
                {
                    if (item.callBack.Equals(callBack)) return true;
                }
            }

            return false;
        }

        private Event getEvent()
        {
            if (_events.Count == 0) return new Event();
            else return _events.Pop();
        }

        public void clear()
        {
            _eventCallBacks?.Clear();
            _events?.Clear();
        }

        public void destory()
        {
            clear();
            _eventCallBacks = null;
            _events         = null;
        }
    }
}