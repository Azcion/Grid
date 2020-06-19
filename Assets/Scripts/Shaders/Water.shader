Shader "Custom/Water" {
	Properties {
		_Texture("Texture", 2D) = "" {}
		_Noise("Noise", 2D) = "" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range (0, 1)) = .5
		_Distortion("Distortion", Float) = 1
		_NoiseScrollVelocity("NoiseScrollVelocity", Float) = 1
		_NoiseScale("NoiseScale", Float) = 1
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
			sampler2D _Noise;

			CBUFFER_START(UnityPerMaterial)
				half3 _Color;
				half _Alpha;
				half _Distortion;
				half _NoiseScrollVelocity;
				half _NoiseScale;
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
				half2 uv = i.uv + -_Time.x * 6;
				half2 waveUV = uv * _NoiseScale;
				half2 travel = _NoiseScrollVelocity * _Time.x;
				uv *= .0625;
				uv += _Distortion * (tex2D(_Noise, waveUV + travel).rg - .5);
				waveUV += .2;
				uv += _Distortion * (tex2D(_Noise, waveUV - travel).rg - .5);
				half4 color = tex2D(_Texture, uv);

				half4 t = half4(color.rgb * _Color, _Alpha);

				//// Lighting ////
				t *= i.diff;

				return t;
			}
			ENDHLSL
		}
	}
}