using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyState _State;
    public EnemyState State
    {
        get
        {
            return _State;
        }
        private set
        {
            if (_State != value)
            {
                OnStateChange?.Invoke(_State, value);
            }
            _State = value;
        }
    }

    public delegate void StateChangeEvent(EnemyState OldState, EnemyState NewState);
    public StateChangeEvent OnStateChange;

    public void ChangeState(EnemyState NewState)
    {
        if (NewState != State)
        {
            State = NewState;
        }
    }
}
