using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Sun : MonoBehaviour
{

    public int dayLength = 5;
    float counter;
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalVector("_SunDirection", -transform.forward);
        counter = dayLength;
    }

    // Update is called once per frame
    void Update()
    {
        // counter -= Time.deltaTime;
        // transform.RotateAroundLocal(-Vector3.right,Time.deltaTime/dayLength);
        //   Shader.SetGlobalVector("_SunDirection", -transform.forward);
        // if(counter <= 0 )counter = dayLength;
    }
}
