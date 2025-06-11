
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    public void OnClickToDeposit()
    {
        UIManager.Instance.ShowDepositPanel();
    }

    public void OnClickToWithdraw()
    {
        UIManager.Instance.ShowWithdrawPanel();
    }

    public void TurnToBack()
    {
        UIManager.Instance.ShowMainButtons();
    }

    public TMP_InputField transferNameInput;
    public TMP_InputField trabsferamountInput;

    public void OnTransferConfirmButtonClick()
    {
        string name = transferNameInput.text.Trim();
        int amount;

        if (string.IsNullOrWhiteSpace(name) || !int.TryParse(trabsferamountInput.text, out amount) || amount <= 0)
        {
            Debug.Log("송금 실패, 입력값 오류");
            return;
        }
        
        bool result = SaveSystem.TryTransfer(name, amount);
        if (!result)
        {
            Debug.Log("대상 없음 또는 잔액 부족");
        }
        else
        {
            Debug.Log("송금 완료");
        }
        
        transferNameInput.text = "";
        trabsferamountInput.text = "";
        
    }
    //금액의 계산과 정수만 받는걸 한번에 체크

    public void OnDepositConfirmButtonClick()
    {
        int amount;
        string input = UIManager.Instance.depositInputField.text;
        if (int.TryParse(input, out amount) && amount > 0)
        {
            if(!GameManager.Instance.TryDeposit(amount))
                UIManager.Instance.ShowDepositError();
        }
        else
        {
            UIManager.Instance.ShowDepositError();
        }
        UIManager.Instance.depositInputField.text = "";
    }

    public void OnWithdrawConfirmButtonClick()
    {
        int amount;
        string input = UIManager.Instance.withdrawInputField.text;
        if (int.TryParse(input, out amount) && amount > 0)
        {
            if(!GameManager.Instance.TryWithdraw(amount))
                UIManager.Instance.ShowWithdrawError();
        }
        else
        {
            UIManager.Instance.ShowWithdrawError();
        }
        UIManager.Instance.withdrawInputField.text = "";
    }
}
