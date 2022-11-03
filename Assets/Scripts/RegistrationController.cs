using ModularMotion;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[Serializable]
public class User
{
    //public int id;
    public string name;
    public string username;
    public string email;
    public string phone;
    public string surename;





    public override string ToString()
    {
        //return UnityEngine.JsonUtility.ToJson(this, true);
        return name + " " + email;
    }
}


public class RegistrationController : MonoBehaviour
{


    private readonly string basePath = "https://localhost:7249";
    private RequestHelper currentRequest;

    [Header("Blocks")]
    public GameObject RegistrationBlock;
    public GameObject RegConfirmationBlock;
    
    
    [Header("Inputs")]
    public TMP_InputField firstName;
    public TMP_InputField sureName;
    public TMP_InputField mobile;
    public TMP_InputField userName;
    public TMP_InputField email;
    
    [Header("Validators")]
    public TextMeshProUGUI firstNameValidation;
    public TextMeshProUGUI surenameValidation;
    public TextMeshProUGUI emailValidation;
    public TextMeshProUGUI mobileValidation;
    public TextMeshProUGUI usernameValidation;

    [Header("Motions")]
    public UIMotion RegMotion;
    public UIMotion RegConfMotion;

    [Header("Colors")]
    public Color invalidColor;

    [Header("Buttons")]
    public Button RegisterButton;
    public Button ConfirmButton;




    private bool firstNameIsValid = false;
    private bool surenameIsValid = false;
    private bool emailIsValid = false;
    private bool mobileIsValid = false;
    private bool usernameIsValid = false;
    private bool isConfirmationPhase = false;
    // Start is called before the first frame update
    void Start()
    {
        RegConfirmationBlock.SetActive(false);
        firstNameValidation.gameObject.SetActive(false);
        surenameValidation.gameObject.SetActive(false);
        emailValidation.gameObject.SetActive(false);
        mobileValidation.gameObject.SetActive(false);
        usernameValidation.gameObject.SetActive(false);

        //ES3.Save("myInt", 123);

        if (ES3.KeyExists("myInt"))
        {
            int myInt = ES3.Load<int>("myInt");
        }

    }

    Guid? userId = null;
    public void ReverseRegistration()
    {
        //RegMotion.WrapMode = ModularMotion.Wrap.Once;
        var usersRoute = basePath + "/user/registerClient";

        RestClient.Post(usersRoute, new User { name = firstName.text,surename=sureName.text, email = email.text, phone = mobile.text, username = userName.text}).Then(res =>
        {
            if (res.Text!="-1")
            {
                
                userId = Guid.Parse(res.Text);
                string tmpId = userId.ToString();
                Guid myuserId = Guid.Parse(tmpId);
                ES3.Save("id", myuserId);
                RegisterButton.interactable = true;
                RegMotion.PlayAllBackward();
                isConfirmationPhase = true;
                
            }


        }).Catch(err =>
        {
            var error = err as RequestException;
            LogMessage("Error Response", error.Response);
        });


        
    }


    public void onEndRegConfirmationMotion()
    {
        RegistrationBlock.SetActive(false);
    }
    public void onEndRegistrationMotion()
    {
        if (isConfirmationPhase)
        {
            RegConfirmationBlock.SetActive(true);
            RegConfMotion.PlayAll();
        }

    }
    // Update is called once per frame
    void Update()
    {
        CheckRegisterValidation();
    }

