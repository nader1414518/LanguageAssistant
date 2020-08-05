using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{
    public string originalWord;
    public string translatedWord;
    public string id;
    public Date creationDate;

    public Card()
    {
        originalWord = "";
        translatedWord = "";
        id = "";
        creationDate = new Date();
        creationDate.minute = DateTime.Now.Minute.ToString();
        creationDate.hour = DateTime.Now.Hour.ToString();
        creationDate.day = DateTime.Now.Day.ToString();
        creationDate.month = DateTime.Now.Month.ToString();
        creationDate.year = DateTime.Now.Year.ToString();
    }

    public Card(string originalWord, string translatedWord)
    {
        this.originalWord = originalWord;
        this.translatedWord = translatedWord;
        this.id = this.originalWord + this.translatedWord;
        creationDate = new Date();
        creationDate.minute = DateTime.Now.Minute.ToString();
        creationDate.hour = DateTime.Now.Hour.ToString();
        creationDate.day = DateTime.Now.Day.ToString();
        creationDate.month = DateTime.Now.Month.ToString();
        creationDate.year = DateTime.Now.Year.ToString();
    }
}
