// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33727,y:32635,varname:node_9361,prsc:2|emission-674-OUT,custl-4975-OUT;n:type:ShaderForge.SFN_Tex2d,id:1273,x:32555,y:32634,ptovrint:False,ptlb:tex,ptin:_tex,varname:node_1273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:202dddb891daff54d81b84e5762cbf8b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2015,x:32806,y:32634,varname:node_2015,prsc:2|A-1273-RGB,B-1273-A;n:type:ShaderForge.SFN_Panner,id:9205,x:32540,y:32903,varname:node_9205,prsc:2,spu:0,spv:0.2|UVIN-14-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:14,x:32370,y:32903,varname:node_14,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:1745,x:32716,y:32933,ptovrint:False,ptlb:uv_1,ptin:_uv_1,varname:node_1745,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a0a596b3169e4aef9d48ae014275c451,ntxv:0,isnm:False|UVIN-9205-UVOUT;n:type:ShaderForge.SFN_Multiply,id:6284,x:33176,y:33004,varname:node_6284,prsc:2|A-4013-OUT,B-5596-OUT,C-2015-OUT;n:type:ShaderForge.SFN_Panner,id:2623,x:32563,y:33103,varname:node_2623,prsc:2,spu:0.3,spv:-0.3|UVIN-14-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1757,x:32716,y:33139,ptovrint:False,ptlb:uv_2,ptin:_uv_2,varname:node_1757,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:dec2e76c32c340a2a6b394c9ad42e65c,ntxv:0,isnm:False|UVIN-2623-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4975,x:33445,y:32965,varname:node_4975,prsc:2|A-6284-OUT,B-8198-RGB,C-3692-OUT,D-9715-A;n:type:ShaderForge.SFN_Color,id:2103,x:32716,y:33333,ptovrint:False,ptlb:uv_2_color,ptin:_uv_2_color,varname:node_2103,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:674,x:33172,y:32614,varname:node_674,prsc:2|A-6951-RGB,B-2015-OUT,C-9715-A;n:type:ShaderForge.SFN_Color,id:6951,x:32980,y:32510,ptovrint:False,ptlb:tex_color,ptin:_tex_color,varname:node_6951,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5596,x:32971,y:33180,varname:node_5596,prsc:2|A-1757-RGB,B-2103-RGB;n:type:ShaderForge.SFN_Multiply,id:4013,x:32952,y:32891,varname:node_4013,prsc:2|A-6161-RGB,B-1745-RGB;n:type:ShaderForge.SFN_Color,id:6161,x:32716,y:32789,ptovrint:False,ptlb:uv_1_color,ptin:_uv_1_color,varname:node_6161,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:8198,x:33162,y:33268,ptovrint:False,ptlb:uv_ap,ptin:_uv_ap,varname:node_8198,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:3692,x:33176,y:33170,ptovrint:False,ptlb:uv_vlue,ptin:_uv_vlue,varname:node_3692,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_VertexColor,id:9715,x:33109,y:32784,varname:node_9715,prsc:2;proporder:6951-1273-6161-1745-2103-1757-8198-3692;pass:END;sub:END;*/

Shader "Custom/Effects/UV_ap" {
    Properties {
        _tex_color ("tex_color", Color) = (1,1,1,1)
        _tex ("tex", 2D) = "white" {}
        [HDR]_uv_1_color ("uv_1_color", Color) = (0.5,0.5,0.5,1)
        _uv_1 ("uv_1", 2D) = "white" {}
        [HDR]_uv_2_color ("uv_2_color", Color) = (1,1,1,1)
        _uv_2 ("uv_2", 2D) = "white" {}
        _uv_ap ("uv_ap", 2D) = "white" {}
        _uv_vlue ("uv_vlue", Float ) = 5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _tex; uniform float4 _tex_ST;
            uniform sampler2D _uv_1; uniform float4 _uv_1_ST;
            uniform sampler2D _uv_2; uniform float4 _uv_2_ST;
            uniform float4 _uv_2_color;
            uniform float4 _tex_color;
            uniform float4 _uv_1_color;
            uniform sampler2D _uv_ap; uniform float4 _uv_ap_ST;
            uniform float _uv_vlue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _tex_var = tex2D(_tex,TRANSFORM_TEX(i.uv0, _tex));
                float3 node_2015 = (_tex_var.rgb*_tex_var.a);
                float3 emissive = (_tex_color.rgb*node_2015*i.vertexColor.a);
                float4 node_3036 = _Time;
                float2 node_9205 = (i.uv0+node_3036.g*float2(0,0.2));
                float4 _uv_1_var = tex2D(_uv_1,TRANSFORM_TEX(node_9205, _uv_1));
                float2 node_2623 = (i.uv0+node_3036.g*float2(0.3,-0.3));
                float4 _uv_2_var = tex2D(_uv_2,TRANSFORM_TEX(node_2623, _uv_2));
                float4 _uv_ap_var = tex2D(_uv_ap,TRANSFORM_TEX(i.uv0, _uv_ap));
                float3 finalColor = emissive + (((_uv_1_color.rgb*_uv_1_var.rgb)*(_uv_2_var.rgb*_uv_2_color.rgb)*node_2015)*_uv_ap_var.rgb*_uv_vlue*i.vertexColor.a);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
