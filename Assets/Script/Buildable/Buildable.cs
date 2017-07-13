using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : MonoBehaviour {

    /* The Renderer and Materials should be loaded through Resources.Load(). Using inspector as placeholder.

    // The mesh used to render this buildable
    public Renderer Render;
    // This material signals the user this board location is OKAY to build on
    public Material Allowed;
    // This material signals the user this board location is NOT OKAY to build on
    public Material NotAllowed;

    */

    // Returns true if the object is alive, false otherwise
    protected bool Alive;
    // Returns true if the object is visible, false otherwise
    protected bool Visible;
    // Returns the current health of the object
    protected int Health;
    // Returns the current defense of the object
    protected int Defense;
    // Returns the objects interaction with movement, LOS, and ranged attacks
    // -1   No interaction
    // 0    Blocks movement
    // 1    Blocks movement, LOS, and ranged attacks
    protected int Obstacle;

    // Applies the current defense of the object to an amount of damage.
    public virtual void ApplyDefense(int damage)
    {
        Debug.Log("Applying Buildable Defense method.");
    }
}
