using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIDebugger : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때만 실행
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 마우스 위치에 있는 모든 UI 요소를 가져옴
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                Debug.Log("--- 마우스 아래 UI 요소들 ---");
                foreach (var result in results)
                {
                    Debug.Log("감지된 오브젝트: " + result.gameObject.name, result.gameObject);
                }
                Debug.Log("--------------------------");
            }
        }
    }
}
  