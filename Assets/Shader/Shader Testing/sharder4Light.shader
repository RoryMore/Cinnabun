// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _World2Object', a built-in variable
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Sharder 4"
{
	Properties
	{
		
		_Color("color",Color) = (1,1,1,1)

		
	}
		SubShader
		{
			Pass
			{
			Tags{"LightMode" = "ForwardBase"}
				CGPROGRAM
				#pragma vertex vertFunc
				#pragma fragment fragFunc

				

			//var
			uniform float4 _Color;

			uniform float4 _lightColor0;
			// uniform float4x4 _World2Object;
			// uniform float4x4 _Object2World;
			//float4 _WorldSpaceLightPos0;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;

			};

			struct vertexOutput
			{
			float4 position : SV_POSITION;
			float4 col : COLOR;
			};


			//Vertez shader
		vertexOutput vertFunc(vertexInput IN)
		{
			vertexOutput Out;

			//light
			float3 normalDirection = normalize(mul(float4(IN.normal, 0.0), unity_WorldToObject).xyz);
			float3 lightDirection;
			float LightA = 1.0f;

			lightDirection = normalize(_WorldSpaceLightPos0.xyz);

			float3 diffuseRelflection = LightA * _lightColor0.xyz * _Color.rgb * max(0.0 , dot(normalDirection, lightDirection));
			float3 lightFinal = diffuseRelflection + UNITY_LIGHTMODEL_AMBIENT.xyz;



			Out.position = UnityObjectToClipPos(IN.vertex);
			Out.col = float4(lightFinal * _Color.rgb, 1.0);

			return Out;
		}

		//fragment shader
		fixed4 fragFunc(vertexOutput IN) : SV_Target
		{



			return	IN.col;


		}
		ENDCG
	}
		}
}