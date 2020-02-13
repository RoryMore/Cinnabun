Shader "Unlit/Sharder1"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_fresTex("Texture", 2D) = "white" {}
		_Color("color",Color) = (1,1,1,1)
[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 1
	}
		SubShader
		{ Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

			Pass
			{
			 Blend OneMinusDstColor One
	Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }

	LOD 100
				CGPROGRAM


				#pragma vertex vertFunc
				#pragma fragment fragFunc

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float4 normal : NORMAL;
					float2 uv : TEXCOORD0;
					
				};
		
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : TEXCOORD1;
				
			};


			//var
			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _fresTex;
			float _FresnelExponent;

			v2f vertFunc(appdata IN)
			{
				v2f Out;
				Out.position = UnityObjectToClipPos(IN.vertex);
				Out.uv = IN.uv;

				float viewDir = normalize(ObjSpaceViewDir(IN.vertex));



				float3 posWorld = mul(unity_ObjectToWorld, IN.vertex).xyz;

				Out.color = smoothstep(1 - 0.7, 1.0, (1 - saturate(dot(IN.normal,viewDir))))*.5f;
				Out.color = _Color;
				return Out;
			}

			fixed4 fragFunc(v2f IN) : SV_Target
			{
				fixed4 o;
				fixed4 colo = tex2D(_MainTex, IN.uv.xy);

				o = colo;
				o.rgb += IN.color* _FresnelExponent;


				return o;
			}
			ENDCG
		}
		}
}
