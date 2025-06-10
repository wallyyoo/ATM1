using System.IO;
using UnityEngine;


public class SaveSystem 
{
  private static string UsersFilePath => Application.persistentDataPath + "/saves/users.json"; //저장경로
  public enum loginResult {Success, NoId, WrongPassword}
  public static UserDataList LoadAllUsers()
  {
    if (!File.Exists(UsersFilePath))
      return new UserDataList();

    string json = File.ReadAllText(UsersFilePath);
    return JsonUtility.FromJson<UserDataList>(json);
  }
  public static void SaveAllUsers(UserDataList dataList)
  {   
    
    string folderPath = Application.persistentDataPath + "/saves/";
    if(!Directory.Exists(folderPath))
    {
      Directory.CreateDirectory(folderPath);
      Debug.Log("저장 폴더 생성됨: " + folderPath);
    }
    
    string json = JsonUtility.ToJson(dataList, true);
    Debug.Log("저장할 JSON: " + json);  
    File.WriteAllText(UsersFilePath, json);
    Debug.Log("파일 저장 완료: " + UsersFilePath);  
  }
  public static loginResult TryLogin(string id, string password)
  {
    var allUsers = LoadAllUsers();
    Debug.Log("로그인 시도: ID = " + id);
    var user = allUsers.users.Find(u => u.ID == id);
    Debug.Log("전체 유저 수: " + allUsers.users.Count);
    
    if (user == null)
    {
      return loginResult.NoId;
    }

    if (user.Password != password)
    {
      return loginResult.WrongPassword;
    }
    
    GameManager.Instance.userData = user;
    
    return loginResult.Success;
  }
  public static bool TrySignUp(string id, string password, string userName)
  {
    var allUsers = LoadAllUsers();
    if (allUsers.users.Exists(u =>u.ID == id))
      return false;
    UserData newUser = new UserData(userName, 100000, 0, id, password);
    allUsers.users.Add(newUser);
    SaveAllUsers(allUsers);
    return true;
  }
  
  public static bool TryTransfer(string toId, int amount)
  {
    if (GameManager.Instance.userData.Balance < amount || amount <= 0)
      return false;
    
    var allUsers = LoadAllUsers();
    var toUser = allUsers.users.Find(u => u.ID == toId);
    
    if(toUser == null)
      return false;

    var myUser = allUsers.users.Find(u => u.ID == GameManager.Instance.userData.ID);
    myUser.Balance -= amount;
    toUser.Balance += amount;
    SaveAllUsers(allUsers);
    GameManager.Instance.userData.Balance -= amount;
    
    GameManager.Instance.Refresh();
    return true;
  }
  
  /*
  public static void Save(UserData userData, string saveFileName)
  {
  
    if (!Directory.Exists(SavePath)) 
    {
      Directory.CreateDirectory(SavePath); // 폴더를 생성
    }
    string saveJson = JsonUtility.ToJson(userData); // 유저데이터를 json으로 변환
    string saveFilepath = SavePath + saveFileName + ".json"; // 여기에 저장
    File.WriteAllText(saveFilepath, saveJson);
    Debug.Log("Save successfully" + saveFilepath);
  }

  public static UserData Load(string saveFileName)
  {
    string saveFilePath = SavePath + saveFileName + ".json";
    if (!File.Exists(saveFilePath))
    {
      Debug.LogError("Save file not found");
      return null;
    }
    string saveFile = File.ReadAllText(saveFilePath); 
    UserData saveData = JsonUtility.FromJson<UserData>(saveFile);
    return saveData;
  }*/
  
}
