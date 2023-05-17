using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FlexibleUIBase : MonoBehaviour {

    public FlexibleUIData skinData;

    protected virtual void OnSkinUI()
    {

    }

    public virtual void Awake()
    {
        OnSkinUI();
    }


    public virtual void Update()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            OnSkinUI();
        }
    }

}