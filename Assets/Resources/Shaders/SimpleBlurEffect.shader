Shader "Custom/SimpleBlurEffect"
{
Properties
{
_MainTex("Texture", 2D) = "white" {}
_BlurSize("Blur size", Float) = 1.0
}
SubShader
{
ZTest Always
cull off
ZWrite off
CGINCLUDE//这个可以使其他pass块都可以使用，而不用在两个pass里都写，减少了写的次数
sampler2D _MainTex;
half4 _MainTex_TexelSize;
float _BlurSize;
struct v2f{
    float4 pos :SV_POSITION;
half2 uv[5]:TEXCOORD0;
};
fixed4 fragBlur(v2f i) :SV_Target{
float weight[3] = {
0.4026,
0.2442,
0.0545
};
fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];
for (int it = 1; it < 3; it++){
    sum += tex2D(_MainTex, i.uv[it]).rgb * weight[it];
sum += tex2D(_MainTex, i.uv[2 * it]).rgb * weight[it];
}
return fixed4(sum, 1.0);
}
ENDCG
Pass{
NAME "GAUSSIAN_BLUR_VERTICAL"//这个pass的唯一名字，可以在其他地方调用，usepass + 名字
CGPROGRAM
#pragma vertex vertlurVertical
#pragma fragment fragBlur
#include "UnityCG.cginc"
v2f vertlurVertical(appdata_img v){
v2f o;
o.pos = UnityObjectToClipPos(v.vertex);
half2 uv = v.texcoord;
o.uv[0] = uv;
o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
return o;
}
ENDCG
}
Pass{
NAME "GAUSSIAN_BLUR_HORIZONTAL"
CGPROGRAM
#pragma vertex vertlurHorizontal
#pragma fragment fragBlur
#include "UnityCG.cginc"
v2f vertlurHorizontal(appdata_img v){
v2f o;
o.pos = UnityObjectToClipPos(v.vertex);
half2 uv = v.texcoord;
o.uv[0] = uv;
o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.x * 1.0) * _BlurSize;
o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.x * 1.0) * _BlurSize;
o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.x * 2.0) * _BlurSize;
o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.x * 2.0) * _BlurSize;
return o;
}
ENDCG
}
}
}