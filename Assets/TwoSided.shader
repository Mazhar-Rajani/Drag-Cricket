Shader "Custom/TwoSided"
{
	Properties
	{
		_Albedo1("Albedo (RGB)", 2D) = "white" {}
		_Albedo2("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Pass{
			Tags {"RenderType" = "Opaque"}
			Cull back
			SetTexture[_Albedo1] { combine texture }
		}

		Pass{
			Tags {"RenderType" = "Opaque"}
			Cull front
			SetTexture[_Albedo2] { combine texture }
		}
	}
	FallBack "Diffuse"
}