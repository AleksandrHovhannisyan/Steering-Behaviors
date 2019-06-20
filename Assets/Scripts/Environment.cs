using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Convenience script so users don't have to expand
 * the Environment prefab and manually toggle the active
 * status of the Walls child. Note: ExecutiveInEditMode
 * allows the changes to be visible not only when the
 * game runs but also when you're in the scene view.
 */
[ExecuteInEditMode]
public class Environment : MonoBehaviour
{
    [SerializeField] private GameObject walls;
    public bool wallsEnabled = true;

    /* Runs every frame. Performance penalty is negligible.
     */  
    private void Update()
    {
        if(walls)
        {
            walls.SetActive(wallsEnabled);
        }
    }
}
