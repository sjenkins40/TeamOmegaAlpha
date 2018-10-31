Shader "Custom/CelShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BlurWidth ("Cel Blur Width", Range(0,2)) = 0.2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Toon fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RampTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _BlurWidth;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir,fixed atten)
		{
			half NdotL = dot(s.Normal, lightDir);  //Value between 0 and 1

	        half cel;

	        /// 0 | threshold 1  |  blur  | threshold 2 | 1
	        /// 0 |**************|<- .5 ->|xxxxxxxxxxxxx| 1

			if (NdotL < 0.5 - _BlurWidth / 2)                                         // Outside of the blur but dark
	            cel = 0;
			else if (NdotL > 0.5 + _BlurWidth / 2)                                    // Outside of the blur but lit
	            cel = 1;
			else                                                                                // Inside of the blur 
	            cel = 1- ((0.5 + _BlurWidth / 2 - NdotL) / _BlurWidth);

			half4 c;

			c.rgb = (cel + 0.3)/2.5  * s.Albedo * _LightColor0.rgb * atten; // So it does not look too lit
			c.a = s.Alpha;

			return c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
