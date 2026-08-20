Shader "UI/CircleReveal"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (0,0,0,1)

        _Radius ("Radius", Range(0, 1.5)) = 0
        _Softness ("Edge Softness", Range(0, 0.1)) = 0.02
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)

        _Aspect ("Aspect Ratio", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Pass
        {
            Name "CircleReveal"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

                float4 _Color;

                float _Radius;
                float _Softness;

                float4 _Center;

                float _Aspect;

            CBUFFER_END


            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionCS =
                    TransformObjectToHClip(
                        input.positionOS.xyz
                    );

                output.uv = input.uv;
                output.color = input.color;

                return output;
            }


            half4 frag(Varyings input) : SV_Target
            {
                // ============================================
                // Текстура Image
                // ============================================

                half4 color =
                    SAMPLE_TEXTURE2D(
                        _MainTex,
                        sampler_MainTex,
                        input.uv
                    );

                color *= _Color;
                color *= input.color;


                // ============================================
                // UV относительно центра
                // ============================================

                float2 uv =
                    input.uv - _Center.xy;


                // ============================================
                // Aspect Ratio
                // ============================================

                uv.x *= _Aspect;


                // ============================================
                // Расстояние от центра
                // ============================================

                float distanceFromCenter =
                    length(uv);


                // ============================================
                // Circular Reveal
                // ============================================

                float mask =
                    1.0 -
                    smoothstep(
                        _Radius - _Softness,
                        _Radius,
                        distanceFromCenter
                    );


                // ============================================
                // Alpha
                // ============================================

                color.a *= mask;


                return color;
            }

            ENDHLSL
        }
    }
}