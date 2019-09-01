//Basically a copy + paste from tutorial 5, view Phong Shader for comments

Shader "Unlit/Waves"
{
    Properties
    {
        _PointLightColor("Point Light Color", Color) = (0, 0, 0)
        _PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
        _Transparency("Transparency", Range(0.0,0.5)) = 0.25
    }
    SubShader
    {      

        //Added these for transparency of the materials
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            uniform float3 _PointLightColor;
            uniform float3 _PointLightPosition;
            
            struct vertIn
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 color : COLOR;
            };
            
            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float4 worldVertex : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };
            
            vertOut vert(vertIn v) 
            {
                vertOut o;
                float4 waves = float4(0.0f, sin(_Time.y + v.vertex.x)/2, 0.0f, 0.0f);
                v.vertex += waves;
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;

                o.worldVertex = worldVertex;
                o.worldNormal = worldNormal;

                return o;
            }
            
            float _Transparency;

            fixed4 frag(vertOut v) : SV_Target
            {
                float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

                float fAtt = 0.75;
                float Kd = 1;
                float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
                float LdotN = dot(L,  normalize(v.worldNormal));
                float3 dif = Kd* fAtt * _PointLightColor.rgb * v.color.rgb * saturate(LdotN);

                float Ks = 1;
                float specN = 50;
                float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                float3 H = normalize(V + L);
                float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(normalize(v.worldNormal), H)), specN);

                float4 returnColor = float4(.0f, 0.0f, 0.0f, 0.0f);
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = v.color.a + _Transparency;

                return returnColor;
            }
            ENDCG
        }
    }
}
