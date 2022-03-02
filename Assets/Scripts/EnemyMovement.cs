using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent), typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Camera Camera = null;
    [SerializeField]
    private LayerMask LayerMask;
    private Enemy Enemy;
    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Ray ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask))
            {
                Agent.enabled = true;
                Agent.SetDestination(hit.point);
                Enemy.ChangeState(EnemyState.Chase);
            }
        }

        if (Agent.enabled && Agent.remainingDistance < Agent.radius)
        {
            Enemy.ChangeState(EnemyState.Idle);
        }
    }
}
