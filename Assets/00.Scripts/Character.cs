using UnityEngine;

public class Character : MonoBehaviour
{
    public bool MainPlayer = false;

    //Animation
    protected Animator animator;

    //Equipment
    [SerializeField] protected GameObject[] Equipments;

    //interaction && Hit
    public M_Object m_Object = null;
    [SerializeField] protected GameObject HitParticle;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Hit()
    {
        if (m_Object == null) return;
       
        m_Object.HP -= 50;
        Vector3 pos = new Vector3(m_Object.transform.position.x + Random.Range(-0.5f, 0.5f), m_Object.transform.position.y + 1.5f, m_Object.transform.position.z + Random.Range(-0.5f, 0.5f));
        Instantiate(HitParticle, pos, Quaternion.identity);
        m_Object.OnHit(this);
    }

    public void DeactiveEquipment()
    {
        for (int i = 0; i < Equipments.Length; i++)
        {
            Equipments[i].SetActive(false);
        }
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    public void EquipmentChange(Object_Type type, bool Active)
    {
        Equipments[(int)type].SetActive(Active);
    }
}
