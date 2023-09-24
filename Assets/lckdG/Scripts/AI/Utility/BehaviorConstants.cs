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
        Float,
        Int,
        String,
        Bool,
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
