using UnityEngine;

public class Player_Handler : MonoBehaviour
{
    public static M_Object m_Object = null;
    [SerializeField] private GameObject HitParticle;
    


    public void Hit()
    {
        m_Object.HP-= 50;
        Vector3 pos = new Vector3(m_Object.transform.position.x + Random.Range(-0.5f, 0.5f), m_Object.transform.position.y + 1.5f, m_Object.transform.position.z+ Random.Range(-0.5f,0.5f));
        Instantiate(HitParticle, pos, Quaternion.identity) ;
        m_Object.OnHit();
    }
}
