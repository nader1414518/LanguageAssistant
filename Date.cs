using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Date
{
    public string minute;
    public string hour;
    public string day;
    public string month;
    public string year;

    public Date()
    {
        minute = "";
        hour = "";
        day = "";
        month = "";
        year = "";
    }
}
