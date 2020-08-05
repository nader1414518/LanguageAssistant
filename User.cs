using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string email;
    public string username;
    public string userId;
    public string userLanguage;
    public string userColorMode;
    public string lessonNotificationStatus;
    public List<Card> cards;

    public User()
    {
        email = "";
        username = "";
        userId = "";
        userLanguage = "EN";
        userColorMode = "Normal";
        lessonNotificationStatus = "";
        cards = new List<Card>();
    }
}
