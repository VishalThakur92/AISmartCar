using UnityEngine;
using System.Collections;
using TrackGen;

/**
 * Script for self destruction of blocks.
 */
public class SelfDestruction : MonoBehaviour
{

	void Update ()
    {
        this.life_span -= Time.deltaTime;

        //Destroys the block
        if (this.life_span <=0)
        {
            this.GetComponent<ArtifactDataAccess>().artifact.Terminate();
        }
    }

    //******************************************************************
    // Attributes ******************************************************
    //******************************************************************

    /**
     * Block life span.
     */
    public float life_span = 5f;
}
