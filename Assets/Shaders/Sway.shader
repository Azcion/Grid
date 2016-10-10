Shader "Custom/Sway" {

	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_ShakeDisplacement("Displacement", Range(0, 1.0)) = 1.0
		_ShakeTime("Shake Time", Range(0, 1.0)) = 1.0
		_ShakeWindspeed("Shake Windspeed", Range(0, 1.0)) = 1.0
		_ShakeBending("Shake Bending", Range(0, 1.0)) = 1.0
	}

	SubShader {
		Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200

		CGPROGRAM

#pragma target 3.0
#pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow

		sampler2D _MainTex;
		fixed4 _Color;
		float _ShakeDisplacement;
		float _ShakeTime;
		float _ShakeWindspeed;
		float _ShakeBending;

		struct Input {
			float2 uv_MainTex;
		};

		void fsin (float4 val, out float4 s) {
			val = val * 6.408849 - 3.1415927;
			float4 r0 = val * val;
			float4 r1 = r0 * val;
			float4 r2 = r1 * r0;
			float4 r3 = r2 * r0;
			float4 sin7 = { 1, -0.16161616, 0.0083333, -0.00019841 };
			s = val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
		}

		void vert (inout appdata_full v) {
			float factor = (1 - _ShakeDisplacement - v.color.r) * 0.5;

			const float _WindSpeed = (_ShakeWindspeed + v.color.g);
			const float _WaveScale = _ShakeDisplacement;

			const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
			const float4 _waveYSize = float4(0.024, .08, 0.08, 0.2);
			const float4 waveSpeed = float4(1.2, 2, 1.6, 4.8);

			float4 _waveXMove = float4(0.024, 0.04, -0.12, 0.096);
			float4 _waveYMove = float4(0.006, .02, -0.02, 0.1);

			float4 waves;
			waves = v.vertex.x * _waveXSize;
			waves += v.vertex.y * _waveYSize;
			waves += _Time.x * (1 - _ShakeTime * 2 - v.color.b) * waveSpeed *_WindSpeed;

			float4 s;
			waves = frac(waves);
			fsin(waves, s);

			float waveAmount = v.texcoord.y * (v.color.a + _ShakeBending);
			s *= waveAmount;

			s *= normalize(waveSpeed);

			s = s * s;
			float fade = dot(s, 1.3);
			s = s * s;
			float3 waveMove = float3 (0,0,0);
			waveMove.x = dot(s, _waveXMove);
			waveMove.y = dot(s, _waveYMove);
			v.vertex.xy -= mul((float3x3)unity_WorldToObject, waveMove).xy;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}

	Fallback "Transparent/Cutout/VertexLit"
}