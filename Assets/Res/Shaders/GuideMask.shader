Shader "Custom/GuideMask"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1, 1, 1, 1)
    //-------------------add----------------------
    _Center("Center", vector) = (0, 0, 0, 0) // 中心点坐标，x，y有效
    _Width("Width", Float) = 100 // 宽度
    _Height("Height", Float) = 100 // 高度
    _Round("Round", Float) = 10 // 圆角半径
    _Blur("Blur", Float) = 5 // 模糊程度
    //-------------------add----------------------
  }

  SubShader
  {
    Tags
    {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    ZTest [unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha

    Pass
    {
      Name "Default"
    CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma target 2.0

      #include "UnityCG.cginc"
      #include "UnityUI.cginc"

      #pragma multi_compile __ UNITY_UI_CLIP_RECT
      #pragma multi_compile __ UNITY_UI_ALPHACLIP
  #pragma multi_compile _ROUNDMODE_ROUND _ROUNDMODE_ELLIPSE _ROUNDMODE_DYNAMIC_ROUND

      struct appdata_t
      {
        float4 vertex  : POSITION;
        float4 color  : COLOR;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f
      {
        float4 vertex  : SV_POSITION;
        fixed4 color  : COLOR;
        float2 texcoord : TEXCOORD0;
        float4 worldPosition : TEXCOORD1;
        UNITY_VERTEX_OUTPUT_STEREO
      };

      fixed4 _Color;
      fixed4 _TextureSampleAdd;
      float4 _ClipRect;
      //-------------------add----------------------
      half2 _Center;
      half _Width;
      half _Height;
      half _Round;
      half _Blur;
      //-------------------add----------------------

      v2f vert(appdata_t v)
      {
        v2f OUT;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
        OUT.worldPosition = v.vertex;
        OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

        OUT.texcoord = v.texcoord;

        OUT.color = v.color * _Color;
        return OUT;
      }

      sampler2D _MainTex;

      fixed4 frag(v2f IN) : SV_Target
      {
        half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

        #ifdef UNITY_UI_CLIP_RECT
        color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
        #endif

        #ifdef UNITY_UI_ALPHACLIP
        clip (color.a - 0.001);
        #endif

        //-------------------add----------------------
        half round = _Round;
        half blur = _Blur;
        half minX = _Center.x - _Width / 2.0;
        half maxX = _Center.x + _Width / 2.0;
        half minY = _Center.y - _Height / 2.0;
        half maxY = _Center.y + _Height / 2.0;
        half2 p = half2(0, 0);
        if (IN.worldPosition.x <= minX + round)
        {
            if (IN.worldPosition.y <= minY + round) p = half2(minX + round, minY + round);//左下角
            else if (IN.worldPosition.y >= maxY - round) p = half2(minX + round, maxY - round);//左上角
            else p = half2(minX + round, IN.worldPosition.y);//左中
        }
        else if (IN.worldPosition.x >= maxX - round)
        {
            if (IN.worldPosition.y <= minY + round) p = half2(maxX - round, minY + round);//右下角
            else if (IN.worldPosition.y >= maxY - round) p = half2(maxX - round, maxY - round);//右上角
            else p = half2(maxX - round, IN.worldPosition.y);//右中
        }
        else if (IN.worldPosition.y <= minY + round) p = half2(IN.worldPosition.x, minY + round);//中下
        else if (IN.worldPosition.y >= maxY - round) p = half2(IN.worldPosition.x, maxY - round);//中上
        else p = half2(IN.worldPosition.x, IN.worldPosition.y);//中间
        half dis = distance(IN.worldPosition.xy, p.xy);
        color.a = smoothstep(round - blur, round, dis) * color.a;
        //-------------------add----------------------

        return color;
      }
    ENDCG
    }
  }
}