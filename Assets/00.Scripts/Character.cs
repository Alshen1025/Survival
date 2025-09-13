using UnityEngine;

public class Character : MonoBehaviour
{

    public int Hp;
    public int MaxHp;

    public bool MainPlayer = false;

    //Animation
    protected Animator animator;

    //Equipment
    [SerializeField] protected GameObject[] Equipments;

    //interaction && Hit
    public M_Object m_Object = null;
    public Collider[] colliders;
    [SerializeField] protected GameObject HitParticle;
    [SerializeField] protected Transform ParitcleTransform;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        Hp = MaxHp;
    }

    public virtual void Hit()
    {
        if (m_Object == null) return;
       
        m_Object.HP -= 50;
        SpawnHitParitcle();
        m_Object.OnHit(this);
    }

    public virtual void Attack()
    {
        SpawnHitParitcle();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Monster>().GetDamage(50);
        }
    }

    public void SpawnHitParitcle()
    {
        Vector3 pos = new Vector3(ParitcleTransform.position.x + Random.Range(-0.5f, 0.5f), ParitcleTransform.position.y , ParitcleTransform.position.z + Random.Range(-0.5f, 0.5f));
        Instantiate(HitParticle, pos, Quaternion.identity);
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
