Shader "ToonShader" {


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

		//Inputs: Incoming Light vector, camera direction vector
		// Based off https://docplayer.net/61098261-Introduction-to-surface-shaders-prof-aaron-lanterman-school-of-electrical-and-computer-engineering-georgia-institute-of-technology.html
		// My new ToonShader allows me to use colors of materials rather than vertices
		// This is because I can't change vertex colors of imported assets
		
		float4 Toon(SurfaceOutput a, float3 lightVector, float3 cameraVector,
			float3 normal, float4 color,float atten) {

			lightVector = normalize(lightVector);
			cameraVector = normalize(cameraVector);

			float diffuse = dot(normal, lightVector);
			float specular = pow(saturate(dot(normalize(lightVector + cameraVector), normal)), _Specular) * color.a;

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