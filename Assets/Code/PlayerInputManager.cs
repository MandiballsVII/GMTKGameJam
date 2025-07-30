using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInputManager : MonoBehaviour
{
    protected bool jumpInput;
    protected bool glideInput;
    protected bool jumpOutput;
    protected bool glideOutput;
    protected bool paintSquareInput;
    protected bool pause;
    protected bool controlsEnable;
    // Start is called before the first frame update
    void Start()
    {
        glideInput = Input.GetButton("Fire3");
        jumpOutput = Input.GetButtonUp("Jump");
        glideOutput = Input.GetButtonUp("Fire3");
        paintSquareInput = Input.GetButtonDown("Fire1");
        pause = Input.GetButtonDown("pause");
}

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void enableControls()
    {
        controlsEnable = true;
    }

    protected void disableControls()
    {
        controlsEnable = false;
    }

}
