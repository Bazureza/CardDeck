using UnityEngine;

namespace GuraGames.Utility
{
    public static class MathExtend
    {

        public static Vector2 RadianToVector(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 AngleToVector(float degree)
        {
            return RadianToVector(degree * Mathf.Deg2Rad);
        }

        public static float VectorToAngle(Vector2 direction)
        {
            direction.Normalize();
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static float GetDegreeBetween(Vector3 posA, Vector3 posB)
        {
            float normalAngle = VectorToAngle(posA);
            if (normalAngle < 0) normalAngle += 360;
            float secondNormalAngle = MathExtend.VectorToAngle(posB);
            if (secondNormalAngle < 0) secondNormalAngle += 360;
            return (normalAngle + (secondNormalAngle - normalAngle) / 2f);
        }

        public static int RandomSign()
        {
            return Random.value < .5 ? -1 : 1;
        }

        public static bool IsAngleInLinear(float angle, int tolerance)
        {
            angle = Mathf.Abs(angle);
            return (angle >= 0 && angle <= 0 + tolerance) || (angle >= 90 - tolerance && angle <= 90 + tolerance) || (angle >= 180 - tolerance && angle <= 180 + tolerance);
        }
    }
}