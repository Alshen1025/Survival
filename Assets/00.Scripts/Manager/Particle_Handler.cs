using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public static Particle_Handler Instance = null;
    ParticleSystem mparticleSystem;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        mparticleSystem = GetComponent<ParticleSystem>();
    }

    public void OnParticle(MeshRenderer mesh)
    {
        transform.position = mesh.transform.position;
        UpdateParticleMesh(mesh);
        mparticleSystem.Play();
    }

    private void UpdateParticleMesh(MeshRenderer meshRenderer)
    {
        var shape = mparticleSystem.shape;
        shape.meshRenderer = meshRenderer;
    }
}
