Shader "Custom/Clouds" {
	Properties {
		_Color("Color", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range (0, 1)) = .5
	}
	SubShader {
		Tags { 
			"RenderPipeline" = "UniversalRenderPipeline"
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"LightMode" = "UniversalForward"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Noise.hlsl"

			struct v2f {
				float4 v : SV_POSITION;
				half4 diff : COLOR;
				float2 uv : TEXCOORD;
			};

			struct Input {
				float4 v : POSITION;
				float2 uv : TEXCOORD;
			};

			CBUFFER_START(UnityPerMaterial)
				half3 _Color;
				half _Alpha;
			CBUFFER_END

			v2f vert (Input IN) {
				v2f o;
				o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv = IN.uv;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

				return o;
			}

			half4 frag (v2f i) : SV_Target {
				half2 uv = i.uv * 10;
				half motion = valueFBM4(uv + -_Time.x);
				half ripple = valueFBM2(uv + motion);
				half4 t = half4(_Color, (motion + ripple) - (1 - _Alpha));

				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}