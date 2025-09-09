using System.Runtime.CompilerServices;
using UnityEngine;

public class Portal : M_Object
{
    UI_Base Base = null;

    //WokerSpawn
    [SerializeField] private Worker worker;
    [SerializeField] private Transform Waypoint;
    public override void Interaction(Character character)
    {
        base.Interaction(character);
        Base = Canvas_Handler.instance.GetUI("Portal");
        Base.Open();
        Base.GetComponent<UI_Portal>().Init(this);
    }

    public void SpawnWorker()
    {
        var go = Instantiate(worker, transform.position, Quaternion.identity);
        go.SetDestination(Waypoint.position, ()=>go.StateChange(WorkerState.Idle));
    }
}
