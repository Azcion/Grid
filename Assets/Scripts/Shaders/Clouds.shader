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

			half rand (half2 uv) {
				const half2 other = half2(51.237452, 67.839726);

				return frac(sin(dot(uv, other) * 929) * 971);
			}

			half noise (half2 uv) {
				half2 i = floor(uv);
				half2 f = frac(uv);

				const half2 plusX = half2(1, 0);
				const half2 plusY = half2(0, 1);
				const half2 plusXY = half2(1, 1);

				half a = rand(i);
				half b = rand(i + plusX);
				half c = rand(i + plusY);
				half d = rand(i + plusXY);
				half2 q = f * f * (3 - 2 * f);

				return lerp(a, b, q.x) + (c - a) * q.y * (1 - q.x) + (d - b) * q.x * q.y;
			}

			half fbm (half2 uv) {
				half value = 0;
				half scale = .5;

				for (int i = 0; i < 3; ++i) {
					value += noise(uv) * scale;
					uv *= 2;
					scale *= .5;
				}

				return value;
			}

			half4 frag (v2f i) : SV_Target {
				half2 uv = i.uv * 10;
				half motion = fbm(uv + -_Time.x * 2);
				half ripple = fbm(uv + motion);
				half4 t = half4(_Color, (motion + ripple) - (1 - _Alpha));

				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}