Shader "Custom/Rain" {
	Properties {
		_Texture("Texture", 2D) = "" {}
		_Alpha("Alpha", Range(0, 1)) = .5
		_Scale("Scale", Float) = .2
		_Speed("Speed", Float) = 5
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

			sampler2D _Texture;

			CBUFFER_START(UnityPerMaterial)
				half _Alpha;
				half _Scale;
				half _Speed;
			CBUFFER_END

			v2f vert (Input IN) {
				v2f o;
				o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv = mul(unity_ObjectToWorld, IN.v).xy;

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
				const half cos81deg = 0.15643446504;
				const half sin81deg = 0.98768834059;
				const half2 angle = half2(cos81deg, sin81deg);

				half travel = _Time.w * _Speed;
				half2 uv = i.uv + travel * angle;
				uv *= _Scale;
				half4 color = tex2D(_Texture, uv);

				half4 t = half4(color.rgb, color.a * _Alpha);

				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}