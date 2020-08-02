using System;

namespace Core
{
    public static class Heuristic
    {
        public static float Manhattan(Node nodeA, Node nodeB)
        {
            var aPosition = nodeA.transform.position;
            var bPosition = nodeB.transform.position;
            return Math.Abs(aPosition.x - bPosition.x) + Math.Abs(aPosition.z - bPosition.z);
        }
    }
}
