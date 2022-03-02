using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ParticleSystem DestroySystem;

    public int Health;

    public delegate void DestryEvent();
    public DestryEvent OnDestroy;

    public delegate void TakeDamageEvent(int Damage, int Health);
    public TakeDamageEvent OnTakeDamage;

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            Health = 0;
            OnTakeDamage?.Invoke(Damage, Health);
            if (DestroySystem != null)
            {
                DestroySystem.gameObject.SetActive(true);
                DestroySystem.transform.SetParent(null, true);
                DestroySystem.Play();
            }
            OnDestroy?.Invoke();

            gameObject.SetActive(false);
            
            NavMeshManager.Instance.BakeNavMesh();

            Destroy(gameObject);
        }
        else
        {
            OnTakeDamage?.Invoke(Damage, Health);
        }
    }
}
