using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MarkerInfo
{
    public Transform targetTransform;
    public RectTransform markerUI;
    public string key;
    public Image Icon;
    public TextMeshProUGUI markerText;

    public MarkerInfo(Transform target, RectTransform ui, string m_key)
    {
        targetTransform = target;
        markerUI = ui;
        key = m_key;
        Icon = markerUI.Find("Icon").GetComponent<Image>();
        markerText = markerUI.Find("Distance").GetComponent<TextMeshProUGUI>();

        Icon.sprite = AssetManager.GetAtlas(key);

    }
}
public class UI_CompassBar : MonoBehaviour
{
    public static UI_CompassBar Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    private Transform playerTransform;
    public TextMeshProUGUI west, north, east, south;

    public float compassWidth = 700.0f;

    [Header("##Settings")]
    public float maxAlpha;
    public float minAlpha;
    public float maxScale;
    public float minScale;
    public float maxDistance;

    [Header("##OtherObject")]
    public static GameObject markerPrefab;
    public static Transform markerParent;
    public static List<MarkerInfo> activeMarkers = new List<MarkerInfo>();

    private void Start()
    {
        playerTransform = Player_Movement.instance.transform;
        markerPrefab = transform.Find("CompassMarker").gameObject;
        markerPrefab.SetActive(false);
        markerParent = transform.Find("Mask").transform;
    }

    private void Update()
    {
        UpdateCompass();
        UpdateMarkers();
    }

    private void UpdateCompass()
    {
        //player가 바라보는 각도로 방위 계산
        float heading = playerTransform.eulerAngles.y;

        SetPosition(west, heading, 90.0f);
        SetPosition(north, heading, 180.0f);
        SetPosition(east, heading, 270.0f);
        SetPosition(south, heading, 0.0f);
    }

    private void SetPosition(TextMeshProUGUI text, float heading, float offset)
    {
        //텍스트가 중앙 기준으로 이동하게 계산
        float relativeAngle = (heading - offset + 360.0f) % 360.0f; //각도 보정
        float normalizedAngle = relativeAngle / 360.0f; //0~1 사이로 정규화

        float positionX = Mathf.Lerp(-compassWidth, compassWidth, normalizedAngle);
        text.rectTransform.anchoredPosition = new Vector2(positionX, text.rectTransform.anchoredPosition.y);

        //거리계산( 중앙 = 0, 최대거리 = 1)
        float distanceFromCenter = Mathf.Abs(positionX/compassWidth);
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, distanceFromCenter);
        float scale = Mathf.Lerp(maxScale, minScale, distanceFromCenter);

        Color color = text.color;
        color.a = alpha;
        text.color = color;
        text.rectTransform.localScale = Vector3.one * scale;

    }

    //오브젝트 거리 표시
    public static void AddMarker(Transform targetTransform, string key)
    {
        if (activeMarkers.Exists(m => m.targetTransform == targetTransform))
            return;
        GameObject marker = Instantiate(markerPrefab, markerParent);
        marker.SetActive(true);
        marker.name = "Marker:" + targetTransform.name;
        RectTransform markerRect = marker.GetComponent<RectTransform>();
        activeMarkers.Add(new MarkerInfo(targetTransform, markerRect, key));
    }

    public void UpdateMarkers()
    {
        for (int i = activeMarkers.Count -1; i>= 0; i--)
        {
            MarkerInfo marker = activeMarkers[i];
            if(marker.targetTransform == null)
            {
                Destroy(marker.markerUI.gameObject);
                activeMarkers.RemoveAt(i);
                continue;
            }

            float heading = playerTransform.eulerAngles.y;
            Vector3 DirectionToTarget = marker.targetTransform.position - playerTransform.position;
            float distance = Vector3.Distance(marker.targetTransform.position, playerTransform.position);

            //각도 생성
            float targetAngle = Mathf.Atan2(-DirectionToTarget.x , -DirectionToTarget.z) * Mathf.Rad2Deg;

            float relativeAngle = (heading - targetAngle + 360f) % 360f;
            float normalizedAngle = relativeAngle / 360f;
            float PositionX = Mathf.Lerp(-compassWidth, compassWidth, normalizedAngle);

            bool isActiveMarker = distance <= maxDistance ? false : true;

            marker.markerUI.gameObject.SetActive(isActiveMarker);
            marker.markerUI.anchoredPosition = new Vector2(PositionX, marker.markerUI.anchoredPosition.y);
            marker.markerText.text = string.Format("{0:0.0} m", distance);
        }
    }
}
