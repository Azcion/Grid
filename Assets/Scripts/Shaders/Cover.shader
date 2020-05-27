Shader "Custom/Cover" {
	Properties {
		_Bases("Bases", 2D) = "" {}
		_Tints("Tints", 2D) = "" {}
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
				float4 v : SV_POSITION;
				half4 diff : COLOR;
				float1 uv2 : TEXCOORD2;
				float1 uv3 : TEXCOORD3;
			};

			struct Input {
				float4 v : POSITION;
				float1 uv2 : TEXCOORD2;
				float1 uv3 : TEXCOORD3;
			};

			v2f vert (Input IN) {
				v2f o;
				o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv2.x = IN.uv2.x;
				o.uv3.x = IN.uv3.x;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

				return o;
			}

			sampler2D _Bases;
			sampler2D _Tints;

			half4 frag (v2f i) : SV_Target {
				half4 base = tex2D(_Bases, float2(i.uv2.x, 0));
				half4 tint = tex2D(_Tints, float2(i.uv3.x, 0));
				half4 t = base * tint;
				
				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}