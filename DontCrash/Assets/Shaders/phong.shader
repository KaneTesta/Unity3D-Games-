Shader "PhongShader" {


	Properties{
		_ReflectionCoefficient("Reflection Coefficient", Range(0,1)) = 0.3
		_MainTint("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Specular("Specular Power", Range(0,100)) = 1

	}

	SubShader{

		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _MainTex;
		float4 _MainTint;
		float _Specular;

		//Phong shading: 
		//Inputs: Incoming Light vector, camera direction vector
		// Based off https://docplayer.net/61098261-Introduction-to-surface-shaders-prof-aaron-lanterman-school-of-electrical-and-computer-engineering-georgia-institute-of-technology.html
		// due to being able to choose materials as I cant change vertex colours from imported assets

		float4 LightingBlinnPhong(SurfaceOutput a, float3 lightVec, float3 camVec,
			float3 normal, float4 color,float atten) {

			lightVec = normalize(lightVec);
			camVec = normalize(camVec);

			float diffuse = dot(normal, lightVec);
			float specular = pow(saturate(dot(normalize(lightVec + camVec), normal)), _Specular) * color.a;

			float4 v;
			v.rgb = (color.rgb * _MainTint.rgb * diffuse +
				specular) * (atten * 2);

			v.a = specular * atten;
			return v;
		}

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			//Specular is the reflection from the light
			o.Specular = _Specular;
			//Albedo is the base color of the surface
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _MainTint;
		}

		ENDCG
	}
 
	FallBack "Diffuse"
}