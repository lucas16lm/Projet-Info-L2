Shader "Custom/PropsTransparent"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _AlphaCutoff("Alpha Cutoff", Range(0,1)) = 0.3
        _TransparencyAmount("Transparency Amount", Range(0,1)) = 1
        _UseTransparency("Use Transparency", Float) = 1
        _Metallic("Metallic", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="TransparentCutout" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float _AlphaCutoff;
            float _TransparencyAmount;
            float _UseTransparency;
            float _Metallic;
            float _Smoothness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // 🧼 Découpe nette : on discard les pixels trop transparents
                clip(texColor.a - _AlphaCutoff);

                // ✨ Alpha blend progressif si activé
                float alpha = (_UseTransparency > 0.5) ? _TransparencyAmount : 1.0;
                texColor.a *= alpha;

                float3 normalWS = normalize(IN.normalWS);
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float NdotL = max(dot(normalWS, lightDir), 0.0);

                float3 diffuse = texColor.rgb * mainLight.color * NdotL;

                float3 halfwayDir = normalize(lightDir + viewDir);
                float spec = pow(max(dot(normalWS, halfwayDir), 0.0), _Smoothness * 128.0);
                float3 specular = spec * mainLight.color * _Metallic;

                return float4(diffuse + specular, texColor.a);
            }

            ENDHLSL
        }
    }

    FallBack "Universal Forward"
}
