using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected GameFace face;

    public BaseManager(GameFace gameFace)
    {
        this.face = gameFace;
    }
    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
