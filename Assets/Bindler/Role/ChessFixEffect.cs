using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bindler
{
    public class ChessFixEffect:MonoBehaviour
    {
        [Header("一级需要显示的物体")]
        public List<GameObject> lv1_showGameObjs;
        [Header("二级级需要显示的物体")]
        public List<GameObject> lv2_showGameObjs;
        [Header("三级级需要显示的物体")]
        public List<GameObject> lv3_showGameObjs;

        private Dictionary<uint,List<GameObject>> map;

        private void Awake()
        {
            map = new Dictionary<uint, List<GameObject>>()
            {
                [1] = lv1_showGameObjs,
                [2] = lv2_showGameObjs,
                [3] = lv3_showGameObjs,
            };
        }

        public void FixEffect(uint level)
        {
            if (!map.ContainsKey(level)) return;
            foreach (var item in map)
            {
                var list = item.Value;
                if (list == null) continue;
                if (item.Key == level)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
