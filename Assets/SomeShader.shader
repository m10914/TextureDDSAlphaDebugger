Shader "Custom/SomeShader" {

	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags {
			"RenderType"="Opaque"
			"Queue" = "Transparent"
			}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma fragment fragment
			#pragma vertex vertex
			#include "UnityCG.cginc"

			float clipValue;
			float alpha;
			
			sampler2D _MainTex;

			struct vertOUT
			{
				float4 position : POSITION;
				float2 texcoord: TEXCOORD0;
				float3 normal: TEXCOORD1;
			};

			vertOUT vertex (appdata_base IN) 
			{ 
				vertOUT OUT;

				float4 pos = float4(IN.vertex.xyz, 1.0) + float4(IN.normal.xyz,1) * 2;

				OUT.position = mul(UNITY_MATRIX_MVP, pos);
				OUT.texcoord = IN.texcoord.xy;
				OUT.normal = mul(UNITY_MATRIX_MVP, float4(IN.normal.xyz,1));

				return OUT; 
			}

			float4 fragment ( vertOUT IN ) : COLOR
			{
				float4 color = tex2D(_MainTex, IN.texcoord);
				
				clip(color.a - clipValue);
				
				color.a = max(alpha, color.a);
				
				return color * 1;
			}

			ENDCG

		}

	} 
	FallBack "Bumped Diffuse"
}
