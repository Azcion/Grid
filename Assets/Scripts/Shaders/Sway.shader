Shader "Custom/Sway" {
	Properties {
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_ShakeWindspeed("Shake Windspeed", Range(0, 10.0)) = 1.0
		_ShakeBending("Shake Bending", Range(0, 10.0)) = 1.0
	}
	SubShader {
		Tags { 
			"RenderPipeline" = "UniversalRenderPipeline"
			"RenderType" = "Transparent" 
			"Queue" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull off
		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			struct appdata {
				float4 v : POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
				half4 diff : COLOR1;
			};

			sampler2D _MainTex;

			CBUFFER_START(UnityPerMaterial)
				float4 _MainTex_ST;
				half4 _Color;
				float _ShakeWindspeed;
				float _ShakeBending;
			CBUFFER_END

			void fsin (float4 val, out float4 s) {
				val = val * 6.408849 - 3.1415927;
				float4 r0 = val * val;
				float4 r1 = r0 * val;
				float4 r2 = r1 * r0;
				float4 r3 = r2 * r0;
				const float4 sin7 = { 1, -0.16161616, 0.0083333, -0.00019841 };
				s = val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
			}

			v2f vert (appdata IN) {
				v2f o;

				o.position = TransformObjectToHClip(IN.v.xyz);
				o.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				o.color = IN.color;

				const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
				const float4 _waveYSize = float4(0.024, .08, 0.08, 0.2);
				const float4 _waveSpeed = float4(1.2, 2, 1.6, 4.8);
				const float4 _waveXMove = float4(0.024, 0.04, -0.12, 0.096);
				const float4 _waveYMove = float4(0.006, .02, -0.02, 0.1);

				float4 waves;
				waves = IN.v.x * _waveXSize;
				waves += IN.v.y * _waveYSize;
				waves -= _Time.x * _waveSpeed * _ShakeWindspeed;

				float4 s;
				waves = frac(waves);
				fsin(waves, s);

				float waveAmount = IN.uv.y * _ShakeBending;
				s *= waveAmount;
				s *= normalize(_waveSpeed);
				s *= s;
				float fade = dot(s, 1.3);
				s *= s;
				float3 waveMove = float3 (0,0,0);
				waveMove.x = dot(s, _waveXMove);
				waveMove.y = dot(s, _waveYMove);
				o.position.xy -= mul((float3x3)unity_WorldToObject, waveMove).xy;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

				return o;
			}

			half4 frag (v2f i) : SV_TARGET {
				half4 t = tex2D(_MainTex, i.uv);
				t *= _Color;
				t *= i.color;
				
				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}