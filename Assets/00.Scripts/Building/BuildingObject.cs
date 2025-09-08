using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public enum MaterialType
{
    Opaque,
    Transparent
}


public class BuildingObject : MonoBehaviour
{
    //Data
    public Building_Scriptable Data;

    //Particle
    [SerializeField] private ParticleSystem particle;

    //Material
    Renderer renderer;
    Collider collider;
    private Material M_Opaque, M_Transparent;
    public Material OriginalMaterial;
    private Color[] colors = { new Color(0.0f, 0.02415333f, 0.7490197f, 1.0f), new Color(1.0f, 0.2688679f, 0.2688679f, 1.0f) };

    //bool
    public bool CanBuild = true;
    bool buildCompleted = false;

    //BoardUI
    public GameObject Board;
    [SerializeField] private Image Icon;
    [SerializeField] private Image Slider;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Percent;

    //portal
    public GameObject PortalQuad;

    //Worker
    public bool Working = false;


    private void Awake()
    {
        M_Opaque = Resources.Load<Material>("M_Opaque");
        M_Transparent = Resources.Load<Material>("M_Transparent");
        renderer = GetComponentInChildren<Renderer>();
        collider = GetComponentInChildren<Collider>();
    }

    public void Comfirm()
    {
        Debug.Log("Comfirm");
        particle.Play();
        Camera mainCamera = Camera.main;
        Transform parent = Board.transform.parent;

        parent.eulerAngles = new Vector3(55.0f, parent.eulerAngles.y - transform.eulerAngles.y, 0.0f);

        Board.SetActive(true);
        Icon.sprite = AssetManager.GetAtlas(Data.Key);
        Name.text = Data.Key;
        SetBuildingData(Data.Time, BuildCompleted);
    }

    public void SetMakeData(string key, float timer, Action action = null)
    {
        Board.SetActive(true);
        Icon.sprite =  AssetManager.GetAtlas(key);
        Name.text = key;
        collider.gameObject.layer = LayerMask.NameToLayer("WorkObject");
        SetBuildingData(timer, action);
    }

    public void SetBuildingData(float time, Action action)
    {
        StartCoroutine(SliderFillCoroutine(time, action));
    }

    private void BuildCompleted()
    {
        SetMaterial(MaterialType.Opaque);
        Board.GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(CompltedCoroutine());
        PortalQuad.SetActive(true);
    }

    private IEnumerator CompltedCoroutine()
    {
        Debug.Log("CompltedCoroutine");
        float current = 0.0f;
        float percent = 0.0f;
        float EmissionStart = 1.0f;
        float EmissionEnd = 20.0f;
        Color startColor = Color.white;
        Color endColor = Color.black;
        while(percent< 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            float LerpEmission = Mathf.Lerp(EmissionStart, EmissionEnd, percent);
            renderer.material.SetColor("_EmissionColor", startColor * LerpEmission);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        current = 0.0f;
        percent = 0.0f;

        while(percent< 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            float LerpEmission = Mathf.Lerp(EmissionEnd, EmissionStart, percent);
            Color LerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor *LerpEmission);
            yield return null;
        }
        if (OriginalMaterial != null)
        {
            renderer.material = OriginalMaterial;
        }
        buildCompleted = true;
        collider.gameObject.layer = LayerMask.NameToLayer("Object");
        
    }

    IEnumerator SliderFillCoroutine(float time, Action action)
    {
        float t = 0.0f;
        while(t <= time)
        {
            t += Time.deltaTime;
            Slider.fillAmount = t/time;
            Percent.text = string.Format("{0:0.0}%", Slider.fillAmount * 100.0f);
            yield return null;  
        }
        if(action != null)
        {
            Board.GetComponent<Animator>().SetTrigger("Out");
            action?.Invoke();
        }
        
    }

    public void SetTrigger(bool active)
    {
        collider.isTrigger = active;
    }



    public void SetMaterial(MaterialType type)
    {
        switch(type)
        {
            case MaterialType.Opaque: renderer.material = M_Opaque; break;
            case MaterialType.Transparent: renderer.material = M_Transparent; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name != "Terrain")
        {
            SetMaterialColor(1);
            CanBuild = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name != "Terrain")
        {
            SetMaterialColor(0);
            CanBuild = true;
        }
    }


    public void SetMaterialColor(int value)
    {
        renderer.material.SetColor("_EmissionColor", colors[value]);
    }
}
