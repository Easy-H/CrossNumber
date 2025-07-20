#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using EHTool.DBKit;

public class FirebaseAuther : IAuther {

    private FirebaseAuth _auth;
    private IDatabaseConnector<string, UserData> _userDBConnect;

    private IList<int> GetFirebaseErrorCode(AggregateException ex)
    {

        IList<int> retval = new List<int>();
        foreach (Exception e in ex.InnerExceptions)
        {
            if (e is FirebaseException fbEx)
                retval.Add(fbEx.ErrorCode);
        }

        return retval;
    }

    private string GetErrorMessage(IList<int> errorCodes) {
        string retval = string.Empty;
        foreach (int e in errorCodes) {
            retval += GetErrorMessage(e);
        }
        return retval;
    }

    private string GetErrorMessage(int errorCode) {


        foreach (var value in Enum.GetValues(typeof(AuthError)))
        {
            if (errorCode == (int)value)
            {
                return value.ToString();

            }
        }
        return string.Empty;
    }

    public void Initialize()
    {
        _auth = FirebaseAuth.DefaultInstance;

        _userDBConnect = new FirestoreConnector<string, UserData>();
        _userDBConnect.Connect("user", "user");

    }

    public void TrySignUp(string id, string pw, Action callback, Action<string> fallback)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(id, pw).ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled)
            {
                fallback("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Create User Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            callback();
        });
    }

    public void TrySignIn(string id, string pw, Action callback, Action<string> fallback)
    {
        _auth.SignInWithEmailAndPasswordAsync(id, pw).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                fallback?.Invoke("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Login Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }

            AuthResult result = task.Result;

            callback();
        });
    }

    public void SignOut()
    {
        _auth.SignOut();
    }

    public bool IsSignIn() => _auth.CurrentUser != null;

    public void EmailVerify(Action callback, Action<string> fallback)
    {
        if (!IsSignIn())
        {
            fallback?.Invoke("Not Logined");
            return;
        }

        _auth.CurrentUser.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                fallback?.Invoke("SendEmailVerificationAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Verify Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }

            Debug.Log("Email sent successfully.");
            callback?.Invoke();

        });
    }

    public bool IsEmailVerified() {
        return _auth.CurrentUser.IsEmailVerified;
    }

    public void ReVerify(string pw, Action callback, Action<string> fallback) {

        if (!IsSignIn())
        {
            fallback?.Invoke("Not Logined");
            return;
        }

        Credential credential = EmailAuthProvider.GetCredential(_auth.CurrentUser.Email, pw);

        _auth.CurrentUser.ReauthenticateAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                fallback?.Invoke("ReauthenticateAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Reauthenticate Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }

            callback?.Invoke();

        });

    }

    public void DisplayNameChange(string newName, Action callback, Action<string> fallback)
    {
        if (!IsSignIn())
        {
            fallback?.Invoke("Not Logined");
            return;
        }

        UserProfile profile = new UserProfile
        {
            DisplayName = newName
        };

        _auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                fallback?.Invoke("UpdateUserProfileAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Update User Profile Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }
            callback?.Invoke();
        });
    }

    public void PasswordChange(string newPassword, Action callback, Action<string> fallback)
    {
        if (!IsSignIn())
        {
            fallback?.Invoke("Not Logined");
            return;
        }

        _auth.CurrentUser.UpdatePasswordAsync(newPassword).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                fallback?.Invoke("UpdatePasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                IList<int> errorCode = GetFirebaseErrorCode(task.Exception);
                string message = "Update Password Failed: ";
                message += GetErrorMessage(errorCode);

                fallback?.Invoke(message);
                return;
            }
            callback?.Invoke();

        });

    }

    public void DeleteUser(Action callback, Action<string> fallback)
    {
        if (!IsSignIn())
        {
            fallback?.Invoke("Not Logined");
            return;
        }

        _auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                fallback?.Invoke("DeleteAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                fallback?.Invoke("DeleteAsync encountered an error: " + task.Exception);
                return;
            }

            callback?.Invoke();
        });

    }
    public string GetUserId()
    {

        if (IsSignIn()) return _auth.CurrentUser.UserId;

        return "Test";
    }

    public string GetName()
    {
        if (IsSignIn()) return _auth.CurrentUser.DisplayName;

        return "Test";

    }
    
    public void GetUserData(Action<UserData> callback, Action<string> fallback)
    {
        _userDBConnect.GetRecordAt(GetUserId(), callback, fallback);
    }
    
    public void UpdateUserData(UserData update)
    {
        _userDBConnect.UpdateRecordAt(GetUserId(), update);
    }

}
#endif