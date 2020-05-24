Shader "Custom/Cover" {
	Properties {
		_Texture("Texture", 2D) = "" {}
		//_Tints
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
				float4 uv2 : TEXCOORD2;
			};

			struct Input {
				float4 v : POSITION;
				float4 uv2 : TEXCOORD2;
			};

			v2f vert (Input IN) {
				v2f o;
				o.v = TransformObjectToHClip(IN.v.xyz);
				o.uv2 = IN.uv2;

				//// Lighting ////
				half3 diff = GetMainLight().color;
				// muten sun color
				half avg = (diff.r + diff.g + diff.b) / 3;
				half3 d = clamp(half3(avg, avg, avg) + diff, .2, 1);
				// half3 to half4
				o.diff = half4(d.r, d.g, d.b, 1);

				return o;
			}

			sampler2D _Texture;

			half4 frag (v2f i) : SV_Target {
				const float2 uv = float2(0, 0);
				half4 color = tex2D(_Texture, uv);
				color.a = i.uv2.x;
				
				//// Lighting ////
				color *= i.diff;

				return color;
			}
			ENDHLSL
		}
	}
}