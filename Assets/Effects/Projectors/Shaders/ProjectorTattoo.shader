Shader "Projector/Tattoo" {
	Properties{
		_ShadowTex("Cookie", 2D) = "white" {}
		// Added Falloff tex
		_FalloffTex("FallOff", 2D) = "white" {}
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
				float4x4 unity_Projector;
				// Added unity_ProjectorClip
				float4x4 unity_ProjectorClip;
				float4 _Color;

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
					tex.a = 1 - tex.a;
					//tex.a = 1 - texF.a;
					if (i.uv.w < 0)
					{
						tex = float4(0, 0, 0, 0);
					}

					half4 res = lerp(half4(0, 0, 0, 1), tex, texF.a);

					
					//return tex;
					return res;
				}
				ENDCG
			}
	}
}