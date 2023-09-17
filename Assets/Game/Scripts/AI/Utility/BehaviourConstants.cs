namespace AI.Tree
{
    public static class BehaviourConstants
    {
        
    }

    public static class BlackboardKey
    {
        // Any blackboard key goes here

    }

    public enum State
    {
        EXECUTING,
        SUCCESS,
        FAILED
    }

    public enum BlackboardObjectType
    {
        NavMeshAgent,
        Float,
        Int,
        String,
        Bool,
        True,
        False,
        Vector2,
        Vector3,
        Object,
    }

    internal enum FinishMode
    {
        IMMEDIATE,
        DELAYED
    }
}
