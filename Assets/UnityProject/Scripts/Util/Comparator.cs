namespace UnityProject.Scripts.Util
{
    public static class Comparator
    {
        public static bool Less(float value, float max) => value < max;
        public static bool Equals(int a, int b) => a == b;
        public static bool LessOrEquals(float value, float max) => value <= max;
    }
}