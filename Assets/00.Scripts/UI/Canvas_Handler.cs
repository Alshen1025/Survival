//using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Canvas_Handler : MonoBehaviour 
{
    public static Canvas_Handler instance = null;
    void Awake()
    {
        if (instance == null) instance = this;
    }

    private Dictionary<string, UI_Base> uiBases = new Dictionary<string, UI_Base>();

    public void OpenUI(string uiName)
    {
        if (uiBases.ContainsKey(uiName))
        {
            uiBases[uiName].Open();
        }
        else Debug.LogWarning("UI not found");
    }

    public void CloseUI(string uiName)
    {
        if (uiBases.ContainsKey(uiName))
        {
            uiBases[uiName].Close();
        }
    }

    public void CloseAllUI()
    {
        foreach (var bases in uiBases.Values )
        {
            bases.Close();
        }
    }

    [SerializeField] private Transform UIParent;
    [SerializeField] private GameObject Board;
    public Image BoardHpFill, BoardHpWhiteFill;
    Coroutine F_Coroutine;

    //Profile
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private Image StaminaSlider;
    

    private void Start()
    {
        UI_Base[] bases = UIParent.GetComponentsInChildren<UI_Base>(true);
        foreach (var part in bases)
        {
            uiBases.Add(part.name, part);
        }
        //Delegate_Handler.OnInteraction += GetBoard;
        Delegate_Handler.OutInteraction += BoardOut;
        Delegate_Handler.OnStamina += StaminaCheck;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiBases["Inventory"].Toggle();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            uiBases["BUILDING"].Toggle();
        }
    }

    public void GetText(string temp, Color color)
    {
        Vector3 posReal = Player_Movement.instance.transform.position;
        posReal.y += 0.5f;
        posReal.x += Random.Range(-0.5f, 0.5f);
        posReal.z += Random.Range(-0.5f, 0.5f);


        var go = Instantiate(Resources.Load<GameObject>("TextObject"), posReal, Quaternion.Euler(55, 0, 0));
        TextMeshPro textObjo = go.GetComponent<TextMeshPro>();
        textObjo.color = color;
        textObjo.text = temp;  

    }


    private void StaminaCheck(int value)
    {
        StaminaText.text = ManagerBase.instance.gameManager.Stamina + "/" + ManagerBase.instance.gameManager.MaxStamina;
        StaminaSlider.fillAmount = ManagerBase.instance.gameManager.Stamina / (float) ManagerBase.instance.gameManager.MaxStamina;
    }

    public void GetBoard()
    {
        Board.SetActive(true);
    }

    public void BoardOut() => Board.GetComponent<UI_AnimationHandler>().AnimationChange("Out");

    public void AllStopCoroutine() => StopAllCoroutines();

    public void BoardFill(float hp, float maxhp)
    {
        BoardHpFill.fillAmount = hp / maxhp;
        if (F_Coroutine != null)
        {
            StopCoroutine(F_Coroutine);
        }
        F_Coroutine = StartCoroutine(FillCoroutine());
    }

    IEnumerator FillCoroutine()
    {
        while (BoardHpWhiteFill.fillAmount > BoardHpFill.fillAmount)
        {
            BoardHpWhiteFill.fillAmount = Mathf.Lerp(BoardHpWhiteFill.fillAmount,
                BoardHpFill.fillAmount, Time.deltaTime * 2.0f);
            yield return null;
        }
        
    }
}
