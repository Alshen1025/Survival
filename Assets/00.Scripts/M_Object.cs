using UnityEngine;

public class M_Object : MonoBehaviour
{
    public Object_Scriptable m_Data;
    public bool GetInteraction = false;
    public int HP;


    private void Start()
    {
        Delegate_Handler.OutInteraction += StopInteraction;
    }

    private void OnDestroy()
    {
        Delegate_Handler.OutInteraction -= StopInteraction;
    }

    public virtual void StopInteraction()
    {

    }

    public virtual void Interaction()
    {
        Player_Handler.m_Object = this;
        GetInteraction = true;
    }

    public virtual void OnHit()
    {
        Canvas_Handler.instance.GetBoard();
        ManagerBase.instance.gameManager.SetStamina(-10);
        HP_Init();
    }

    public virtual void HP_Init()
    {
        if (m_Data == null)
            Debug.LogError("m_Data가 null입니다: " + gameObject.name);
        if (Canvas_Handler.instance == null)
            Debug.LogError("Canvas_Handler.instance가 null입니다");

        if (HP<=0)
        {
            HP = 0;
            Particle_Handler.Instance.OnParticle(transform.GetChild(0).GetComponent<MeshRenderer>());
            Canvas_Handler.instance.BoardHpWhiteFill.fillAmount = 1.0f;
            Canvas_Handler.instance.AllStopCoroutine();
            Destroy(this.gameObject);
            Delegate_Handler.OnEndInteraction();
            return;
        }
        
        Canvas_Handler.instance.BoardFill(HP, m_Data.HP);
    }
}
