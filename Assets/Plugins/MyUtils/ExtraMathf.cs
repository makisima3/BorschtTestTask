﻿using UnityEngine;

namespace Plugins.MyUtils
{
    public static class ExtraMathf
    {
        public static float GetSpeed(float distance, float time)
        {
            return distance / time;
        }

        public static float GetDistance(float speed, float time)
        {
            return speed * time;
        }

        public static float GetTime(float distance, float speed)
        {
            return distance / speed;
        }

        public static Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        public static Quaternion GetRotation(Vector3 from, Vector3 to, Vector3 axis)
        {
            var direction = GetDirection(from, to);
            return GetRotation(direction, axis);
        }

        public static Quaternion GetRotation(Vector3 direction, Vector3 axis)
        {
            //TODO: Make method universal for any direction and axis
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            return Quaternion.Euler(axis * angle);
        }
    }
}