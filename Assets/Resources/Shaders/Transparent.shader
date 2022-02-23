Shader "Custom/Effects/Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaScale("AlphaScale",Range(0,1))=0.5
    }
    SubShader
    {
            Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
       
         

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            
            Lighting Off ZWrite On
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
      

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
      
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed _AlphaScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
          
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
               
      
                return fixed4 (col.rgb,col.a*_AlphaScale);
            }
            ENDCG
        }
    }
}
