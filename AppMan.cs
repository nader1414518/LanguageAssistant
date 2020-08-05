using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using UnityEngine.SceneManagement;
using ArabicSupport;
using Unity.Notifications.Android;

public class Globals : MonoBehaviour
{
    public static string username;
    public static string email;
    public static string userId;

    public static bool isLoggedIn = false;
    public static bool loggedInWithGoogle = false;
    public static bool loggedInWithEmail = false;

    public static User currentUser = new User();

    public static string lessonChannelId = "12juneu3dduwh984ncuhfreouhfncufhfenrf9rehfnuwiernfvd";
}

public class HelperMan : MonoBehaviour
{
    public static bool reservationSent = false;

    public static void ShowPanel(GameObject panel)
    {
        if (panel)
        {
            panel.SetActive(true);
        }
    }
    public static void HidePanel(GameObject panel)
    {
        if (panel)
        {
            panel.SetActive(false);
        }
    }
    public static void HighlightPanel(GameObject panel)
    {
        if (panel)
        {
            Color color = panel.GetComponent<Image>().color;
            color.a = 1.0f;
            panel.GetComponent<Image>().color = color;
        }
    }
    public static void DehighlightPanel(GameObject panel)
    {
        if (panel)
        {
            Color color = panel.GetComponent<Image>().color;
            color.a = 0.0f;
            panel.GetComponent<Image>().color = color;
        }
    }
    public static void DestroyGameObjectList(List<GameObject> list)
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i])
                {
                    Destroy(list[i]);
                }
            }
            list.Clear();
        }
    }
    public static bool CheckEmpty(InputField inputField)
    {
        if (inputField)
        {
            if (inputField.text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool CheckNumeric(string str)
    {
        try
        {
            double.Parse(str);
            return true;
        }
        catch
        {
            Debug.Log("Enter a number please ... ");
            return false;
        }
    }
    public static void SendEmail(string msg, string topic, string destinationMail)
    {
        reservationSent = false;
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress("amicabot413@gmail.com");
        mail.To.Add(new MailAddress(destinationMail));

        mail.Subject = topic;
        mail.Body = msg;


        SmtpServer.Credentials = new System.Net.NetworkCredential("amicabot413@gmail.com", "Amicabot314") as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        SmtpServer.Send(mail);
        Debug.Log("Mail Sent ... ");
        reservationSent = true;
    }
    public static void CheckInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MessageBox.Open("Check Internet Connection ... ");
        }
    }
    public static string GetBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
    public static string GetUnicode(string sourceString)
    {
        Encoding ascii = Encoding.ASCII;
        Encoding unicode = Encoding.Unicode;

        // get the ascii bytes array 
        byte[] asciiBytes = ascii.GetBytes(sourceString);

        // Perform the conversion from ascii to unicode
        byte[] unicodeBytes = Encoding.Convert(ascii, unicode, asciiBytes);

        // Convert the new byte[] into a char[] and then into a string
        char[] unicodeChars = new char[unicode.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
        unicode.GetChars(unicodeBytes, 0, unicodeBytes.Length, unicodeChars, 0);
        string unicodeString = new string(unicodeChars);

        return unicodeString;
    }
    public static string GetUnicodeCharArray(string data)
    {
        string res = string.Empty;
        for (int i = 0; i < data.Length; i++)
        {
            if (i == 0)
            {
                res += char.ConvertToUtf32(data, i).ToString("X");
            }
            else
            {
                res += "," + char.ConvertToUtf32(data, i).ToString("X");
            }
        }
        return res;
    }
    public static string FindLang(string text)
    {
        string result = "";
        if (text.Any(c => c >= 0xFB50 && c <= 0xFEFC) || text.Any(c => c >= 0x0600 && c <= 0x06FF))
        {
            result = "Arabic";
        }
        //if (text.Any(c => c >= 0x0600 && c <= 0x06FF))
        //{
        //    result += "Persian";
        //}
        else if (text.Any(c => c >= 0x20 && c <= 0x7E))
        {
            result = "English";
        }
        //if (text.Any(c => c >= 0x0530 && c <= 0x058F))
        //{
        //    result += "Armenian";
        //}
        //if (text.Any(c => c >= 0x2000 && c <= 0xFA2D))
        //{
        //    result += "Chinese";
        //}
        return result;
    }
    public static string FindScalars(string response)
    {
        string res = string.Empty;
        string temp = response;
        List<int> locs = new List<int>();
        List<string> syll = new List<string>();

        // Determine the locations of commas in the string 
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == ',')
            {
                locs.Add(i);
            }
        }

        // Cut Every Syllabus based on the locations of commas 
        for (int j = 0; j < locs.Count; j++)
        {
            if (j == 0)
            {
                syll.Add(temp.Substring(0, temp.IndexOf(",")).Replace(",", ""));
            }
            else
            {
                syll.Add(temp.Substring(locs[j - 1] + 1, temp.IndexOf(",")).Replace(",", ""));
            }
        }

        foreach (string snap in syll)
        {
            res += char.ConvertFromUtf32(int.Parse(snap, NumberStyles.HexNumber));
        }

        return res;
    }
}

