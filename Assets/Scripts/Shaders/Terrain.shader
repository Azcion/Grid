Shader "Custom/Terrain" {
	Properties {
		_Textures("Texture Array", 2DArray) = "" {}
		_Tints("Tints", 2D) = "" {}
		_Index("Index", Float) = 1
	}
	SubShader {
		Tags { 
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct v2f {
				float4 uv : TEXCOORD0;
				float4 v : SV_POSITION;
				float4 vc : COLOR;
				fixed4 diff : COLOR1;
			};

			struct Input {
				float4 v : POSITION;
				float4 uv : TEXCOORD2;
				float4 vc : COLOR;
			};

			float _Index;
			sampler2D _Tints;

			v2f vert (Input IN) {
				v2f o;

                o.v = UnityObjectToClipPos(IN.v);
				o.uv.xy = mul(unity_ObjectToWorld, IN.v) * .0625; // 16 tiles per tex

				float a = IN.uv.z;
				float b = IN.uv.a;
                o.uv.z = (a - a % _Index) / _Index;
				o.uv.a = (b - b % _Index) / _Index;

				o.vc = IN.vc;
				float c = o.vc.a;
				o.vc.a = (c - c % _Index) / _Index;

				// Lighting
				half4 diff = _LightColor0;
				half avg = (diff.r + diff.g + diff.b) / 3;
				o.diff = clamp(half4(avg, avg, avg, 1) + diff, .2, 1);
				
                return o;
			}

			UNITY_DECLARE_TEX2DARRAY(_Textures);

			half4 frag (v2f i) : SV_Target {
				half4 aT = tex2D(_Tints, float2(i.uv.z * _Index, 0));
				half4 bT = tex2D(_Tints, float2(i.uv.a * _Index, 0));
				half4 cT = tex2D(_Tints, float2(i.vc.a * _Index, 0));

				half4 tA = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.r * aT;
				i.uv.z = i.uv.a;
				half4 tB = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.g * bT;
				i.uv.z = i.vc.a;
				half4 tC = UNITY_SAMPLE_TEX2DARRAY(_Textures, i.uv) * i.vc.b * cT;
				half4 t = tA + tB + tC;
				
				// Lighting
				t *= i.diff;

				return t;
			}
			ENDCG
		}
	}
}