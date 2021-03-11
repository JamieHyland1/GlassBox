using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform sun;
    public Transform moon;

    public Light moonLight;
    public float cycleInMinutes = 1;
    public AnimationCurve sunBrightness = new AnimationCurve(
        new Keyframe(0 ,0.01f),
        new Keyframe(0.15f,0.01f),
        new Keyframe(0.35f,1),
        new Keyframe(0.65f,1),
        new Keyframe(0.85f,0.01f),
        new Keyframe(1 ,0.01f)
        );

        public AnimationCurve starBrightness = new AnimationCurve(
       new Keyframe(0 ,0.01f),
        new Keyframe(0.15f,0.01f),
        new Keyframe(0.35f,1),
        new Keyframe(0.65f,1),
        new Keyframe(0.85f,0.01f),
        new Keyframe(1 ,0.01f)
        );
    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private Material skybox;

    public Gradient skyColorDay;
    public Gradient skyColorNight;

    public Gradient sunColor = new Gradient(){
        colorKeys = new GradientColorKey[2]{
            new GradientColorKey(new Color(1, 0.75f, 0.3f), 0.45f),
            new GradientColorKey(new Color(0.95f, 0.95f, 1), 0.75f),
            },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };


    public float sunAngle = 0;
    void Start()
    {
        
        sun.rotation = Quaternion.identity;
        Shader.SetGlobalVector("_SunDirection", -sun.forward);
     
    }

    // Update is called once per frame
    void Update()
    {
        updateSunAngle();
        rotateSun();
        SetSunBrightness();
        setSunColor();
        setSkyColor();
        setStars();
    }

    public void rotateSun(){
        
        sun.Rotate((Vector3.right*Time.deltaTime*6)/cycleInMinutes);
        moon.Rotate((Vector3.left*Time.deltaTime*6)/cycleInMinutes);

        Shader.SetGlobalVector("_SunDirection", sun.forward);
    }
     void SetSunBrightness(){
      
        sunLight.intensity = sunBrightness.Evaluate(sunAngle);
    }

    void updateSunAngle(){
        sunAngle = Vector3.SignedAngle(Vector3.down,sun.forward,sun.right);
        sunAngle = sunAngle/360+0.5f;
    }

    void setSunColor(){
        sunLight.color = sunColor.Evaluate(sunAngle);
    }

    void setSkyColor(){
    if(sunAngle >= 0.25f && sunAngle < 0.75f){
        skybox.SetColor("_SkyColor",skyColorDay.Evaluate(sunAngle*2-0.5f));
    }
    else if(sunAngle > 0.75f){
        skybox.SetColor("_SkyColor",skyColorNight.Evaluate(sunAngle*2f-1.5f));
    }else{
        skybox.SetColor("_SkyColor",skyColorNight.Evaluate(sunAngle*2f+0.5f));
    }
    }

    void setStars(){
       skybox.SetFloat("_StarBrightness", starBrightness.Evaluate(sunAngle));

    }

  
}
