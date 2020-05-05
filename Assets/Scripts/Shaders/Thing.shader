Shader "Custom/Thing" {
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (0, 0, 0, 1)
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
			float4 _MainTex_ST;
			half4 _Color;

			v2f vert (appdata IN) {
				v2f o;

				o.position = TransformObjectToHClip(IN.v.xyz);
				o.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				o.color = IN.color;

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