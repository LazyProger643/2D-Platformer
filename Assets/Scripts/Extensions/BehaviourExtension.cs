
namespace UnityEngine
{
    public static class BehaviourExtension
    {
        public static bool HasComponent<T>(this Behaviour behaviour) where T : Component
        {
            return behaviour.GetComponent<T>() != null;
        }
    }
}
