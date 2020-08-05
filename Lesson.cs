using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson : MonoBehaviour
{
    public string title;
    public string description;
    public List<Card> cards;

    public Lesson()
    {
        title = "";
        description = "";
        cards = new List<Card>();
    }
}
