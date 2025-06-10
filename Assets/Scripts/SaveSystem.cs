using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class SaveSystem 
{
  private static string UsersFilePath => Application.persistentDataPath + "/saves/users.json"; //저장경로
  public enum loginResult {Success, NoId, WrongPassword}
  public static UserDataList LoadAllUsers() // 유저데이터 불러오기
  {
    Debug.Log("LoadAllUsers 호출됨");
    if (!File.Exists(UsersFilePath))
    {
      Debug.Log("파일이 존재하지 않음: " + UsersFilePath);
      return new UserDataList { users = new List<UserData>() };
    }

    string json = File.ReadAllText(UsersFilePath);
    Debug.Log("읽은 JSON 내용: " + json);
    UserDataList data = JsonUtility.FromJson<UserDataList>(json);

    if (data.users == null) 
    {
      Debug.Log("users 리스트가 null임, 새 리스트 생성");
      data.users = new List<UserData>();
    }
    else
    {
      Debug.Log("users 리스트 크기: " + data.users.Count);
    }
    return data;
  }
  public static void SaveAllUsers(UserDataList dataList) // 유저데이터 저장
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
  public static loginResult TryLogin(string id, string password) //로그인 시도
  {
    id = id.Trim();
    password = password.Trim();
    
    Debug.Log($"Trimmed ID: '{id}', Trimmed PW: '{password}'");
    
    var allUsers = LoadAllUsers();
    foreach (var u in allUsers.users)
    {
      Debug.Log($"유저 DB ID='{u.ID}', PW='{u.Password}'");
    }

    var user = allUsers.users.Find(u => u.ID.Trim() == id.Trim());
    if (user == null)
    {
      Debug.Log("로그인 실패: 아이디 없음");
      return loginResult.NoId;
    }

    if (user.Password.Trim() != password.Trim())
    {
      Debug.Log("로그인 실패: 비밀번호 틀림");
      return loginResult.WrongPassword;
    }

    Debug.Log("로그인 성공!");
    GameManager.Instance.userData = user;
    return loginResult.Success;
  }
  public static bool TrySignUp(string id, string password, string userName) // 회원가입 시도
  {
    var allUsers = LoadAllUsers();
    if (allUsers.users.Exists(u =>u.ID.Trim() == id.Trim()))
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
