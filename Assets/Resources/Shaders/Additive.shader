Shader "Custom/Effects/Additive" {
    Properties 
    {
        _Color("Color",Color) = (1,1,1,1)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Intensity("Intensity",Range(0,10))=1
    }
    SubShader {
            Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
            Blend SrcAlpha One
            ColorMask RGB
            Cull Off Lighting Off ZWrite Off



        Pass {

            

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        
        struct appdata {
            float4 vertex : POSITION;
            fixed4 color : COLOR; 
            float2 uv : TEXCOORD0;
            
        };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
            float2 uv : TEXCOORD1;
        };
        
            uniform sampler2D _MainTex;
            uniform fixed4 _MainTex_ST;
            uniform fixed _Intensity;
            uniform fixed4 _Color;
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex );
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            o.color = v.color;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target 
        {
        fixed4 col = tex2D(_MainTex, i.uv)* _Color * i.color*_Intensity;

        return col; 
        
        } 
        ENDCG
    }
}
}