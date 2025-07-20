using System;

public interface IAuther {
    
    public void Initialize();
    public void TrySignIn(string id, string pw, Action callback, Action<string> fallback);
    public void TrySignUp(string id, string pw, Action callback, Action<string> fallback);
    public void SignOut();
    public bool IsSignIn();

    public void EmailVerify(Action callback, Action<string> fallback);
    public bool IsEmailVerified();
    public void ReVerify(string pw, Action callback, Action<string> fallback);

    public void DisplayNameChange(string newName, Action callback, Action<string> fallback);
    public void PasswordChange(string newPassword, Action callback, Action<string> fallback);
    public void DeleteUser(Action callback, Action<string> fallback);

    public string GetUserId();
    public string GetName();

    public void GetUserData(Action<UserData> callback, Action<string> fallback);
    public void UpdateUserData(UserData update);
}

public class DefaultAuther : IAuther {

    public void TrySignIn(string id, string pw, Action callback, Action<string> fallback) {
        callback?.Invoke();
    }
    
    public void TrySignUp(string id, string pw, Action callback, Action<string> fallback)
    {
        callback?.Invoke();
    }

    public void SignOut() { 
        
    }

    public bool IsSignIn() => false;

    public void EmailVerify(Action callback, Action<string> fallback) {
        callback?.Invoke();
    }

    public bool IsEmailVerified() => false;

    public void ReVerify(string pw, Action callback, Action<string> fallback) {
        callback?.Invoke();
    }

    public string GetUserId() => "Test";

    public string GetName() => "Test";

    public void Initialize()
    {

    }

    public void DisplayNameChange(string newName, Action callback, Action<string> fallback)
    {
        callback?.Invoke();
    }

    public void PasswordChange(string newPassword, Action callback, Action<string> fallback)
    {
        callback?.Invoke();
    }

    public void DeleteUser(Action callback, Action<string> fallback)
    {
        callback?.Invoke();
    }

    public void GetUserData(Action<UserData> callback, Action<string> fallback)
    { 
        
    }
    
    public void UpdateUserData(UserData update)
    {

    }

}