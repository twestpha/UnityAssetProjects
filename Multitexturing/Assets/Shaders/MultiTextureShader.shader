Shader "Custom/MultiTextureShader" {
	Properties {
		[Header(Sub Material 0 (Base Sub Material))]
		[NoScaleOffset] _diff0 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm0 ("Normal", 2D) = "" {}
		_metal0 ("Metallic", Range(0,1)) = 0.0
		_gloss0 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 1)]
		[NoScaleOffset] _mask1 ("Mask", 2D) = "black" {}
		[NoScaleOffset] _diff1 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm1 ("Normal", 2D) = "" {}
		_metal1 ("Metallic", Range(0,1)) = 0.0
		_gloss1 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 2)]
		[NoScaleOffset] _mask2 ("Mask", 2D) = "black" {}
		[NoScaleOffset] _diff2 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm2 ("Normal", 2D) = "" {}
		_metal2 ("Metallic", Range(0,1)) = 0.0
		_gloss2 ("Smoothness", Range(0,1)) = 0.5

		[Header(Sub Material 3)]
		[NoScaleOffset] _mask3 ("Mask", 2D) = "black" {}
		[NoScaleOffset] _diff3 ("Diffuse", 2D) = "white" {}
		[NoScaleOffset] [Normal] _norm3 ("Normal", 2D) = "" {}
		_metal3 ("Metallic", Range(0,1)) = 0.0
		_gloss3 ("Smoothness", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		// Material 0 (Base Material)
		sampler2D _diff0;
		sampler2D _norm0;
		float _metal0;
		float _gloss0;

		// Material 1
		sampler2D _mask1;
		sampler2D _diff1;
		sampler2D _norm1;
		float _metal1;
		float _gloss1;

		// Material 2
		sampler2D _mask2;
		sampler2D _diff2;
		sampler2D _norm2;
		float _metal2;
		float _gloss2;

		// Material 3
		sampler2D _mask3;
		sampler2D _diff3;
		sampler2D _norm3;
		float _metal3;
		float _gloss3;


		struct Input {
			float2 uv_diff0;
		};

		// Surface Function
		void surf (Input IN, inout SurfaceOutputStandard output) {

			float3 grayscalar = float3(0.3, 0.59, 0.11);

			//###################
			// DIFFUSE
			//###################
			fixed3 diffuse = lerp(
				tex2D (_diff0, IN.uv_diff0).rgb,
				tex2D (_diff1, IN.uv_diff0).rgb,
				dot(tex2D (_mask1, IN.uv_diff0), grayscalar) * tex2D (_diff1, IN.uv_diff0).a
			);

			diffuse = lerp(
				diffuse,
				tex2D (_diff2, IN.uv_diff0).rgb,
				dot(tex2D (_mask2, IN.uv_diff0), grayscalar) * tex2D (_diff2, IN.uv_diff0).a
			);

			diffuse = lerp(
				diffuse,
				tex2D (_diff3, IN.uv_diff0).rgb,
				dot(tex2D (_mask3, IN.uv_diff0), grayscalar) * tex2D (_diff3, IN.uv_diff0).a
			);

			//###################
			// NORMAL
			//###################
			fixed3 normal = lerp(
				UnpackNormal(tex2D (_norm0, IN.uv_diff0)).rgb,
				UnpackNormal(tex2D (_norm1, IN.uv_diff0)).rgb,
				dot(tex2D (_mask1, IN.uv_diff0).rgb, grayscalar) * tex2D (_norm1, IN.uv_diff0).a
			);

			normal = lerp(
				normal.rgb,
				UnpackNormal(tex2D (_norm2, IN.uv_diff0)).rgb,
				dot(tex2D (_mask2, IN.uv_diff0).rgb, grayscalar) * tex2D (_norm2, IN.uv_diff0).a
			);

			normal = lerp(
				normal.rgb,
				UnpackNormal(tex2D (_norm3, IN.uv_diff0)).rgb,
				dot(tex2D (_mask3, IN.uv_diff0).rgb, grayscalar) * tex2D (_norm2, IN.uv_diff0).a
			);

			//###################
			// Metallic
			//###################
			fixed3 metal = lerp(
				_metal0,
				_metal1,
				dot(tex2D (_mask1, IN.uv_diff0).rgb, grayscalar)
			);

			metal = lerp(
				metal,
				_metal2,
				dot(tex2D (_mask2, IN.uv_diff0).rgb, grayscalar)
			);

			metal = lerp(
				metal,
				_metal3,
				dot(tex2D (_mask3, IN.uv_diff0).rgb, grayscalar)
			);

			//###################
			// Gloss
			//###################
			fixed3 gloss = lerp(
				_gloss0,
				_gloss1,
				dot(tex2D (_mask1, IN.uv_diff0).rgb, grayscalar)
			);

			gloss = lerp(
				gloss,
				_gloss2,
				dot(tex2D (_mask2, IN.uv_diff0).rgb, grayscalar)
			);

			gloss = lerp(
				gloss,
				_gloss3,
				dot(tex2D (_mask3, IN.uv_diff0).rgb, grayscalar)
			);


			//###################
			// Output
			//###################
			output.Albedo = diffuse;
			output.Normal = normal;
			output.Metallic = metal;
			output.Smoothness = gloss;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
