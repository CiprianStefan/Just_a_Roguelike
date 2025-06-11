using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileDirections
{
    public Vector3 direction;
    public Vector3 rotation;
}

public static class AbilitiesUtilities
{

    public static List<ProjectileDirections> GetDirectionsForFrontalProjectiles(float angleSpread = 5, float numberOfProjectiles = 1, PlayerDirection playerDirection = PlayerDirection.Up)
    {
        List<ProjectileDirections> directions = new List<ProjectileDirections>();

        Vector3 direction = playerDirection switch{
            PlayerDirection.Up => Vector3.up,
            PlayerDirection.Down => Vector3.down,
            PlayerDirection.Left => Vector3.left,
            PlayerDirection.Right => Vector3.right,
            _ => Vector3.zero
        };

        float angle = -angleSpread  * (numberOfProjectiles - 1);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            ProjectileDirections directionsInstance = new ProjectileDirections();
            Vector3 projectileDirection = Quaternion.Euler(0f, 0f, angle) * direction;
            directionsInstance.direction = projectileDirection;

            float rotationAngle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
            directionsInstance.rotation = new Vector3(0, 0, rotationAngle);

            angle += angleSpread * 2;
            directions.Add(directionsInstance);
        }
        return directions;
    }

    public static Tuple<ProjectileDirections, ProjectileDirections> GetDirectionsForSideProjectiles()
    {
        ProjectileDirections left = new ProjectileDirections();
        ProjectileDirections right = new ProjectileDirections();

        left.direction = Vector3.left;
        left.rotation = new Vector3(0, 0 , Mathf.Atan2(Vector3.left.y,Vector3.left.x) * Mathf.Rad2Deg);

        right.direction = Vector3.right;
        right.rotation = new Vector3(0, 0 , Mathf.Atan2(Vector3.right.y,Vector3.right.x) * Mathf.Rad2Deg);


        return new Tuple<ProjectileDirections, ProjectileDirections>(left,right);
    }

    public static Tuple<ProjectileDirections, ProjectileDirections> GetDirectionsForUpDownProjectiles()
    {
        ProjectileDirections up = new ProjectileDirections();
        ProjectileDirections down = new ProjectileDirections();

        up.direction = Vector3.up;
        up.rotation = new Vector3(0, 0 , Mathf.Atan2(Vector3.up.y,Vector3.up.x) * Mathf.Rad2Deg);

        down.direction = Vector3.down;
        down.rotation = new Vector3(0, 0 , Mathf.Atan2(Vector3.down.y,Vector3.down.x) * Mathf.Rad2Deg);


        return new Tuple<ProjectileDirections, ProjectileDirections>(up,down);
    }

    public static ProjectileDirections GetDirectionsForTargetedProjectiles(Vector3 origin, Vector3 target, float offset = 1)
    {
        Vector3 direction = (target - origin).normalized;
        return new ProjectileDirections
        {
            direction = direction * offset,
            rotation = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)
        };
    }

    public static List<ProjectileDirections> GetDirectionsForMultipleTargetedProjectiles(Vector3 origin, Vector3 target, float numberOfProjectiles = 1, float offset = 1)
    {
        List<ProjectileDirections> projectilesDirections = new List<ProjectileDirections>();
        if(numberOfProjectiles == 1)
        {
            projectilesDirections.Add(GetDirectionsForTargetedProjectiles(origin, target, offset));
            return projectilesDirections;
        }
        float angleStep = 5f;
        float angle = -Mathf.Min(angleStep * (numberOfProjectiles - 1), 90) / 2;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector3 projectileDirection = Quaternion.Euler(0f, 0f, angle) * (target - origin).normalized;
            ProjectileDirections projectileDirections = new ProjectileDirections
            {
                direction = projectileDirection * offset,
                rotation = new Vector3(0, 0, Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg)
            };
            projectilesDirections.Add(projectileDirections);
            angle += angleStep;
        }
        return projectilesDirections;
    }

    public static List<ProjectileDirections> GetDirectionsForRandomProjectiles(float numberOfProjectiles = 1)
    {
        List<ProjectileDirections> projectilesDirections = new List<ProjectileDirections>();
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 base_direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            ProjectileDirections projectileDirections = new ProjectileDirections
            {
                direction = base_direction,
                rotation = new Vector3(0, 0, Mathf.Atan2(base_direction.y, base_direction.x) * Mathf.Rad2Deg)
            };
            projectilesDirections.Add(projectileDirections);
        }
        return projectilesDirections;
    }

    public static List<Vector3> GetOrbitalProjectilesSpawnLocation(Vector3 origin,float numberOfProjectiles = 1, float distanceFromOrigin = 1)
    {
        List<Vector3> spawnLocations = new List<Vector3>();
        float angleStep = 360f / numberOfProjectiles;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            spawnLocations.Add(origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * distanceFromOrigin);
        }
        return spawnLocations;
    }

    public static List<ProjectileDirections> GetOrbitalProjectilesSpawnDirections(float numberOfProjectiles = 1, float distanceFromOrigin = 1)
    {
        List<ProjectileDirections> projectilesDirections = new List<ProjectileDirections>();
        float angleStep = 360f / numberOfProjectiles;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 base_direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            ProjectileDirections projectileDirections = new ProjectileDirections
            {
                direction = base_direction * distanceFromOrigin,
                rotation = new Vector3(0, 0, Mathf.Atan2(base_direction.y, base_direction.x) * Mathf.Rad2Deg)
            };
            projectilesDirections.Add(projectileDirections);
        }
        return projectilesDirections;
    }

    public static Vector3 UpdateOrbitalProjectilePosition(Vector3 origin, Vector3 projectilePosition, float projectileSpeed = 1, float distanceFromOrigin = 1, int rotationDirection = 1)
    {
        Vector3 offset = projectilePosition - origin;
        float angleModifier = Mathf.Atan2(offset.y, offset.x);
        angleModifier += 2 * projectileSpeed * Time.deltaTime * rotationDirection;
        Vector3 circleCalculation = new Vector3(Mathf.Cos(angleModifier), Mathf.Sin(angleModifier)); 
        return origin + circleCalculation * distanceFromOrigin;
    }

    public static void DestroyAbilitiesInstance(GameObject abilityInstance)
    {
        GameObject.Destroy(abilityInstance);
    }

    public static void DestroyAbilitiesInstances(List<GameObject> abilityInstances)
    {
        foreach(GameObject abilityInstance in abilityInstances)
            GameObject.Destroy(abilityInstance);
    }

    public static void ManageDuration(GameObject abilityInstance, ref float remaningDuration)
    {
        remaningDuration -= Time.deltaTime;
        if(remaningDuration <= 0)
            abilityInstance.SetActive(false);
    }
}
