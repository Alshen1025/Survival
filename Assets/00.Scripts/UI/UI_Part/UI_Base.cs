using UnityEngine;

public class UI_Base : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning("UI is not active");
            return;
        }
        if(GetComponent<Animator>()  != null)
        {
            GetComponent<Animator>().SetTrigger("Out");
            return;
        }
        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if(IsActive) Close();
        else Open();
    }


}
