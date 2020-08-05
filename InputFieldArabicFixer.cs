using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldArabicFixer : MonoBehaviour
{
    public void OnEndEditHandler()
    {
        if (HelperMan.FindLang(this.GetComponent<InputField>().text) == "Arabic")
        {
            this.GetComponent<InputField>().text = ArabicSupport.ArabicFixer.Fix(this.GetComponent<InputField>().text);
        }
        //else
        //{
        //    this.GetComponent<InputField>().text = this.GetComponent<InputField>().text;
        //}
    }
}
