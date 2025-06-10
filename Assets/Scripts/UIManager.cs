using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("기본 UI")]
    public GameObject DnW_Btn_panel;
    public GameObject depositPanel;
    public GameObject withdrawPanel;
    public GameObject depositError;
    public GameObject withdrawError;
    
    [Header("직접입력 UI")]
    public TMP_InputField depositInputField;
    public TMP_InputField withdrawInputField;
    
    [Header("로그인 관련UI")]
    public GameObject loginPanel;
    public GameObject loginErrorPopup;
    public TMP_InputField loginIdInputField;
    public TMP_InputField loginPwInputField;
    public TMP_Text loginErrorText; 
    
    [Header("회원가입 관련UI")]
    public GameObject signInPanel;
    public GameObject signUpErrorPopup;
    public TMP_InputField signUpIdInputField;
    public TMP_InputField signUpNameInputField;
    public TMP_InputField signUpPwInputField;
    public TMP_InputField signUpPwConfirmInputField;
    public TMP_Text signUpErrorText;
    
    
    private Coroutine depositErrorCoroutine;
    private Coroutine withdrawErrorCoroutine;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        ShowlogInPanel();
    }

   /* public void OnDepositButtonClick()
    {
        int amount;
        if (int.TryParse(depositInputField.text, out amount) && amount > 0)
        {
            var userData = GameManager.Instance.userData;
            if (userData.Cash >= amount)
            {
               GameManager.Instance.TryDeposit(amount);
            }
            else
            {
                ShowDepositError();
            }
        }
        else
        {
            ShowDepositError();
        }
    }

    public void OnWithdrawButtonClick()
    {
        int amount;
        if (int.TryParse(withdrawInputField.text, out amount) && amount > 0)
        {
            var userData = GameManager.Instance.userData;
            if (userData.Balance >= amount)
            {
                GameManager.Instance.TryWithdraw(amount);
            }

            else
            {
                ShowWithdrawError();
            }
        }
        else
        {
            ShowWithdrawError();
        }
    } */
   
   //각종 창 전환 on off 방식
    public void ShowMainButtons()
    {
        DnW_Btn_panel.SetActive(true);
        depositPanel.SetActive(false);
        withdrawPanel.SetActive(false);
        depositError.SetActive(false);
        withdrawError.SetActive(false);
        signInPanel.SetActive(false);
        loginPanel.SetActive(false);
    }
    public void ShowlogInPanel()
    {
        DnW_Btn_panel.SetActive(false);
        depositPanel.SetActive(false);
        withdrawPanel.SetActive(false);
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
        signUpErrorPopup.SetActive(false);
        loginErrorPopup.SetActive(false);
    }    
    public void ShowSignInPanel()
    {
        DnW_Btn_panel.SetActive(false);
        depositPanel.SetActive(false);
        withdrawPanel.SetActive(false);
        signInPanel.SetActive(true);
        loginPanel.SetActive(false);
        signUpErrorPopup.SetActive(false);
        loginErrorPopup.SetActive(false);
    }    
    public void ShowDepositPanel()
    {
        depositPanel.SetActive(true);
        DnW_Btn_panel.SetActive(false);
        signInPanel.SetActive(false);
        loginPanel.SetActive(false);
    
    }
    public void ShowWithdrawPanel()
    {
        withdrawPanel.SetActive(true);
        DnW_Btn_panel.SetActive(false);
        signInPanel.SetActive(false);
        loginPanel.SetActive(false);

    }
    
    //에러 안내
    public void ShowDepositError()
    {
        if (depositErrorCoroutine == null)
        {
            depositErrorCoroutine = StartCoroutine(DepositErrorPopupControl());
        }
    }
    public void ShowWithdrawError()
    {
        if (withdrawErrorCoroutine == null)
        {
            withdrawErrorCoroutine = StartCoroutine(WithdrawErrorPopupControl());
        }
    }
    public void ShowLoginError(string msg)
    {
        loginErrorText.text = msg;
        loginErrorPopup.SetActive(true);
        StartCoroutine(HideLoginErrorAfterSeconds(2f));
    }
    public void ShowSignUpError(string msg)
    {
        signUpErrorText.text = msg;
        signUpErrorPopup.SetActive(true);
        StartCoroutine(HideSignUpErrorAfterSeconds(2f));
    }
   
    // 로그인, 회원가입 처리 
    public void OnLoginButtonClick()
    {
        string id = loginIdInputField.text;
        string pw = loginPwInputField.text;

        var result = SaveSystem.TryLogin(id,pw);
        if (result == SaveSystem.loginResult.Success)
        {
            ShowMainButtons();
        }
        else if (result == SaveSystem.loginResult.NoId)
        {
            ShowLoginError("아이디가 없습니다. 회원가입하세요.");
            StartCoroutine(SwitchToSignUpAfterSeconds(2f));
            
        }
        else
        {
            ShowLoginError("비밀번호가 다릅니다.");
        }
    }
    public void OnSignUpButtonClick()
    {
        string id = signUpIdInputField.text;
        string name = signUpNameInputField.text;
        string pw = signUpPwInputField.text;
        string pwConfirm = signUpPwConfirmInputField.text;
 	
	   signUpIdInputField.text = "";
  	   signUpNameInputField.text = "";
 	   signUpPwInputField.text = "";
 	   signUpPwConfirmInputField.text = "";



        if (pw != pwConfirm)
        {
            ShowSignUpError("비밀번호가 일치하지 않습니다.");
            return;
        }
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pw))
        {
            ShowSignUpError("모든 항목을 입력하세요.");
            return;
        }
        if (SaveSystem.TrySignUp(id, pw, name))
        {
			var allUsers = SaveSystem.LoadAllUsers();
 		    var newUser = allUsers.users.Find(u => u.ID == id);
	
 		    GameManager.Instance.userData = newUser; 
    		GameManager.Instance.Refresh();

            signInPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
        else
        {
            ShowSignUpError("이미 존재하는 아이디입니다.");
        }
    }
    
    // 코루틴
    private IEnumerator SwitchToSignUpAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ShowSignInPanel();
    }
    private IEnumerator HideLoginErrorAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        loginErrorPopup.SetActive(false);
    }
    private IEnumerator HideSignUpErrorAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        signUpErrorPopup.SetActive(false);
    }
    private IEnumerator DepositErrorPopupControl()
    {
        depositError.SetActive(true);
        yield return new WaitForSeconds(5);
        depositError.SetActive(false);
        depositErrorCoroutine = null;    
    
    }
    private IEnumerator WithdrawErrorPopupControl()
    {
        withdrawError.SetActive(true);
        yield return new WaitForSeconds(5);
        withdrawError.SetActive(false);
        withdrawErrorCoroutine = null;
    }
}