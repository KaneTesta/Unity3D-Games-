Shader "Custom/Grass"
{

	// Based off of a grass Geometry shader created by Roystan Ross
	// https://roystan.net/articles/grass-shader.html


    Properties
    {
		//Grass Variables
		[Header(Shading)]
        _TopColor("Top Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,1,1)
		_TranslucentGain("Translucent Gain", Range(0,1)) = 0.5
		_BendRotationRandom("Bend Rotation Random", Range(0, 1)) = 0.8
		_BladeWidth("Blade Width", Float) = 0.05
		_BladeWidthRandom("Blade Width Random", Float) = 0.02
		_BladeHeight("Blade Height", Float) = 0.5
		_BladeHeightRandom("Blade Height Random", Float) = 0.2
		_TessellationUniform("Tessellation Uniform", Range(1, 64)) = 12
		
		//Wind Variables
		_WindDistortionMap("Wind Distortion Map", 2D) = "white" {}
		_WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
		_WindStrength("Wind Strength", Float) = 1

		//Phong Stuff
		_ReflectionCoefficient("Reflection Coefficient", Range(0,1)) = 0.3
		_MainTint("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Specular("Specular Power", Range(0,100)) = 1


    }

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "Autolight.cginc"
	#include "/CustomTessellation.cginc"
	
	//Grass Variables
	float _BendRotationRandom;
	float _BladeHeight;
	float _BladeHeightRandom;	
	float _BladeWidth;
	float _BladeWidthRandom;

	//Wind Variables
	sampler2D _WindDistortionMap;
	float4 _WindDistortionMap_ST;
	float2 _WindFrequency;
	float _WindStrength;



	// Simple noise function, sourced from http://answers.unity.com/answers/624136/view.html
	// Returns a number in the 0...1 range.
	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	}

	// Construct a rotation matrix that rotates around the provided axis, sourced from:
	// https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
	float3x3 AngleAxis3x3(float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);

		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;

		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c
			);
	}

	struct geometryOutput
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	geometryOutput VertexOutput(float3 pos, float2 uv)
	{
		geometryOutput o;
		o.pos = UnityObjectToClipPos(pos);
		o.uv = uv;
		return o;
	}

	[maxvertexcount(3)]
	//Geometry shader that takes a triangle as an input, and sets up out shader to output a stream of triangles
	//geometry output structure contains data for each vertex
	void geo(triangle vertexOutput IN[3] : SV_POSITION, inout TriangleStream<geometryOutput> triStream)
	{
		float3 pos = IN[0].vertex;

		//Vector created from cross product of normal and tangent, returning perpendicular vector for tangent space
		float3 vNormal = IN[0].normal;
		float4 vTangent = IN[0].tangent;
		float3 vBinormal = cross(vNormal, vTangent) * vTangent.w;

		//Matrix so we can transform b/w tangent and local space, to be multiplied with vertex of grass before put into local space
		float3x3 tangentToLocal = float3x3(
		vTangent.x, vBinormal.x, vNormal.x,
		vTangent.y, vBinormal.y, vNormal.y,
		vTangent.z, vBinormal.z, vNormal.z
		);
		
		//Ensure every blade of grass is facing a random direction
		float3x3 facingRotationMatrix = AngleAxis3x3(rand(pos) * UNITY_TWO_PI, float3(0, 0, 1));
		float3x3 bendRotationMatrix = AngleAxis3x3(rand(pos.zzx) * _BendRotationRandom * UNITY_PI * 0.5, float3(-1, 0, 0));
		
		// WIND STUFF
		float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * _Time.y;
		float2 windSample = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength;
		float3 wind = normalize(float3(windSample.x, windSample.y, 0));
		float3x3 windRotation = AngleAxis3x3(UNITY_PI * windSample, wind);
		float3x3 transformationMatrix = mul(mul(mul(tangentToLocal, windRotation), facingRotationMatrix), bendRotationMatrix);
		

		//Properties to randomise width and height of grass blade
		float height = (rand(pos.zyx) * 2 - 1) * _BladeHeightRandom + _BladeHeight;
		float width = (rand(pos.xzy) * 2 - 1) * _BladeWidthRandom + _BladeWidth;
		
		// Pos adds an offset to the triangle vertices so theyre spread out
		triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(width, 0, 0)), float2(0, 0)));
		triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(-width, 0, 0)), float2(1, 0)));
		triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(0, 0, height)), float2(0.5, 1)));
	}
	

	ENDCG

    SubShader
    {
		Cull Off

        Pass
        {
			Tags
			{
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 4.6
			#pragma geometry geo
			#pragma hull hull
			#pragma domain domain
            
			#include "Lighting.cginc"

			float4 _TopColor;
			float4 _BottomColor;
			float _TranslucentGain;

			float4 frag (geometryOutput i, fixed facing : VFACE) : SV_Target
            {	
				return lerp(_BottomColor, _TopColor, i.uv.y);
            }
            ENDCG
        }
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