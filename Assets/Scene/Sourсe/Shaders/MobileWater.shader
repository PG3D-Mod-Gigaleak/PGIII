Shader "MobileWater/MobileWater" {
Properties {
 _Color ("Water Colour", Color) = (1,1,1,1)
 _MainTex ("Water Texture", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM

  #pragma vertex vert
  #pragma fragment frag

  sampler2D _MainTex;
  float4 _MainTex_ST, _Color;

  struct appdata_t
  {
	float4 vertex : POSITION;
	float4 texcoord0 : TEXCOORD0;
  };

  struct v2f
  {
	float4 vertex : POSITION;
	float2 texcoord0 : TEXCOORD0;
  };

  v2f vert(appdata_t v)
  {
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.texcoord0 = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	return o;
  }

  half4 frag(v2f i) : SV_TARGET
  {
	return tex2D(_MainTex, i.texcoord0) * _Color;
  }
  ENDCG
 }
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
 }
}
}