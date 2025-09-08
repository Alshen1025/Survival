//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Canvas_Handler : MonoBehaviour 
{
    public static Canvas_Handler instance = null;
    void Awake()
    {
        if (instance == null) instance = this;
    }

    private Dictionary<string, UI_Base> uiBases = new Dictionary<string, UI_Base>();
    public static Queue<UI_Base> Uis = new Queue<UI_Base>();
     
    public UI_Base GetUI(string name)
    {
        if(uiBases.ContainsKey(name))
        {
            return uiBases[name];
        }

        var uiBase = Instantiate(Resources.Load<UI_Base>("UI/" + name), transform);
        uiBases.Add(name, uiBase);
        uiBase.gameObject.SetActive(false);
        return uiBase;

    }


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
            Player_Movement.instance.ReturnCharacterMove();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            uiBases["BUILDING"].Toggle();
            Player_Movement.instance.ReturnCharacterMove();
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

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
