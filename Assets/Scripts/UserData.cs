
[System.Serializable]
public class UserData
{
    public string UserName;
    public int Cash;
    public int Balance;
    public string ID;
    public string Password;
    public UserData(string UserName, int Cash, int Balance, string ID, string Password)
    {
        this.UserName = UserName;
        this.Cash = Cash;
        this.Balance = Balance;
        this.ID = ID;
        this.Password = Password;
    }
}



