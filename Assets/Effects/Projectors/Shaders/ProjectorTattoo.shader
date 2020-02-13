Shader "Projector/Tattoo" {
	Properties{
		_ShadowTex("Cookie", 2D) = "white" {}
		// Added Falloff tex
		_FalloffTex("FallOff", 2D) = "white" {}
		_FillTex("CookieFill", 2D) = "white" {}
		_Progress("Progress", Range(0.0,1.0)) = 0.0
		_SkillType("SkillType", Int) = 0	// 1 = LINEAR "BASE2END" | 2 = CIRCULAR "IN2OUT"
	}

		Subshader{
			Tags {
				"RenderType" = "Transparent"
				"Queue" = "Transparent+100"
			}
			Pass {
				ZWrite Off
				Offset -1, -1

				Fog{ Mode Off }

				ColorMask RGB
				Blend OneMinusSrcAlpha SrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_fog_exp2
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 uv : TEXCOORD0;
					// Added uvFalloff
					float4 uvFalloff : TEXCOORD1;
				};

				sampler2D _ShadowTex;
				// Added _FalloffTex;
				sampler2D _FalloffTex;
				sampler2D _FillTex;
				float4x4 unity_Projector;
				// Added unity_ProjectorClip
				float4x4 unity_ProjectorClip;
				float4 _Color;
				float _Progress;
				int _SkillType;

				const float pi = 3.141592653589793238462;

				v2f vert(appdata_tan v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = mul(unity_Projector, v.vertex);
					// Added o.uvFalloff multiply
					o.uvFalloff = mul(unity_ProjectorClip, v.vertex);
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					half4 tex = tex2Dproj(_ShadowTex, i.uv);
					half4 texF = tex2Dproj(_FalloffTex, i.uvFalloff);
					half4 texPF = tex2Dproj(_FillTex, i.uv);

					tex.a = 1 - tex.a;
					//tex.a = 1 - texF.a;
					if (i.uv.w < 0)
					{
						tex = float4(0, 0, 0, 0);
					}
					// 'Shadow' creeps up objects infinitely. Add a cap to this, essentially reverse falloff

					//=== CIRCULAR FILL FROM INSIDE TO OUT METHOD ========
					float2 uvPoint;
					uvPoint.x = i.uv.x;
					uvPoint.y = i.uv.y;
					float2 centre;

					if (_SkillType == 1)
					{
						// Centre is set to be at the end of the image, where the unit should be casting from
						centre.x = 0.5f;
						centre.y = 0.0f;
					}
					else if (_SkillType == 2)
					{
						// Centre is set to be in the middle of the image so the fill expands outward
						centre.x = 0.5f;
						centre.y = 0.5f;
					}
					// If distance is less than or equal to _Progress/radius, set tex.rgb to texPF.rgb
					float d = distance(uvPoint, centre);
					if (d < _Progress)
					{
						tex.rgb = texPF.rgb;
					}

					half4 res = lerp(half4(0, 0, 0, 1), tex, texF.a);

					return res;
				}
				ENDCG
			}

	}

}
//=== LINEAR FILL FROM i.uv.y=0 TO i.uv.y=1 =====================
						// Calculate Texture Fill Offset
						//texPF.a *= i.uv.y < _Progress;

						//if (texPF.a == 1)
						//{
							//tex.rgb = texPF.rgb;
						//}
						// -------------
						// ============================================================