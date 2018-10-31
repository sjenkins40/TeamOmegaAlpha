Shader "Custom/ToonShader" {
	Properties {
		_MainTex("Texture", 2D) = "white"
//		_RampTex ("Ramp", 2D) = "white" {} 
		_CelShadingLevels("Cel Shading Levels", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Toon
		#pragma surface surf CustomLambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half _CelShadingLevels;
//		sampler2D _RampTex;

		struct Input {
			float2 uv_MainTex;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb; 
		}

		half4 LightingCustomLambert (SurfaceOutput s, half3 lightDir, 
                half3 viewDir, half atten) 
		{ 
		  half NdotL = dot (s.Normal, lightDir); 

		  // Snap instead
		  half cel = ceil(NdotL * _CelShadingLevels) / 
		             (_CelShadingLevels - 0.5); 

		  // Next, set what color should be returned
		  half4 color; 

		  color.rgb = s.Albedo * _LightColor0.rgb * (cel * atten); 
		  color.a = s.Alpha; 

		  // Return the calculated color
		  return color; 
		} 

//		fixed4 LightingToon (SurfaceOutput s, fixed3 lightDir, 
//            fixed atten) 
//		{ 
//		  // First calculate the dot product of the light direction and the 
//		  // surface's normal
//		  half NdotL = dot(s.Normal, lightDir); 
//		      
//		  // Remap NdotL to the value on the ramp map
//		  NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5)); 
//		 
//		  // Next, set what color should be returned
//		  half4 color; 
//
//		  color.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten ); 
//		  color.a = s.Alpha; 
//
//		  // Return the calculated color
//		  return color; 
//		} 
		ENDCG
	}
	FallBack "Diffuse"
}
