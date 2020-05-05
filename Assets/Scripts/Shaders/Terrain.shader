Shader "Custom/Terrain" {
	Properties {
		_Textures("Texture Array", 2DArray) = "" {}
		_Tints("Tints", 2D) = "" {}
		_Index("Index", Float) = 1
	}
	SubShader {
		Tags { 
			"RenderPipeline" = "UniversalRenderPipeline"
			"RenderType" = "Opaque"
			"LightMode" = "UniversalForward"
		}
		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "HLSLSupport.cginc"

			struct v2f {
				float4 uv : TEXCOORD0;
				float4 v : SV_POSITION;
				float4 vc : COLOR;
				half4 diff : COLOR1;
			};

			struct Input {
				float4 v : POSITION;
				float4 uv : TEXCOORD2;
				float4 vc : COLOR;
			};

			float _Index;
			sampler2D _Tints;

			v2f vert (Input IN) {
				v2f o;

                o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv.xy = mul(unity_ObjectToWorld, IN.v).xy * .0625; // 16 tiles per tex
				o.vc = IN.vc;

                o.uv.z = (IN.uv.z - IN.uv.z % _Index) / _Index;
				o.uv.a = (IN.uv.a - IN.uv.a % _Index) / _Index;
				o.vc.a = (IN.vc.a - IN.vc.a % _Index) / _Index;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

                return o;
			}

			UNITY_DECLARE_TEX2DARRAY(_Textures);

			half4 frag (v2f i) : SV_Target {
				half4 aT = tex2D(_Tints, float2(i.uv.z * _Index, 0));
				half4 bT = tex2D(_Tints, float2(i.uv.a * _Index, 0));
				half4 cT = tex2D(_Tints, float2(i.vc.a * _Index, 0));

				half4 tA = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.r * aT;
				i.uv.z = i.uv.a;
				half4 tB = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.g * bT;
				i.uv.z = i.vc.a;
				half4 tC = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.b * cT;
				half4 t = tA + tB + tC;
				
				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}