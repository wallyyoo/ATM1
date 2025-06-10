
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UserData userData; //유저데이터 가져오기
    
    public TextMeshProUGUI userNameTXT;
    public TextMeshProUGUI cashTXT;
    public TextMeshProUGUI balanceTXT;
    
    private void Awake()// 싱글턴
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject) ;
    }
    void Start()
    {
        Refresh();
    }
    public bool TryDeposit(int amount)
    {
        if (amount > 0 && userData.Cash >= amount)
        {
            userData.Cash -= amount;
            userData.Balance += amount;
            Refresh();
            SaveAllUserData();
            return true;
        }
        else return false;
    }//입금 처리 + 유저데이터 저장
    public bool TryWithdraw(int amount)
    {
        if (amount > 0 && userData.Balance >= amount)
        {
            userData.Balance -= amount;
            userData.Cash += amount;
            Refresh();
            SaveAllUserData();
            return true;
        }
        else return false;
    }//출금 처리 + 유저데이터 저장
    public void Refresh()  
    {
        
        if (userNameTXT != null)
            userNameTXT.text = userData.UserName;

        if (cashTXT != null)
            cashTXT.text = userData.Cash.ToString("#,##0");

        if (balanceTXT != null)
            balanceTXT.text = $"잔액 : {userData.Balance:#,##0}";
           
    }// ui갱신
    public void SaveAllUserData()
        {
            var allUsers = SaveSystem.LoadAllUsers();
            var myUser = allUsers.users.Find(u => u.ID == userData.ID);
            if (myUser != null)
            {
                myUser.Cash = userData.Cash;
                myUser.Balance = userData.Balance;
            }
            SaveSystem.SaveAllUsers(allUsers);
        }// 현재 유저 데이터 저장

}
   


