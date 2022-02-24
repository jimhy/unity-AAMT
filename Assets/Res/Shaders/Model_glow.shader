// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-9797-OUT,custl-6175-OUT;n:type:ShaderForge.SFN_Tex2d,id:7210,x:32679,y:32764,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_7210,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d4e45893b27236946b5316a52ce52282,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6175,x:32988,y:32932,varname:node_6175,prsc:2|A-2273-RGB,B-5076-OUT,C-6847-RGB;n:type:ShaderForge.SFN_ValueProperty,id:5076,x:32679,y:33142,ptovrint:False,ptlb:lighting_value,ptin:_lighting_value,varname:node_5076,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Tex2d,id:2273,x:32679,y:32961,ptovrint:False,ptlb:Lighting_Texture,ptin:_Lighting_Texture,varname:node_2273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:636d72a4cde441e439e68daa2ecf536f,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9797,x:32928,y:32684,varname:node_9797,prsc:2|A-472-RGB,B-7210-RGB;n:type:ShaderForge.SFN_Color,id:472,x:32679,y:32598,ptovrint:False,ptlb:Texture_color,ptin:_Texture_color,varname:node_472,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:6847,x:32679,y:33216,ptovrint:False,ptlb:lighting_color,ptin:_lighting_color,varname:node_6847,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:472-7210-6847-2273-5076;pass:END;sub:END;*/

Shader "Custom/Effects/Lighting_model" {
    Properties {
        [HDR]_Texture_color ("Texture_color", Color) = (0.5,0.5,0.5,1)
        _Texture ("Texture", 2D) = "white" {}
        [HDR]_lighting_color ("lighting_color", Color) = (0.5,0.5,0.5,1)
        _Lighting_Texture ("Lighting_Texture", 2D) = "black" {}
        _lighting_value ("lighting_value", Float ) = 5
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _lighting_value;
            uniform sampler2D _Lighting_Texture; uniform float4 _Lighting_Texture_ST;
            uniform float4 _Texture_color;
            uniform float4 _lighting_color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = (_Texture_color.rgb*_Texture_var.rgb);
                float4 _Lighting_Texture_var = tex2D(_Lighting_Texture,TRANSFORM_TEX(i.uv0, _Lighting_Texture));
                float3 finalColor = emissive + (_Lighting_Texture_var.rgb*_lighting_value*_lighting_color.rgb);
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
