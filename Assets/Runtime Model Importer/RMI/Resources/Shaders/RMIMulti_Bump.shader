Shader "RMI/RMIMulti_Bump" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0,0,0, 1)
	_Emission ("Emisive Color", Color) = (0,0,0, 1)	
	_Shininess ("Shininess", Range (0.01, 1)) = 0
	_MainTex ("Base (RGBA)", 2D) = "white" {}
	_AlphaTex ("Alpha (RGB)", 2D) = "white" {}
	_SpecTex ("Specular", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}

SubShader {
	Tags { "RenderType"="Opaque" }
	Tags {Queue = Transparent}
	//ZWrite On
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 300
        
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma exclude_renderers flash

sampler2D _MainTex;
sampler2D _AlphaTex;
sampler2D _SpecTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed4 _Emission;
fixed4 _Specular;
half _Shininess;
sampler2D _Illum;

struct Input {
	float2 uv_MainTex;
	float2 uv_AlphaTex;	
	float2 uv_SpecTex;
	float2 uv_BumpMap;
	float2 uv_Illum;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 tex_a = tex2D(_AlphaTex, IN.uv_AlphaTex);
	fixed4 tex_s = tex2D(_SpecTex, IN.uv_SpecTex);
	
	o.Albedo = tex.rgb * _Color.rgb;

	o.Emission = _Emission.rgb * UNITY_SAMPLE_1CHANNEL(_Illum, IN.uv_Illum);
	
	o.Gloss = tex_s.rgb;
	
	o.Alpha = tex.a * tex_a.rgb * _Color.a;
	
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	
	o.Specular = _Shininess;
}
ENDCG  
}

FallBack "Diffuse"
}
