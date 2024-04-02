namespace SwordNShield.Combat.States
{
    public interface IState
    {
        StateType Type { get; }
        void SetState(float rate, float time);
    }
}