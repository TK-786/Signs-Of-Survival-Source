using System.Collections;
using UnityEngine;

public static class KnockbackUtility
{
    public static IEnumerator ApplyKnockback(GameObject target, Vector3 direction, float force, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float knockbackStep = force * (1 - (elapsedTime / duration));
            Vector3 movement = direction * knockbackStep * Time.deltaTime;

            target.transform.position += movement;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Knockback movement completed.");
    }
}
