using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour 
{
    [SerializeField]
    Renderer[] trapRenders;

    [SerializeField]
    Material allowed;

    [SerializeField]
    Material notAllowed;

    [SerializeField]
    public Vector2 size = Vector2.one;

    bool curStatus = true;

    public void setStatus(bool status)
    {

        if (curStatus == status)
        {
            return;
        }

        foreach (Renderer trapRender in this.trapRenders)
        {
            trapRender.material = (status) ? this.allowed : notAllowed;
        }

        curStatus = status;
    }
}
