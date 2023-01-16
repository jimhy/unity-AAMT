using System;

namespace AAMT.Editor
{
    /// <summary>
    /// 事件类型
    /// 事件派发需要事件派发事件实例
    /// </summary>
    public class Event
    {
        private IEventDispatcher _target;
        private string           _type;
        private object           _data;
        public  Action<object>   callBack { get; set; }

        public Event(string type, object data = null)
        {
            _type = type;
            _data = data;
        }

        internal Event()
        {
        }

        public string type
        {
            internal set { _type = value; }
            get { return _type; }
        }

        public object data
        {
            internal set { _data = value; }
            get { return _data; }
        }

        public IEventDispatcher target
        {
            internal set { _target = value; }
            get { return _target; }
        }

        public void callBackInvoke(object data)
        {
            if (callBack != null) callBack.Invoke(data);
        }

        internal void clear()
        {
            _target = null;
            _type   = null;
            _data   = null;
        }
    }
}