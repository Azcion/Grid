Shader "Custom/Thing" {
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (0, 0, 0, 1)
	}
	SubShader {
		Tags { 
			"RenderType" = "Transparent" 
			"Queue" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull off
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct appdata {
				float4 v : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				fixed4 diff : COLOR1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			v2f vert (appdata IN) {
				v2f o;

				o.position = UnityObjectToClipPos(IN.v);
				o.uv = TRANSFORM_TEX(IN.uv, _MainTex);
				o.color = IN.color;

				// Lighting
				half4 diff = _LightColor0;
				half avg = (diff.r + diff.g + diff.b) / 3;
				o.diff = clamp(half4(avg, avg, avg, 1) + diff, .2, 1);

				return o;
			}

			fixed4 frag (v2f i) : SV_TARGET {
				fixed4 t = tex2D(_MainTex, i.uv);
				t *= _Color;
				t *= i.color;
				
				// Lighting
				t *= i.diff;

				return t;
			}
			ENDCG
		}
	}
}