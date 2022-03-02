using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyDestructible : MonoBehaviour
{
    [SerializeField]
    private int DestructibleObjectCheckRate = 10;
    [SerializeField]
    private float CheckDistance = 1f;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private float DestructibleAttackDelay = 1f;
    [SerializeField]
    private int DestructibleAttackDamage = 10;
    [SerializeField]
    private LayerMask DestructibleLayers;

    private Enemy Enemy;
    private NavMeshPath OriginalPath;

    private Coroutine CheckCoroutine;
    private Coroutine AttackCoroutine;

    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        Enemy.OnStateChange += HandleStateChange;
    }

    private void HandleStateChange(EnemyState OldState, EnemyState NewState)
    {
        if (NewState == EnemyState.Chase)
        {
            if (CheckCoroutine != null)
            {
                StopCoroutine(CheckCoroutine);
            }
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }
            CheckCoroutine = StartCoroutine(CheckForDestructibleObjects());
        }
    }

    private IEnumerator CheckForDestructibleObjects()
    {
        yield return null;
        WaitForSeconds Wait = new WaitForSeconds(1f / DestructibleObjectCheckRate);
        Vector3[] corners = new Vector3[2];

        bool foundDestructible = false;
        while (!foundDestructible)
        {
            int length = Agent.path.GetCornersNonAlloc(corners);
            if (length > 1 &&
                Physics.Raycast(
                    corners[0],
                    (corners[1] - corners[0]).normalized,
                    out RaycastHit hit,
                    CheckDistance,
                    DestructibleLayers) &&
                    hit.collider.TryGetComponent(out DestructibleObject destructible)
                )
            {
                destructible.OnDestroy += HandleDestroy;
                OriginalPath = Agent.path;
                Agent.enabled = false;
                Enemy.ChangeState(EnemyState.Destroy);
                
                StopCoroutine(CheckCoroutine);
                AttackCoroutine = StartCoroutine(AttackDestructible(destructible));
                
                foundDestructible = true;
                break;
            }

            yield return Wait;
        }
    }

    private void HandleDestroy()
    {
        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
        }

        if (Enemy.State == EnemyState.Destroy)
        {
            Agent.enabled = true;
            Agent.SetPath(OriginalPath);
            Enemy.ChangeState(EnemyState.Chase);
        }
    }

    private IEnumerator AttackDestructible(DestructibleObject Destructible)
    {
        WaitForSeconds Wait = new WaitForSeconds(DestructibleAttackDelay);
        while (Destructible != null)
        {
            Destructible.TakeDamage(DestructibleAttackDamage);

            yield return Wait;
        }
    }
}
