using System.Collections.Generic;
using UnityEngine;

namespace Bindler
{
    public class RoleBindler : MonoBehaviour
    {
        public SimpleAnimation animatorPlayer;
        public int confId { get; set; }
        public int showIndex { get; set; }//方便设置商店位置的下标
        public uint serverId { get; set; }
        public string userId { get; set; }//拥有者id
        public int gridId { get; set; }
        private BoxCollider _collider;
        private Transform[] allChild;
        private Dictionary<string, Material> _materials;

        //private List<Material> _originMaterials;

        private Dictionary<string, List<Material>> _originMaterials;


        private void Awake()
        {
            allChild = transform.GetComponentsInChildren<Transform>();
            foreach (var item in allChild)
            {
                item.gameObject.layer = 8;
            }

            _materials = new Dictionary<string, Material>();
            //_originMaterials = new List<Material>();

            _originMaterials = new Dictionary<string, List<Material>>();
            animatorPlayer = GetComponentInChildren<SimpleAnimation>();
            //soundPlayer = GetComponentInChildren<SoundPlayer>();

            var meshRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for(int i = 0; i< meshRenders.Length; i++)
            {
                var meshRender = meshRenders[i];
                _originMaterials[meshRender.gameObject.name] = new List<Material>();
                var preMats = meshRender.materials;
                foreach (Material mat in preMats)
                {
                    _originMaterials[meshRender.gameObject.name].Add(mat);
                }
            }
            
        }

        private void Start()
        {
        }

        public void show()
        {
            gameObject.SetActive(true);
        }


        public void hide()
        {
            gameObject.SetActive(false);
        }

        public Transform findNode(string path)
        {
            var node = transform.Find(path);
            return node;
        }


        /// <summary>
        /// 切换材质
        /// </summary>
        /// <param name="mat"></param>
        public void addMat(string effectName, Material mat)
        {
            if (!_materials.ContainsKey(effectName))
            {
                _materials.Add(effectName, mat);
            }

            var meshRender = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            var preMats = meshRender.materials;

            bool isExist = false;
            foreach(var material in preMats)
            {
                var matName = material.name;
                var preName = matName.Substring(0, matName.IndexOf("("));
                if (mat.name.Trim() == preName.Trim())
                {
                    isExist = true;
                    break;
                }
            }
            if (isExist) return;

            Material srcMat = null;
            string realModeName = transform.GetChild(0).name;
            for(int i = 0; i < preMats.Length; i++)
            {
                var matName = preMats[i].name;
                var preName = matName.Substring(0, matName.IndexOf("("));
                if(preName.Trim() == realModeName.Trim())
                {
                    srcMat = preMats[i];
                }
            }

            mat.SetTexture("_MainTex", srcMat.GetTexture("_MainTex"));
            mat.SetColor("_MainColor", new Color(1f, 0.686f, 0, 1f));
            float factor = Mathf.Pow(2, 2.6010f);
            mat.SetColor("_RimColor", new Color(0.749f * factor, 0.5176f * factor, 0 * factor, 1f));
            mat.SetFloat("_RimPower", 1f);
            Material[] materials = new Material[preMats.Length + 1];
            preMats.CopyTo(materials, 0);
            materials[preMats.Length] = mat;
            meshRender.materials = materials;
        }


        public void removeMat(string effectName)
        {
            if (!_materials.ContainsKey(effectName))
            {
                Debug.LogError($"当前角色{serverId}身上没有增加对应效果{effectName}");
                return;
            }

            Material mat = _materials[effectName];
            var meshRender = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            var preMats = meshRender.materials;

            bool notExist = true;
            foreach (var material in preMats)
            {
                var matName = material.name;
                var preName = matName.Substring(0, matName.IndexOf("("));
                if (mat.name.Trim() == preName.Trim())
                {
                    notExist = false;
                    break;
                }
            }
            if (notExist) return;


            Material[] materials = new Material[preMats.Length - 1];
            int index = 0;
            for(int i = 0; i < preMats.Length; i++)
            {
                var matName = preMats[i].name;
                var preName = matName.Substring(0, matName.IndexOf("("));
                if (mat.name.Trim() != preName.Trim())
                {
                    materials[index++] = preMats[i];
                }
            }
            meshRender.materials = materials;
        }


