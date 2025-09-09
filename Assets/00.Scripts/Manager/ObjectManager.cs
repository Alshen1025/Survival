using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ObjectManager : MonoBehaviour
{
    private CullingGroup cullingGroup;
    private BoundingSphere[] boundingSpheres;
    private List<GameObject> SetObjects = new List<GameObject>();

    public float cullingGroupRadius = 5.0f;
    public float spawnAngle = 80.0f;
    public float CenterLimit = 5.0f;
    public int Maximum = 60;
   public Object_Scriptable[] Datas;

    private void Start()
    {

        Datas = Resources.LoadAll<Object_Scriptable>("Object");
        GetSpawnObject();

       
    }

    private void SetCulling()
    {
        boundingSpheres = new BoundingSphere[SetObjects.Count];

        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = Camera.main;
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(SetObjects.Count);

        for (int i = 0; i < SetObjects.Count; i++)
        {
            boundingSpheres[i] = new BoundingSphere(SetObjects[i].transform.position, cullingGroupRadius);
        }
        cullingGroup.onStateChanged += OnStateChanged;
    }

    public void RemoveObjectFromCullingGroup(GameObject obj)
    {
        int index = SetObjects.IndexOf(obj);
        SetObjects.RemoveAt(index);

        List<BoundingSphere> newSpheres = new List<BoundingSphere>(boundingSpheres);
        newSpheres.RemoveAt(index);
        boundingSpheres = newSpheres.ToArray();
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(boundingSpheres.Length);
    }

    public void OnDestroy()
    {
        if(cullingGroup != null)
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }
    }

    private void OnStateChanged(CullingGroupEvent evt)
    {
        if(evt.isVisible)
        {
            SetObjects[evt.index].SetActive(true);
        }
        else
        {
            SetObjects[evt.index].SetActive(false);
        }
    }


    public void GetSpawnObject()
    {
        StartCoroutine(CreateObjectStart());
    }

    IEnumerator CreateObjectStart()
    {
        for (int i = 0; i < Maximum; i++)
        {
            Vector3 pos;
            MakePos(out pos);
            while (Vector3.Distance(pos, Vector3.zero) <= CenterLimit)
            {
                MakePos(out pos);
            }
            var Getobject = Datas[Random.Range(0, Datas.Length - 1)].obj;
            Vector3 objPos = new Vector3(pos.x, Getobject.transform.position.y, pos.z);
            Quaternion objRotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
            var go = Instantiate(Getobject, objPos, objRotation);
            go.gameObject.SetActive(false);
            SetObjects.Add(go);
            yield return null;
        }
        SetCulling();
    }

    public void MakePos(out Vector3 pos)
    {
        pos = Vector3.zero + Random.insideUnitSphere * spawnAngle;
        pos.y = 0.0f;
    }
}