public class AppMan : MonoBehaviour
{
    #region AuthenticationArea
    #region Variables
    // This the client id you get from the json file you downloaded to the assets folder (type: 3)
    public static string webClientId = "915147918749-e9lgij0bmdi1at773s9h15f2bg7hh4hg.apps.googleusercontent.com";
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public static bool loadDashboard = false;
    public Button forgotPasswordBtn;
    #endregion
    #region Functions
    // Facebook And Google SignIn Functionality
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                }
                else
                {
                    //AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                    MessageBox.Open("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                }
            }
        });
    }
    
    private void SignInWithGoogle() { OnSignIn(); }
    private void SignOutFromGoogle() { OnSignOut(); }
    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    private void OnSignOut()
    {
        //AddToInformation("Calling SignOut ... ");
        GoogleSignIn.DefaultInstance.SignOut();
        FirebaseAuth.DefaultInstance.SignOut();
    }
    private void OnDisconnect()
    {
        //AddToInformation("Calling Disconnect ... ");
        GoogleSignIn.DefaultInstance.Disconnect();
    }
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        //loadDashboard = false;
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    //AddToInformation("Got Error: " + error.Status + " " + error.Message);
                    MessageBox.Open("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    //AddToInformation("Got Unexpected Exception " + task.Exception);
                    MessageBox.Open("Got Unexpected Exception " + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            //AddToInformation("Canceled ... ");
            MessageBox.Open("Canceled ... ");
        }
        else
        {
            //AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            //AddToInformation("Email: " + task.Result.Email);
            loadDashboard = true;
            Globals.username = task.Result.DisplayName;
            Globals.email = task.Result.Email;
            Globals.userId = task.Result.UserId;
            Globals.isLoggedIn = true;
            Globals.loggedInWithGoogle = true;
            //AddToInformation("Google Id Token: " + task.Result.IdToken);
            //AddToInformation("Google Id Token: " + task.Result.IdToken);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {
                    //AddToInformation("\nError code: " + inner.ErrorCode + " Message: " + inner.Message);
                    MessageBox.Open("\nError code: " + inner.ErrorCode + " Message: " + inner.Message);
                }
                else
                {
                    //loadDashboard = true;
                    Globals.username = task.Result.DisplayName;
                    Globals.email = task.Result.Email;
                    Globals.userId = task.Result.UserId;
                    Globals.isLoggedIn = true;
                    Globals.loggedInWithGoogle = true;
                    //loadDashboard = true;
                    Debug.Log("Google Username: " + Globals.username);
                    MessageBox.Open("Google Username: " + Globals.username);
                    //SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
                    //AddToInformation("SignIn Successful ... ");
                }
            }
        });
    }
    private void SignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        //AddToInformation("Calling Sign In Silently ... ");
        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }
    // Email SignIn Functionality
    private static bool EmailLogin(string username, string password)
    {
        bool res = false;
        if (username == "" || password == "")
        {
            // Empty fields 
            Debug.Log("Empty Field");
            //LogScreenMessages("Empty Field");
            MessageBox.Open("Empty Field");
            res = false;
        }
        else
        {
            FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(username, password).ContinueWith((task =>
            {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    //GetErrorMessage((AuthError)e.ErrorCode);
                    MessageBox.Open(((AuthError)e.ErrorCode).ToString());
                    res = false;
                    return;
                }
                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    //GetErrorMessage((AuthError)e.ErrorCode);
                    MessageBox.Open(((AuthError)e.ErrorCode).ToString());
                    res = false;
                    return;
                }
                if (task.IsCompleted)
                {
                    Globals.username = username;
                    Globals.email = username;
                    Globals.userId = task.Result.UserId;
                    Globals.isLoggedIn = true;
                    Globals.loggedInWithEmail = true;
                    Debug.Log("Username: " + Globals.username);
                    loadDashboard = true;
                    //SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
                    //SceneManager.LoadSceneAsync("Dashboard", LoadSceneMode.Single);
                    Debug.Log("Welcome back " + Globals.email);
                    //LogScreenMessages("Welcome back " + Globals.email);
                    res = true;
                }
            }));
        }
        return res;
    }
    private bool RegisterUser(string email, string password)
    {
        bool res = false;
        if (password == "" || email == "")
        {
            // Empty field 
            res = false;
            Debug.Log("Some fields are empty ... ");
            MessageBox.Open("Some fields are empty ... ");
        }
        else
        {
            FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith((task =>
            {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    //GetErrorMessage((AuthError)e.ErrorCode);
                    MessageBox.Open(((AuthError)e.ErrorCode).ToString());
                    res = false;
                    return;
                }
                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    //GetErrorMessage((AuthError)e.ErrorCode);
                    MessageBox.Open(((AuthError)e.ErrorCode).ToString());
                    res = false;
                    return;
                }
                if (task.IsCompleted)
                {
                    Debug.Log("Signed Up Successfully ... ");
                    //signedUpSuccessfully = true;
                    //LogScreenMessages("Signed Up Successfully ... ");
                    MessageBox.Open("Signed Up Successfully ... ");
                    HideAllPanels();
                    HelperMan.ShowPanel(loginPanel);
                    res = true;
                }
            }));
        }
        return res;
    }
    public static void LogOut()
    {
        if (Globals.loggedInWithEmail)
        {
            Globals.isLoggedIn = false;
            Globals.loggedInWithEmail = false;
            Globals.username = "";
            Globals.email = "";
            Globals.userId = "";
            //isDashboardLoaded = false;
            //DBMan.isItemsRetrieved = false;
            //loadDashboard = false;
            //LogScreenMessages("Logged Out ... ");
            FirebaseAuth.DefaultInstance.SignOut();
            //SceneManager.LoadScene("LoginMenu", LoadSceneMode.Single);
            loadDashboard = false;
        }
        else if (Globals.loggedInWithGoogle)
        {
            Globals.isLoggedIn = false;
            Globals.loggedInWithGoogle = false;
            Globals.username = "";
            Globals.email = "";
            Globals.userId = "";
            //DBMan.isItemsRetrieved = false;
            //isDashboardLoaded = false;
            loadDashboard = false;
            GoogleSignIn.DefaultInstance.SignOut();
            FirebaseAuth.DefaultInstance.SignOut();
            Debug.Log("Signed out from google ... ");
            //SceneManager.LoadScene("LoginMenu", LoadSceneMode.Single);
        }
    }
    #endregion
    #endregion

    #region UIArea
    #region Variables
    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject dashboardPanel;

    public GameObject startLessonBtnContainer;
    public GameObject revisionBtnContainer;
    public GameObject addCardsBtnContainer;
    public GameObject profileBtnContainer;

    public GameObject startLessonPanel;
    public GameObject revisionPanel;
    public GameObject addCardsPanel;
    public GameObject profilePanel;

    public InputField loginEmailIF;
    public InputField loginPasswordIF;
    public InputField signupEmailIF;
    public InputField signupPasswordIF;
    public InputField signupPasswordConfirmIF;

    public GameObject loadingPanel;

    // Save Cards
    public InputField originalWordIF;
    public InputField translatedWordIF;

    public static bool showLoadingPanel;
    public bool addedACard = false;
    public bool loadRevisionCards = false;
    public bool userInfoLoaded = false;

    GameObject cardSlotPrefab;
    GameObject cardDetailsPanelPrefab;
    GameObject cardLabelPrefab;
    public List<GameObject> revisionCards = new List<GameObject>();

    GameObject profileLabelPrefab;
    GameObject profileTogglePrefab;
    GameObject languageDropdownPrefab;
    GameObject saveSettingsBtnPrefab;
    public List<GameObject> profileElements = new List<GameObject>();
    public bool loadProfile = false;

    public GameObject logOutBtn;
    #endregion
    #region Auxilaries
    void HideAllPanels()
    {
        HelperMan.HidePanel(loginPanel);
        HelperMan.HidePanel(signupPanel);
        HelperMan.HidePanel(dashboardPanel);
        HelperMan.HidePanel(startLessonPanel);
        HelperMan.HidePanel(revisionPanel);
        HelperMan.HidePanel(addCardsPanel);
        HelperMan.HidePanel(profilePanel);
    }
    void DehighlightAllPanels()
    {
        HelperMan.DehighlightPanel(startLessonBtnContainer);
        HelperMan.DehighlightPanel(revisionBtnContainer);
        HelperMan.DehighlightPanel(addCardsBtnContainer);
        HelperMan.DehighlightPanel(profileBtnContainer);
    }
    void LoadRevisionCardsIntoUI()
    {
        if (revisionPanel && cardSlotPrefab)
        {
            for (int i = 0; i < revisionCards.Count; i++)
            {
                Destroy(revisionCards[i].gameObject);
            }
            revisionCards.Clear();

            for (int j = 0; j < Globals.currentUser.cards.Count; j++)
            {
                GameObject cardSlotObj = Instantiate(cardSlotPrefab, revisionPanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
                cardSlotObj.GetComponentInChildren<Text>().text = (j + 1).ToString();
                cardSlotObj.GetComponent<CardSlot>().currentCard = Globals.currentUser.cards[j];
                //cardSlotObj.GetComponent<CardSlot>().currentCard.originalWord = Globals.currentUser.cards[j].originalWord;
                //cardSlotObj.GetComponent<CardSlot>().currentCard.translatedWord = Globals.currentUser.cards[j].translatedWord;
                //cardSlotObj.GetComponent<CardSlot>().currentCard.creationDate = Globals.currentUser.cards[j].creationDate;
                cardSlotObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    // Open Card 
                    Debug.Log("Card ID: " + cardSlotObj.GetComponent<CardSlot>().currentCard.id);
                    if (cardDetailsPanelPrefab && cardLabelPrefab)
                    {
                        // Add Card Details Panel
                        GameObject cardDetailsPanelObj = Instantiate(cardDetailsPanelPrefab, dashboardPanel.transform);
                        cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard = cardSlotObj.GetComponent<CardSlot>().currentCard;
                        // Assign its back btn callback
                        cardDetailsPanelObj.GetComponentsInChildren<Button>()[cardDetailsPanelObj.GetComponentsInChildren<Button>().Length - 1].onClick.AddListener(delegate
                        {
                            // Close this panel
                            Destroy(cardDetailsPanelObj.gameObject);
                        });
                        // Add Label Object to the panel
                        GameObject cardLabelObj = Instantiate(cardLabelPrefab, cardDetailsPanelObj.GetComponentsInChildren<Image>()[1].gameObject.transform);
                        // Assign its text 
                        cardLabelObj.GetComponent<Text>().text = cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard.originalWord;
                        // Assign its on click event 
                        cardLabelObj.GetComponent<Button>().onClick.AddListener(delegate
                        {
                            if (cardLabelObj.GetComponent<Text>().text == cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard.originalWord)
                            {
                                cardLabelObj.GetComponent<Text>().text = cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard.translatedWord;
                            }
                            else if (cardLabelObj.GetComponent<Text>().text == cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard.translatedWord)
                            {
                                cardLabelObj.GetComponent<Text>().text = cardDetailsPanelObj.GetComponent<CardDetailsPanel>().currentCard.originalWord;
                            }
                        });
                    }
                });
                // Keep track of added slots 
                revisionCards.Add(cardSlotObj);
            }
        } 
    }
    void LoadProfileElementsIntoUI()
    {
        // Clear Profile Elements
        for (int i = 0; i < profileElements.Count; i++)
        {
            Destroy(profileElements[i].gameObject);
        }
        profileElements.Clear();

        if (profilePanel && profileLabelPrefab)
        {
            // Add Email Label
            GameObject emailLabelObj = Instantiate(profileLabelPrefab, profilePanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
            if (Globals.currentUser.userLanguage == "AR")
            {
                emailLabelObj.GetComponent<Text>().text = ArabicFixer.Fix("البريد الالكتروني: " + Globals.currentUser.email);
            }
            else
            {
                emailLabelObj.GetComponent<Text>().text = "Email: " + Globals.currentUser.email;
            }
            // Add Username Label 
            GameObject usernameLabelObj = Instantiate(profileLabelPrefab, profilePanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
            if (Globals.currentUser.userLanguage == "AR")
            {
                usernameLabelObj.GetComponent<Text>().text = ArabicFixer.Fix("اسم المستخدم: " + Globals.currentUser.username);
            }
            else
            {
                usernameLabelObj.GetComponent<Text>().text = "Username: " + Globals.currentUser.username;
            }
            // Add Toggle for color mode
            GameObject colorModeToggleObj = Instantiate(profileTogglePrefab, profilePanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
            if (Globals.currentUser.userLanguage == "AR")
            {
                colorModeToggleObj.GetComponentInChildren<Text>().text = ArabicFixer.Fix("الوضع الليلي");
            }
            else
            {
                colorModeToggleObj.GetComponentInChildren<Text>().text = "Dark Mode";
            }
            if (Globals.currentUser.userColorMode == "Dark")
            {
                colorModeToggleObj.GetComponent<Toggle>().isOn = true;
                dashboardPanel.GetComponent<Image>().color = Color.black;
            }
            else
            {
                colorModeToggleObj.GetComponent<Toggle>().isOn = false;
                dashboardPanel.GetComponent<Image>().color = Color.white;
            }
            // Assign its on Value Changed Event 
            colorModeToggleObj.GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                if (colorModeToggleObj.GetComponent<Toggle>().isOn)
                {
                    Globals.currentUser.userColorMode = "Dark";
                    dashboardPanel.GetComponent<Image>().color = Color.black;
                }
                else
                {
                    Globals.currentUser.userColorMode = "Normal";
                    dashboardPanel.GetComponent<Image>().color = Color.white;
                }
                // Save current profile settings 
                showLoadingPanel = true;
                string json = JsonUtility.ToJson(Globals.currentUser);
                FirebaseDatabase.DefaultInstance.RootReference
                .Child("users")
                .Child(Globals.currentUser.userId)
                .Child("profile")
                .SetRawJsonValueAsync(json)
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (Globals.currentUser.userLanguage == "AR")
                        {
                            MessageBox.Open(ArabicFixer.Fix("تم حفظ التغييرات ..."));
                        }
                        else
                        {
                            MessageBox.Open("Saved Settings Successfully ... ");
                        }
                    }
                    showLoadingPanel = false;
                });
            });
            // Add a dropdown for userLanguage
            if (languageDropdownPrefab)
            {
                GameObject langDDObj = Instantiate(languageDropdownPrefab, profilePanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
                // Assign its on value changed event 
                langDDObj.GetComponent<Dropdown>().onValueChanged.AddListener(delegate
                {
                    if (langDDObj.GetComponent<Dropdown>().value == 1)
                    {
                        Globals.currentUser.userLanguage = "AR";
                        if (logOutBtn)
                        {
                            logOutBtn.GetComponentInChildren<Text>().text = ArabicFixer.Fix("خروج");
                        }
                    }
                    else
                    {
                        Globals.currentUser.userLanguage = "EN";
                        if (logOutBtn)
                        {
                            logOutBtn.GetComponentInChildren<Text>().text = "Log Out";
                        }
                    }
                    //// Reload current panel with new language
                    //OpenProfilePanelBtnCallback();
                });
                // Assign its value
                if (Globals.currentUser.userLanguage == "AR")
                {
                    langDDObj.GetComponent<Dropdown>().value = 1;
                }
                else
                {
                    langDDObj.GetComponent<Dropdown>().value = 0;
                }

                // Keep track of added elements 
                profileElements.Add(langDDObj);
            }

            // Add Save Settings Button 
            if (saveSettingsBtnPrefab)
            {
                GameObject saveSettingsBtnObj = Instantiate(saveSettingsBtnPrefab, profilePanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
                if (Globals.currentUser.userLanguage == "AR")
                {
                    saveSettingsBtnObj.GetComponentInChildren<Text>().text = ArabicFixer.Fix("حفظ");
                }
                else
                {
                    saveSettingsBtnObj.GetComponentInChildren<Text>().text = "Save";
                }
                // Assign its on click event 
                saveSettingsBtnObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    // Save current profile settings 
                    showLoadingPanel = true;
                    string json = JsonUtility.ToJson(Globals.currentUser);
                    FirebaseDatabase.DefaultInstance.RootReference
                    .Child("users")
                    .Child(Globals.currentUser.userId)
                    .Child("profile")
                    .SetRawJsonValueAsync(json)
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            if (Globals.currentUser.userLanguage == "AR")
                            {
                                MessageBox.Open(ArabicFixer.Fix("تم حفظ التغييرات ..."));
                            }
                            else
                            {
                                MessageBox.Open("Saved Settings Successfully ... ");
                            }
                        }
                        showLoadingPanel = false;
                    });
                });
                // Keep track of added profile elements 
                profileElements.Add(saveSettingsBtnObj);
            }

            // Keep tracks of the profile elements
            profileElements.Add(emailLabelObj);
            profileElements.Add(usernameLabelObj);
            profileElements.Add(colorModeToggleObj);
        }
    }
    #endregion
    #region Callbacks
    public void LoginBtnCallback()
    {
        if (loginEmailIF && loginPasswordIF)
        {
            loginPasswordIF.contentType = InputField.ContentType.Standard;
            string pass = loginPasswordIF.text;
            loginPasswordIF.contentType = InputField.ContentType.Password;
            string email = loginEmailIF.text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Open("Some Fields are empty ... ");
            }
            else
            {
                EmailLogin(email, pass);
            }
        }
    }
    public void LoginWithGoogleBtnCallback()
    {
        SignInWithGoogle();
    }
    public void SignUpBtnCallback()
    {
        if (signupEmailIF && signupPasswordIF && signupPasswordConfirmIF)
        {
            signupPasswordIF.contentType = InputField.ContentType.Standard;
            string pass = signupPasswordIF.text;
            signupPasswordIF.contentType = InputField.ContentType.Password;
            signupPasswordConfirmIF.contentType = InputField.ContentType.Standard;
            string passConfirm = signupPasswordConfirmIF.text;
            signupPasswordConfirmIF.contentType = InputField.ContentType.Password;
            string email = signupEmailIF.text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(passConfirm))
            {
                MessageBox.Open("Some fields are empty ... ");
            }
            else if (pass != passConfirm)
            {
                MessageBox.Open("Passwords don't match ... ");
            }
            else
            {
                RegisterUser(email, pass);
            }
        }
    }
    public void CreateAcountBtnCallback()
    {
        HelperMan.ShowPanel(signupPanel);
    }
    public void BackToLoginPanelBtnCallback()
    {
        HelperMan.HidePanel(signupPanel);
    }
    public void LogOutBtnCallback()
    {
        LogOut();
    }
    public void OpenStartLessonPanelBtnCallback()
    {
        HideAllPanels();
        HelperMan.ShowPanel(dashboardPanel);
        HelperMan.ShowPanel(startLessonPanel);
        DehighlightAllPanels();
        HelperMan.HighlightPanel(startLessonBtnContainer);

        // Load Lessons 
        loadLessons = true;
    }
    public void OpenRevisionPanelBtnCallback()
    {
        HideAllPanels();
        HelperMan.ShowPanel(dashboardPanel);
        HelperMan.ShowPanel(revisionPanel);
        DehighlightAllPanels();
        HelperMan.HighlightPanel(revisionBtnContainer);
    }
    public void OpenAddCardsPanelBtnCallback()
    {
        HideAllPanels();
        HelperMan.ShowPanel(dashboardPanel);
        HelperMan.ShowPanel(addCardsPanel);
        DehighlightAllPanels();
        HelperMan.HighlightPanel(addCardsBtnContainer);
        if (originalWordIF && translatedWordIF)
        {
            originalWordIF.text = "";
            translatedWordIF.text = "";
        }
    }
    public void OpenProfilePanelBtnCallback()
    {
        HideAllPanels();
        HelperMan.ShowPanel(dashboardPanel);
        HelperMan.ShowPanel(profilePanel);
        DehighlightAllPanels();
        HelperMan.HighlightPanel(profileBtnContainer);

        // Load user data 
        showLoadingPanel = true;
        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(Globals.currentUser.userId)
            .Child("profile").GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    try
                    {
                        Globals.currentUser.userLanguage = snapshot.Child("userLanguage").Value.ToString();
                        Globals.currentUser.userColorMode = snapshot.Child("userColorMode").Value.ToString();
                    }
                    catch
                    {
                        Debug.Log("Could not fetch user info ... ");
                    }
                }
                showLoadingPanel = false;
                loadProfile = true;
            });
    }
    public void SaveCardBtnCallback()
    {
        if (originalWordIF && translatedWordIF)
        {
            string originalWord = originalWordIF.text;
            string translatedWord = translatedWordIF.text;
            if (!(string.IsNullOrEmpty(originalWord)) && !(string.IsNullOrEmpty(translatedWord)))
            {
                // Save Card To Database for this user 
                showLoadingPanel = true;
                Globals.currentUser.email = Globals.email;
                Globals.currentUser.username = Globals.username;
                Globals.currentUser.userId = Globals.userId;
                Card card = new Card(originalWord, translatedWord);
                Globals.currentUser.cards.Add(card);
                string jsonCard = JsonUtility.ToJson(card);
                FirebaseDatabase.DefaultInstance.RootReference
                    .Child("users")
                    .Child(Globals.userId)
                    .Child("cards")
                    .Child(card.id)
                    .SetRawJsonValueAsync(jsonCard).ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            MessageBox.Open("Done!!");
                        }
                        showLoadingPanel = false;
                        loadRevisionCards = true;
                        addedACard = true;
                    });
            }
            else
            {
                MessageBox.Open("Please Fill in all fields ... ");
            }
        }
    }
    #endregion
    #endregion

    #region MainRegion
    #region Functions
    void Awake()
    {
        /* Google Initializations */
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();        // Don't add this when you are running the game in the editor 

        Application.runInBackground = true;
    }
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "LoginMenu")
        {
            HideAllPanels();
            HelperMan.ShowPanel(loginPanel);
            //if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            //{
            //    SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
            //}
        }
        else if (SceneManager.GetActiveScene().name == "Dashboard")
        {
            HideAllPanels();
            DehighlightAllPanels();
            HelperMan.ShowPanel(dashboardPanel);
            OpenStartLessonPanelBtnCallback();
            // Load Resources
            cardSlotPrefab = Resources.Load<GameObject>("CardSlot");
            cardDetailsPanelPrefab = Resources.Load<GameObject>("CardDetailsPanel");
            cardLabelPrefab = Resources.Load<GameObject>("CardLabel");
            profileLabelPrefab = Resources.Load<GameObject>("ProfileLabel");
            profileTogglePrefab = Resources.Load<GameObject>("ProfileToggle");
            languageDropdownPrefab = Resources.Load<GameObject>("LanguageDropdown");
            saveSettingsBtnPrefab = Resources.Load<GameObject>("SaveSettingsBtn");
            lessonSlotPrefab = Resources.Load<GameObject>("LessonSlot");
            lessonPanelPrefab = Resources.Load<GameObject>("LessonPanel");
            cardQuizSlotPrefab = Resources.Load<GameObject>("CardQuizSlot");
            // Assign user info 
            Globals.currentUser.email = Globals.email;
            Globals.currentUser.username = Globals.username;
            Globals.currentUser.userId = Globals.userId;
            // Load Cards for current user
            showLoadingPanel = true;
            FirebaseDatabase.DefaultInstance.RootReference
                .Child("users")
                .Child(Globals.userId)
                .Child("cards")
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        Globals.currentUser.cards.Clear();
                        try
                        {
                            foreach(DataSnapshot snap in snapshot.Children)
                            {
                                Card card = new Card();
                                card.originalWord = snap.Child("originalWord").Value.ToString();
                                card.translatedWord = snap.Child("translatedWord").Value.ToString();
                                card.id = snap.Child("id").Value.ToString();
                                Date creationDate = new Date();
                                creationDate.minute = snap.Child("creationDate").Child("minute").Value.ToString();
                                creationDate.hour = snap.Child("creationDate").Child("hour").Value.ToString();
                                creationDate.day = snap.Child("creationDate").Child("day").Value.ToString();
                                creationDate.month = snap.Child("creationDate").Child("month").Value.ToString();
                                creationDate.year = snap.Child("creationDate").Child("year").Value.ToString();
                                card.creationDate = creationDate;
                                Globals.currentUser.cards.Add(card);
                                loadRevisionCards = true;
                            }
                        }
                        catch
                        {
                            MessageBox.Open("Welcome ... ");
                        }
                        loadLessons = true;
                    }
                    FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(Globals.userId).Child("profile").GetValueAsync().ContinueWith(task1 =>
                    {
                        if (task1.IsCompleted)
                        {
                            DataSnapshot data = task1.Result;
                            try
                            {
                                Globals.currentUser.userColorMode = data.Child("userColorMode").Value.ToString();
                                Globals.currentUser.userLanguage = data.Child("userLanguage").Value.ToString();
                                try
                                {
                                    Globals.currentUser.lessonNotificationStatus = data.Child("lessonNotificationStatus").Value.ToString();
                                }
                                catch
                                {
                                    Debug.Log("Could not load lesson notification status ... ");
                                }
                                userInfoLoaded = true;
                                loadLessons = true;
                            }
                            catch
                            {
                                Debug.Log("Failed to fetch user info ... ");
                            }
                        }
                    });
                    showLoadingPanel = false;
                });
        }
    }
    void Update()
    {
        HelperMan.CheckInternet();
        if (SceneManager.GetActiveScene().name == "LoginMenu")
        {
            if (loadDashboard)
            {
                loadDashboard = false;
                SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Dashboard")
        {
            if (Globals.isLoggedIn == false)
            {
                SceneManager.LoadScene("LoginMenu", LoadSceneMode.Single);
            }
            if (showLoadingPanel)
            {
                HelperMan.ShowPanel(loadingPanel);
            }
            else
            {
                HelperMan.HidePanel(loadingPanel);
            }
            if (addedACard)
            {
                OpenAddCardsPanelBtnCallback();
                addedACard = false;
            }
            if (loadRevisionCards)
            {
                LoadRevisionCardsIntoUI();
                loadRevisionCards = false;
            }
            if (loadProfile)
            {
                LoadProfileElementsIntoUI();
                loadProfile = false;
            }
            if (userInfoLoaded)
            {
                // Adjust colors 
                if (Globals.currentUser.userColorMode == "Dark")
                {
                    if (dashboardPanel)
                    {
                        dashboardPanel.GetComponent<Image>().color = Color.black;
                    }
                }
                else
                {
                    if (dashboardPanel)
                    {
                        dashboardPanel.GetComponent<Image>().color = Color.white;
                    }
                }
                // Adjust language
                if (Globals.currentUser.userLanguage == "AR")
                {
                    if (logOutBtn)
                    {
                        logOutBtn.GetComponentInChildren<Text>().text = ArabicFixer.Fix("خروج");
                    }
                }
                else
                {
                    if (logOutBtn)
                    {
                        logOutBtn.GetComponentInChildren<Text>().text = "Log Out";
                    }
                }
                // Check if user notifcations are scheduled or not
                if (Globals.currentUser.lessonNotificationStatus != "scheduled")
                {
                    var lessonChannel = new AndroidNotificationChannel()
                    {
                        Id = Globals.lessonChannelId,
                        Name = "Lesson Channel",
                        Importance = Importance.High,
                        Description = "Lessons alert channel"
                    };
                    AndroidNotificationCenter.RegisterNotificationChannel(lessonChannel);
                    AndroidNotification lessonNotification = new AndroidNotification();
                    if (Globals.currentUser.userLanguage == "AR")
                    {
                        lessonNotification.Title = "تدرب على درس اليوم";
                        lessonNotification.Text = "راجع بعض الكلمات اليوم وحافظ على تقدمك ...";
                    }
                    else
                    {
                        lessonNotification.Title = "Practice today's lesson";
                        lessonNotification.Text = "Revise some words today and keep your progress ... ";
                    }
                    lessonNotification.FireTime = DateTime.Now.AddMinutes(2);
                    lessonNotification.RepeatInterval = DateTime.Now - DateTime.Now.AddDays(1);
                    lessonNotification.Style = NotificationStyle.BigTextStyle;
                    AndroidNotificationCenter.SendNotification(lessonNotification, Globals.lessonChannelId);
                    Globals.currentUser.lessonNotificationStatus = "scheduled";

                    // Save the scheduled status of the user 
                    string jsonUser = JsonUtility.ToJson(Globals.currentUser);
                    FirebaseDatabase.DefaultInstance.RootReference
                        .Child("users")
                        .Child(Globals.currentUser.userId)
                        .Child("profile")
                        .SetRawJsonValueAsync(jsonUser).ContinueWith(scheduleTask =>
                        {
                            if (scheduleTask.IsCompleted)
                            {
                                Debug.Log("Saved Notifications Status ... ");
                            }
                        });
                }
                userInfoLoaded = false;
            }
            if (loadLessons)
            {
                LoadLessonSlotsUI();
                loadLessons = false;
            }
        }
    }
    #endregion
    #endregion

    #region RevisionArea
    #region Variables
    GameObject lessonSlotPrefab;
    List<GameObject> lessonsList = new List<GameObject>();
    GameObject lessonPanelPrefab;
    GameObject cardQuizSlotPrefab;
    GameObject currentCardQuizObj;

    bool loadLessons = false;
    #endregion

    #region Functionality
    void LoadLessonSlotsUI()
    {
        // Compose 2 lessons for the user to practice with (Recent lesson, Random lesson)
        if (lessonSlotPrefab && startLessonPanel)
        {
            // Clear slots from UI first 
            for (int k = 0; k < lessonsList.Count; k++)
            {
                if (lessonsList[k])
                {
                    Destroy(lessonsList[k].gameObject);
                }
            }
            lessonsList.Clear();

            // Recent lesson
            Lesson recentLesson = new Lesson();
            if (Globals.currentUser.userLanguage == "AR")
            {
                recentLesson.title = ArabicFixer.Fix("اخر التحديثات");
                recentLesson.description = ArabicFixer.Fix("تجميعه من اخر البطاقات المضافة ...");
            }
            else
            {
                recentLesson.title = "Recently Added";
                recentLesson.description = "Collection of the recently added cards ... ";
            }
            for (int i = 0; i < Globals.currentUser.cards.Count; i++)
            {
                if (Globals.currentUser.cards[i].creationDate.day == DateTime.Now.Day.ToString())
                {
                    // this card was added today so put it in recent lesson 
                    if (recentLesson.cards.Count < 20)
                    {
                        recentLesson.cards.Add(Globals.currentUser.cards[i]);
                    }
                }
            }

            // Random lesson
            Lesson randomLesson = new Lesson();
            if (Globals.currentUser.userLanguage == "AR")
            {
                randomLesson.title = ArabicFixer.Fix("درس عشوائي");
                randomLesson.description = ArabicFixer.Fix("تجميعه عشوائية من البطاقات الحالية ...");
            }
            else
            {
                randomLesson.title = "Random Lesson";
                randomLesson.description = "Collection of randomly selected cards ... ";
            }
            for (int j = 0; j < Globals.currentUser.cards.Count; j++)
            {
                // this card is selected randomly
                if (randomLesson.cards.Count < 20)
                {
                    randomLesson.cards.Add(Globals.currentUser.cards[UnityEngine.Random.Range(0, Globals.currentUser.cards.Count)]);
                }
            }

            // Create a panel that contains lessons slots 
            GameObject recentLessonSlotObj = Instantiate(lessonSlotPrefab, startLessonPanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
            GameObject randomLessonSlotObj = Instantiate(lessonSlotPrefab, startLessonPanel.GetComponentsInChildren<Image>()[1].gameObject.transform);
            // Assign their lesson references 
            recentLessonSlotObj.GetComponent<LessonSlot>().currentLesson = recentLesson;
            randomLessonSlotObj.GetComponent<LessonSlot>().currentLesson = randomLesson;
            // Assign their labels 
            recentLessonSlotObj.GetComponentsInChildren<Text>()[0].text = recentLessonSlotObj.GetComponent<LessonSlot>().currentLesson.title;
            recentLessonSlotObj.GetComponentsInChildren<Text>()[1].text = recentLessonSlotObj.GetComponent<LessonSlot>().currentLesson.description;
            randomLessonSlotObj.GetComponentsInChildren<Text>()[0].text = randomLessonSlotObj.GetComponent<LessonSlot>().currentLesson.title;
            randomLessonSlotObj.GetComponentsInChildren<Text>()[1].text = randomLessonSlotObj.GetComponent<LessonSlot>().currentLesson.description;
            // Assign their on click btn callback 
            recentLessonSlotObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                Debug.Log("Will be implemented in a sec .. ");
                // Begin Lesson and schedule 20 cards 
                if (lessonPanelPrefab && cardSlotPrefab && dashboardPanel)
                {
                    // Add Lesson panel to dashboard 
                    GameObject lessonPanelObj = Instantiate(lessonPanelPrefab, dashboardPanel.transform);
                    // Assign its close btn callback 
                    lessonPanelObj.GetComponentsInChildren<Button>()[lessonPanelObj.GetComponentsInChildren<Button>().Length - 1].onClick.AddListener(delegate
                    {
                        // Close this panel
                        Destroy(lessonPanelObj.gameObject);
                    });
                    if (recentLesson.cards.Count > 0)
                    {
                        // Add card quizez recursively
                        int iterator = 0;
                        AddCardQuiz(iterator, lessonPanelObj, recentLesson);
                    }
                    else
                    {
                        if (Globals.currentUser.userLanguage == "AR")
                        {
                            MessageBox.Open(ArabicFixer.Fix("لا شئ هنا بعد ... "));
                        }
                        else
                        {
                            MessageBox.Open("Nothing here yet ... ");
                        }
                    }
                }
            });
            randomLessonSlotObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                Debug.Log("Will be implemented in a sec .. ");
                // Add Lesson panel to dashboard 
                GameObject lessonPanelObj = Instantiate(lessonPanelPrefab, dashboardPanel.transform);
                // Assign its close btn callback 
                lessonPanelObj.GetComponentsInChildren<Button>()[lessonPanelObj.GetComponentsInChildren<Button>().Length - 1].onClick.AddListener(delegate
                {
                    // Close this panel
                    Destroy(lessonPanelObj.gameObject);
                });
                if (randomLesson.cards.Count > 0)
                {
                    // Add card quizez recursively
                    int iterator = 0;
                    AddCardQuiz(iterator, lessonPanelObj, randomLesson);
                }
                else
                {
                    if (Globals.currentUser.userLanguage == "AR")
                    {
                        MessageBox.Open(ArabicFixer.Fix("لا شئ هنا بعد ... "));
                    }
                    else
                    {
                        MessageBox.Open("Nothing here yet ... ");
                    }
                    lessonPanelObj.GetComponentsInChildren<Button>()[lessonPanelObj.GetComponentsInChildren<Button>().Length - 1].onClick.Invoke();
                }
            });

            // Keep track of added lessons 
            lessonsList.Add(recentLessonSlotObj);
            lessonsList.Add(randomLessonSlotObj);
        }
    }
    void AddCardQuiz(int index, GameObject lessonPanelObj, Lesson lesson)
    {
        if (currentCardQuizObj)
        {
            Destroy(currentCardQuizObj.gameObject);
        }

        if (index < lesson.cards.Count)
        {
            currentCardQuizObj = Instantiate(cardQuizSlotPrefab, lessonPanelObj.GetComponentsInChildren<Image>()[1].gameObject.transform);
            // Assign title 
            if (Globals.currentUser.userLanguage == "AR")
            {
                currentCardQuizObj.GetComponentsInChildren<Text>()[0].text = ArabicFixer.Fix("بطاقة " + index + 1);
            }
            else
            {
                currentCardQuizObj.GetComponentsInChildren<Text>()[0].text = "Card " + (index + 1);
            }
            // Assign content 
            if (lesson.cards.Count != 0)
            {
                currentCardQuizObj.GetComponentsInChildren<Text>()[1].text = lesson.cards[index].originalWord;
            }
            // Configure InputField for Arabic input
            currentCardQuizObj.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate
            {
                if (HelperMan.FindLang(currentCardQuizObj.GetComponentInChildren<InputField>().text) == "Arabic")
                {
                    currentCardQuizObj.GetComponentInChildren<InputField>().text = ArabicFixer.Fix(currentCardQuizObj.GetComponentInChildren<InputField>().text);
                }
            });
            // Assign Next Btn callback 
            currentCardQuizObj.GetComponentsInChildren<Button>()[currentCardQuizObj.GetComponentsInChildren<Button>().Length - 1].onClick.AddListener(delegate
            {
                // Check the user answer 
                if (!(string.IsNullOrEmpty(currentCardQuizObj.GetComponentInChildren<InputField>().text)))
                {
                    string answer = currentCardQuizObj.GetComponentInChildren<InputField>().text;
                    answer = answer.ToLower();
                    if (answer == lesson.cards[index].translatedWord.ToLower())
                    {
                        // Proceed to next lesson showing a right message 
                        if (Globals.currentUser.userLanguage == "AR")
                        {
                            MessageBox.Open(ArabicFixer.Fix("احسنت ..."));
                        }
                        else
                        {
                            MessageBox.Open("Well Done ... ");
                        }
                        AddCardQuiz(index + 1, lessonPanelObj, lesson);
                    }
                    else if (lesson.cards[index].translatedWord.ToLower().Contains(answer.ToLower()))
                    {
                        // Proceed to next lesson but with a half point
                        if (Globals.currentUser.userLanguage == "AR")
                        {
                            MessageBox.Open(ArabicFixer.Fix("الاجابه الاصح هي \n" + lesson.cards[index].translatedWord));
                        }
                        else
                        {
                            MessageBox.Open("Another correct answer: " + lesson.cards[index].translatedWord);
                        }
                        AddCardQuiz(index + 1, lessonPanelObj, lesson);
                    }
                    else
                    {
                        // the answer was wrong 
                        if (Globals.currentUser.userLanguage == "AR")
                        {
                            MessageBox.Open(ArabicFixer.Fix("الاجابه خاطئة. الاجابه الصحيحه هي: \n") + lesson.cards[index].translatedWord);
                        }
                        else
                        {
                            MessageBox.Open("Answer is wrong. Correct one is " + lesson.cards[index].translatedWord);
                        }
                        AddCardQuiz(index + 1, lessonPanelObj, lesson);
                    }
                }
                else
                {
                    if (Globals.currentUser.userLanguage == "AR")
                    {
                        MessageBox.Open(ArabicFixer.Fix("ادخل اجابه اولا"));
                    }
                    else
                    {
                        MessageBox.Open("Please enter an answer ... ");
                    }
                }
            });
        }
        else
        {
            // Lesson finished 
            if (Globals.currentUser.userLanguage == "AR")
            {
                MessageBox.Open(ArabicFixer.Fix("الدرس انتهى ..."));
            }
            else
            {
                MessageBox.Open("Lesson Completed ... ");
            }
            Destroy(lessonPanelObj.gameObject);
        }
    }
    #endregion
    #endregion
}