    private void CheckRegisterValidation()
    {
        if (isConfirmationPhase==false)
        {
            if (firstNameIsValid && surenameIsValid && usernameIsValid && mobileIsValid && emailIsValid)
            {
                RegisterButton.interactable = true;
            }
            else
            {
                RegisterButton.interactable=false;
            }
        }
    }

    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
		EditorUtility.DisplayDialog (title, message, "Ok");
#else
        Debug.Log(message);
#endif
    }


    public void CheckEmailIsValidOrNot(string stringToValidate)
    {
        if (emailIsValid)
        {
            RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

            var usersRoute = basePath + "/user/checkemail/" + stringToValidate;

            RestClient.Get(usersRoute).Then(res =>
            {
                if (res.Text == "ok")
                {
                    Debug.Log("Email is Valid!");
                    email.image.color = Color.green;
                    emailIsValid = true;
                    emailValidation.gameObject.active = false;
                }
                else
                {
                    Debug.Log("Email is NOT Valid");
                    email.image.color = Color.red;
                    emailIsValid = false;
                    emailValidation.gameObject.active = true;
                    emailValidation.text = "Email is already taken!";
                    emailValidation.color = invalidColor;
                }
            }).Catch(err =>
            {
                var error = err as RequestException;
                LogMessage("Error Response", error.Response);
            });

        }
    }
    public void CheckMobileIsValidOrNot(string stringToValidate)
    {
        if (mobileIsValid)
        {
            RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

            var usersRoute = basePath + "/user/checkmobile/" + stringToValidate;

            RestClient.Get(usersRoute).Then(res =>
            {
                if (res.Text == "ok")
                {
                    Debug.Log("Mobile is Valid!");
                    mobile.image.color = Color.green;
                    mobileIsValid = true;
                    mobileValidation.gameObject.active = false;
                }
                else
                {
                    Debug.Log("Mobile is NOT Valid");
                    mobile.image.color = Color.red;
                    mobileIsValid = false;
                    mobileValidation.gameObject.active = true;
                    mobileValidation.text = "Mobile number is already taken!";
                    mobileValidation.color = invalidColor;
                }
            }).Catch(err =>
            {
                var error = err as RequestException;
                LogMessage("Error Response", error.Response);
            });

        }
    }
    public void CheckUsernameIsValidOrNot(string stringToValidate)
    {
        if (usernameIsValid)
        {
            RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

            var usersRoute = basePath + "/user/checkusername/" + stringToValidate;

            RestClient.Get(usersRoute).Then(res =>
            {
                if (res.Text == "ok")
                {
                    Debug.Log("Username is Valid!");
                    userName.image.color = Color.green;
                    usernameIsValid = true;
                    usernameValidation.gameObject.active = false;
                }
                else
                {
                    Debug.Log("Username is NOT Valid");
                    userName.image.color = Color.red;
                    usernameIsValid = false;
                    usernameValidation.gameObject.active = true;
                    usernameValidation.text = "Username is already taken!";
                    usernameValidation.color = invalidColor;
                }
            }).Catch(err =>
            {
                var error = err as RequestException;
                LogMessage("Error Response", error.Response);
            });

        }
    }

    public void EmailValidator(string charToValidate)
    {
        try
        {
            MailAddress m = new MailAddress(charToValidate);

            email.image.color = Color.green;
            emailIsValid = true;
        }
        catch (Exception e)
        {
            email.image.color = Color.red;
            emailIsValid = false;
        }


    }
    public void MobileValidator(string charToValidate)
    {
        if (charToValidate.StartsWith("00") && charToValidate.Length > 11)
        {

            mobileValidation.gameObject.active = false;
            mobile.image.color = Color.green;
            mobileIsValid = true;
        }
        else
        {
            mobileValidation.gameObject.active = true;
            mobileValidation.text = "Mobile number should be started with 00 & atleast 11 chars";
            mobileValidation.color = invalidColor;

            mobile.image.color = Color.red;
            mobileIsValid = false;
        }


    }
    public void UsernameValidator(string charToValidate)
    {
        if (charToValidate.Length > 4)
        {

            usernameValidation.gameObject.active = false;
            userName.image.color = Color.green;
            usernameIsValid = true;
        }
        else
        {
            usernameValidation.gameObject.active = true;
            usernameValidation.text = "Username should be atleast 5 chars";
            usernameValidation.color = invalidColor;

            userName.image.color = Color.red;
            usernameIsValid = false;
        }


    }
    public void NameValidator(string charToValidate)
    {
        if (charToValidate.Length > 2)
        {

            firstNameValidation.gameObject.active = false;
            firstName.image.color = Color.green;
            firstNameIsValid = true;
        }
        else
        {
            firstNameValidation.gameObject.active = true;
            firstNameValidation.text = "First Name should be atleast 2 chars";
            firstNameValidation.color = invalidColor;

            firstName.image.color = Color.red;
            firstNameIsValid = false;
        }


    }
    public void SurenameValidator(string charToValidate)
    {
        if (charToValidate.Length > 2)
        {

            surenameValidation.gameObject.active = false;
            sureName.image.color = Color.green;
            surenameIsValid = true;
        }
        else
        {
            surenameValidation.gameObject.active = true;
            surenameValidation.text = "Sure Name should be atleast 2 chars";
            surenameValidation.color = invalidColor;

            sureName.image.color = Color.red;
            surenameIsValid = false;
        }


    }
}
