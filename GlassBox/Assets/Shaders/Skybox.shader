// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/TestSky"
{
     Properties
    {
        _SkyColor("Sky Color Day", Color) = (1,1,1,1)
        _SkyColorNight("Sky Color Night", Color) = (1,1,1,1)
        _HorizonColor("Horizon Color Day", Color) = (1,1,1,1)
        _HorizonColorNight("Horizon Color Night", Color) = (1,1,1,1)
        _DayLength("Day length", Range(1,4)) = 2
        _MoonSize("Moon Size", Float) = 0.1
        _MoonMaskSize("Moon Mask Size", Float) = 0.02
        _CellSize("Cell Size", Range(0,2)) = 2
        _StarDensity("Star Density", Range(50,550)) = 121
        _MoonMaskOffset("Sun Mask Offset", Vector) = (0,0,0,0)
        _SunColor("Sun Color", Color) = (1,1,1,1)
        _StarBrightness("Star Brightness", Range(0,1)) = 0
        _SunSize("Sun Size", Float) = 0.1
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "RandomFunc.cginc"
            #include "Voronoi.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal:NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal :NORMAL;
                float3 direction : TEXCOORD0;
                float2 uv : TEXCOORD1;

            };

            float4 _SkyColor;
            float4 _SkyColorNight;
            float4 _HorizonColor;
            float4 _HorizonColorNight;
            float4 _SunColor;
            float _DayLength;
            uniform float3 _SunDirection;
            float _MoonSize;
            float _CellSize;
            float _StarDensity;
            float _MoonMaskSize;
            float3 _MoonMaskOffset;
            float _StarBrightness;
            float _SunSize;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = (UnityObjectToClipPos(v.vertex));
                o.uv = v.uv;
                //get view direction of camera
                o.direction = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
                return o;
            }

            float4 rgbTohsv(float4 In){
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                float D = Q.x - min(Q.w, Q.y);
                float  E = 1e-10;
                return float4(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x,1);
            }
            
             
            #define PI 3.141592653589793
            fixed4 frag (v2f i) : SV_Target
            {

                //map UV's to skybox
                float3 pos = normalize(i.direction);
                float2 newUV;
                newUV.x = 0.5 + atan2(pos.z,pos.x)/(PI*2);
                newUV.y = 0.5 - asin(pos.y)/PI;

                //add the stars    
                float val = voronoiNoise1d(newUV/_CellSize);
                float stars = saturate(val);
                stars = 1-stars;
                
                

                //blend the sky color
                fixed4 currentSkyColor = lerp(_SkyColor,_SkyColorNight,saturate(sin((_Time.y)+1)/_DayLength));
                fixed4 currentHorizonColor = lerp(_HorizonColor,_HorizonColorNight,saturate(((_Time.y-_DayLength))/_DayLength));
                fixed4 col = _SkyColor;

                //add sun   
                float sunDot = dot(normalize(-_SunDirection), normalize(i.direction));
                float sunAngle = acos(sunDot);
                float sunSize = pow(_SunSize,2);
                float sun = 1-(step(sunSize,sunAngle));


                //add Moon
                float moonDot = dot(normalize(_SunDirection), normalize(i.direction));
                float moonAngle = acos(moonDot);
                float moonSize = pow(_MoonSize, 2);
                float moon = 1-(step(moonSize,moonAngle));
                
               
                float moonMaskDot = dot(normalize(_SunDirection + _MoonMaskOffset), normalize(i.direction));
                float moonMaskAngle = acos(moonMaskDot);
                float moonMaskSize = pow(_MoonMaskSize,2);
                float moonMask = 1-(step(moonMaskSize,moonMaskAngle));
               

                float finalMoon = saturate(moon-moonMask);

             
                return col + + sun + (finalMoon) + pow(stars,_StarDensity)*_StarBrightness;
            }
            ENDCG
        }
    }
}
