using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanelController : MonoBehaviour
{
    RectTransform iconTranform;

    public float rotationSpeed;

    void OnEnable()
    {
        iconTranform = this.GetComponentsInChildren<RectTransform>()[this.GetComponentsInChildren<RectTransform>().Length - 1];
    }

    void FixedUpdate()
    {
        if (AppMan.showLoadingPanel)
        {
            iconTranform.eulerAngles = new Vector3(iconTranform.eulerAngles.x, iconTranform.eulerAngles.y, iconTranform.eulerAngles.z - rotationSpeed);
        }
    }
}
