using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DestructibleHealthTextDisplay : MonoBehaviour
{
    private TextMeshPro Text;
    [SerializeField]
    private DestructibleObject Destructible;

    private void Awake()
    {
        Text = GetComponent<TextMeshPro>();
        Destructible.OnTakeDamage += HandleDamage;
        Destructible.OnDestroy += OnDestroyed;
        Text.SetText($"{Destructible.Health}");
    }

    private void HandleDamage(int Damage, int Health)
    {
        Text.SetText($"{Health}");
    }

    private void OnDestroyed()
    {
        Destroy(gameObject);
    }
}
