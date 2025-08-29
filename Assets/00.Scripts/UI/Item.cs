using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    //퍼지는 반경, 포물선 높이, 아이템 이동 속도
    [SerializeField] private float spreadRadius = 10.0f;
    [SerializeField] private float arcHeight = 5.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    Transform player;
    private ITEM m_Item;
    

    public void Init(ITEM item)
    {
        m_Item = item;
    }

    private void Start()
    {
        player = Player_Movement.instance.transform;
        StartCoroutine(Spread());
    }

    IEnumerator Spread()
    {
        Vector3 spreadDirection = Random.insideUnitCircle * spreadRadius;
        Vector3 spreadPostion = transform.position + spreadDirection;

        if(spreadPostion.y <= 1.0f)
        {
            spreadPostion.y = Mathf.Max(spreadPostion.y, 1.0f);
        }

        float spreadTime = 0.3f;
        float elapsedTime = 0.0f;

        Vector3 startPosition = transform.position;

        while (elapsedTime < spreadTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spreadTime;
            transform.position = Vector3.Lerp(startPosition, spreadPostion, t);
            yield return null;  //1프레임 대기
        }
        StartCoroutine(MoveToPlayer(spreadPostion));
    }

    IEnumerator MoveToPlayer(Vector3 startPosition)
    {
        float journeyTime;
        float elapsedTime;

        Vector3 endPosition;

        while(true)
        {
            endPosition = player.position;
            journeyTime = Vector3.Distance(startPosition, endPosition) / moveSpeed;
            elapsedTime = 0.0f;
            while (elapsedTime < journeyTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / journeyTime;
                Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);

                transform.position = currentPos;
                endPosition = player.position;

                yield return null;
            }
            if(Vector3.Distance(transform.position, player.position) < 0.5f) break;

            //시작 위치 갱신
            startPosition = transform.position;
        }

        Navigation_Manager.Instance.CreateItemPanel(m_Item.Data, m_Item.Count);
        ItemDrop_Manager.GetITEM(m_Item.Data, m_Item.Count);
        Destroy(this.gameObject);
        
    }
}
