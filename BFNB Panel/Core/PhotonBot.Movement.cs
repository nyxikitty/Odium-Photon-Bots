using System;

namespace OdiumPhoton.Core
{
    public partial class PhotonBot
    {
        public void SetMovement(MovementMode mode, int actor = -1, float offset = 0f)
        {
            currentMode = mode;
            targetActor = actor;
            orbitOffset = offset;

            if (mode != MovementMode.Idle)
                isActive = true;
            else
                isActive = false;
        }

        public void SetOrbitRadius(float radius)
        {
            orbitRadius = radius;
        }

        public void SetOrbitSpeed(float speed)
        {
            orbitSpeed = speed;
        }

        public void StopMovement()
        {
            currentMode = MovementMode.Idle;
            isActive = false;
        }

        private Vec3 CalculateNextPosition()
        {
            if (currentMode == MovementMode.Follow)
            {
                return GetFollowPosition();
            }
            else if (currentMode == MovementMode.Orbit)
            {
                return GetOrbitPosition();
            }
            else
            {
                return GetCirclePosition();
            }
        }

        private Vec3 GetFollowPosition()
        {
            Vec3 targetPos;

            lock (posLock)
            {
                if (!playerPositions.ContainsKey(targetActor))
                    return GetCirclePosition();

                targetPos = playerPositions[targetActor];
            }

            Vec3 currentPos = GetCirclePosition();
            float dist = GetDistance(currentPos, targetPos);

            if (dist < 2f)
                return currentPos;

            Vec3 dir = NormalizeVector(targetPos - currentPos);
            float moveAmount = FOLLOW_SPEED * dist;

            return new Vec3(
                currentPos.x + dir.x * moveAmount,
                currentPos.y + dir.y * moveAmount,
                currentPos.z + dir.z * moveAmount
            );
        }

        private Vec3 GetOrbitPosition()
        {
            Vec3 center;

            lock (posLock)
            {
                if (!playerPositions.ContainsKey(targetActor))
                    return GetCirclePosition();

                center = playerPositions[targetActor];
            }

            rotAngle = rotAngle + orbitSpeed;
            float ang = rotAngle + orbitOffset;

            float x = center.x + ((float)Math.Cos(ang) * orbitRadius);
            float y = center.y;
            float z = center.z + ((float)Math.Sin(ang) * orbitRadius);

            return new Vec3(x, y, z);
        }

        private Vec3 GetCirclePosition()
        {
            rotAngle += IDLE_SPEED;

            float cx = (float)Math.Cos(rotAngle) * 5f;
            float cy = DEFAULT_HEIGHT;
            float cz = (float)Math.Sin(rotAngle) * 5f;

            return new Vec3(cx, cy, cz);
        }

        private static Vec3 NormalizeVector(Vec3 vector)
        {
            float magnitude = (float)Math.Sqrt(
                vector.x * vector.x +
                vector.y * vector.y +
                vector.z * vector.z
            );

            if (magnitude > 0)
            {
                return new Vec3(
                    vector.x / magnitude,
                    vector.y / magnitude,
                    vector.z / magnitude
                );
            }

            return vector;
        }

        private static float GetDistance(Vec3 from, Vec3 to)
        {
            float deltaX = to.x - from.x;
            float deltaY = to.y - from.y;
            float deltaZ = to.z - from.z;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }
}