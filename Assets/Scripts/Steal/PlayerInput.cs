using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action KeyAction = null;
    void Update()
    {
        if(Input.anyKey) KeyAction?.Invoke();
    }
}
