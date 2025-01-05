using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variables to track fuel and repair status
    public static float fuel = 0f;
    public static float repair = 0f;
    public static bool Engine = false;

    // Method to increment the fuel level, up to a maximum of 3
    public static void IncrementFuel()
    {
        if (fuel < 3f) { fuel++; }
    }

    // Method to decrement the fuel level
    public static void DecrementFuel()
    {
        fuel--;
    }

    public static void submitEngine(){
        Engine = true;
    }

    // Method to set the repair status to fully repaired (100)
    public static void RepairedShip()
    {
        repair = 100;
    }

    // Method to set the repair status to broken (0)
    public static void BreakShip()
    {
        repair = 0;
    }

    // Method to get the current repair status
    public static float getRepair()
    {
        return repair;
    }

    // Method to get the current fuel level
    public static float getFuel()
    {
        return fuel;
    }
}
