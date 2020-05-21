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

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 v : SV_POSITION;
				half4 diff : COLOR;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
			};

			struct Input {
				float4 v : POSITION;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
			};

			sampler2D _Tints;

			CBUFFER_START(UnityPerMaterial)
				float _Index;
			CBUFFER_END

			v2f vert (Input IN) {
				v2f o;
				o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv = mul(unity_ObjectToWorld, IN.v).xy * .0625; // 16 tiles per tex
				o.uv2 = IN.uv2;

				o.uv3.x = (IN.uv3.x - IN.uv3.x % _Index) / _Index;
				o.uv3.y = (IN.uv3.y - IN.uv3.y % _Index) / _Index;
				o.uv3.z = (IN.uv3.z - IN.uv3.z % _Index) / _Index;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

				return o;
			}

			TEXTURE2D_ARRAY(_Textures);
			SAMPLER(sampler_Textures);

			half4 frag (v2f i) : SV_Target {
				half4 aT = tex2D(_Tints, float2(i.uv3.x * _Index, 0));
				half4 bT = tex2D(_Tints, float2(i.uv3.y * _Index, 0));
				half4 cT = tex2D(_Tints, float2(i.uv3.z * _Index, 0));

				half2 uv = half2(i.uv.x, i.uv.y);
				half4 color;
				color = SAMPLE_TEXTURE2D_ARRAY(_Textures, sampler_Textures, uv, half1(i.uv3.x));
				half4 tA =  color * i.uv2.x * aT;
				color = SAMPLE_TEXTURE2D_ARRAY(_Textures, sampler_Textures, uv, half1(i.uv3.y));
				half4 tB =  color * i.uv2.y * bT;
				color = SAMPLE_TEXTURE2D_ARRAY(_Textures, sampler_Textures, uv, half1(i.uv3.z));
				half4 tC =  color * i.uv2.z * cT;
				half4 t = tA + tB + tC;
				
				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}