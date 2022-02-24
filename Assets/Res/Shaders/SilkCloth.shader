Shader "Custom/SilkCloth"
{
    Properties
    {
        _Color("Color",Color)=(1,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _CubeMap("Cube Map",CUBE)=""{}
        _Opacity("Opacity",Range(0,1))=0.5
        _ReflectAmount("Reflect Amount",Range(0,2))=1
        _FresnelPower("Fresnel Power",Range(0,5))=1


    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
       

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

          

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvN : TEXCOORD3;
                float4 pos : SV_POSITION;
                fixed4 reflectDir : TEXCOORD1;
                float4 NdotV : TEXCOORD2;
                
            };

            float4 _Color;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            samplerCUBE _CubeMap;
            float _Opacity;
            fixed _ReflectAmount;
            half _FresnelPower;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv =TRANSFORM_TEX(v.texcoord,_MainTex);
                o.uvN =TRANSFORM_TEX(v.texcoord,_NoiseTex);
                
                fixed3 normal = normalize(UnityObjectToWorldNormal(v.normal));
                fixed3 viewDir = normalize(_WorldSpaceCameraPos-mul(unity_ObjectToWorld,v.vertex));
        

                fixed NdotV = dot(normal,viewDir);
                o.NdotV=NdotV;
                o.reflectDir.xyz =  reflect(-viewDir,normal);
                // o.reflectDir.w = _ReflectAmount * pow(1-NdotV,_FresnelPower);
                

                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                
                fixed4 Texcol = tex2D(_MainTex, i.uv);
                fixed4 NoiseTex = tex2D(_NoiseTex, i.uvN);
                fixed4 Fresnel =  _ReflectAmount * pow(1-i.NdotV,_FresnelPower);
                fixed3 CubeCorol = texCUBE(_CubeMap,i.reflectDir.xyz).rgb ;
                float newOpacity = min(1, _Color.a/abs(i.NdotV)*_Opacity);
               
                //return float4 (_Color.rgb,newOpacity);
                return float4((min(CubeCorol,CubeCorol*Texcol)+ NoiseTex*Texcol*_Color + Fresnel*4).rgb,newOpacity);
            }
            ENDCG
        }
    }
}
