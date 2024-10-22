using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float fuel = 0f;
    public static float repair = 0f;

    public static void IncrementFuel()
    {
        if(fuel < 3f) { fuel++; }
    }

    public static void DecrementFuel()
    {
        fuel--;
    }

    public static void RepairedShip()
    {
        repair = 100;
    }

    public static void BreakShip()
    {
        repair = 0;
    }

    public static float getRepair()
    {
        return repair;
    }

    public static float getFuel()
    {
        return fuel;
    }
}