        public void changeOutLineShader(string shaderName,string color = null,float outLine = -1)
        {
            Shader outlineShader = Shader.Find(shaderName);
            var meshRender = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRender == null)
            {
                Debug.LogErrorFormat("当前角色找不到skinmeshrender 物体名字：{0}", gameObject.name);
                return;
            }
            var shareMats = meshRender.materials;
            string realModelName = transform.GetChild(0).name;
            foreach (var mat in shareMats)
            {
                var realNameEndPos = mat.name.IndexOf("(");
                string matName = mat.name;
                if (realNameEndPos != -1)
                {
                    var realMatName = mat.name.Substring(0, realNameEndPos);
                    matName = realMatName;
                }
                if (matName.Trim() == realModelName.Trim())
                {
                    mat.shader = outlineShader;
                    if (outLine != -1) mat.SetFloat("_OutlineFactor", outLine);
                    if (color != null) mat.SetColor("_OutlineCol", getColorByString(color));
                    break;
                }
            }
        }

        public void changeAssassinShader(string shaderName)
        {
            Shader outlineShader = Shader.Find(shaderName);
            Material mat = new Material(outlineShader);

            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                //设置对应的参数
                Material srcMat = null;
                string realModeName = transform.GetChild(0).GetChild(i).name;
                List<Material> materials = null;
                if (_originMaterials.TryGetValue(transform.GetChild(0).GetChild(i).name, out materials))
                {
                    for (int j = 0; j < materials.Count; j++)
                    {
                        var matName = materials[j].name;
                        var preName = matName.Substring(0, matName.IndexOf("("));
                        if (preName.Trim() == realModeName.Trim())
                        {
                            srcMat = materials[j];
                        }
                    }
                    if (srcMat != null)
                    {
                        mat.SetTexture("_MainTex", srcMat.GetTexture("_MainTex"));
                        mat.SetColor("_MainColor", new Color(1f, 0.75f, 0.75f, 1f));
                        mat.DisableKeyword("_RAMPON_ON");
                        //mat.SetTexture("_RampTex", asset);
                        mat.SetTextureScale("_RampTex", new Vector2(1, 1));
                        float factor = 1.4169f;
                        mat.SetColor("_RimColor", new Color(2 * factor, 1.3f * factor, 2f * factor, 1f));
                        mat.SetFloat("_RimPower", 1f);
                        mat.SetFloat("_Transparent", 0.5f);
                        mat.SetFloat("_U_SP", 0f);
                        Material[] mats = new Material[] { mat };
                        var meshRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                        for (int t = 0; t < meshRenders.Length; t++)
                        {
                            if (_originMaterials.ContainsKey(meshRenders[t].gameObject.name))
                                meshRenders[t].materials = mats;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 恢复原先的材质
        /// </summary>
        public void recoveryOriginShader()
        {
            var meshRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for(int i = 0; i < meshRenders.Length; i++)
            {
                var meshRender = meshRenders[i];
                List<Material> result = new List<Material>();
                if(_originMaterials.TryGetValue(meshRender.gameObject.name, out result))
                {
                    Material[] materials = result.ToArray();
                    meshRender.materials = materials;
                }                
            }           
        }

        private Color getColorByString(string colStr)
        {
            string[] strArr = colStr.Split(',');
            if (strArr.Length < 4)
            {
                Debug.LogWarningFormat("颜色参数不对");
                return Color.white;
            }
            return new Color(float.Parse(strArr[0]) / 255, float.Parse(strArr[1]) / 255, float.Parse(strArr[2]) / 255, float.Parse(strArr[3]) / 255);
        }
    }
}
