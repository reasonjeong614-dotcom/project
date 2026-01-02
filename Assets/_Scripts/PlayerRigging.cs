using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerRigging : MonoBehaviour
{
    Rig rig;
    float targetWeight = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponentInChildren<Rig>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);
        //if(Input.GetKeyDown(KeyCode.T))
        //Keyboard.current.tKey.isPressed   => KeyHold
        //Keyboard.current.tKey.wasPressedThisFrame => KeyDown 
        //Keyboard.current.tKey.wasReleasedThisFrame => KeyUp
        if (Keyboard.current.tKey.isPressed)
        {
            targetWeight = 1f;
        }
        if (Keyboard.current.yKey.isPressed)
        {
            targetWeight = 0f;
        }

    }
}
