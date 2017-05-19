using UnityEngine;
using System.Collections;
using TrackGen;

/**
 * Script responsible for generating a complete track at once.
 */
public class PreGeneratedTrack : MonoBehaviour
{

	void Start ()
    {
        //Generates track
        TrackPiece track_piece = null;
	    for(int block = 0; block < this.blocks_count; ++block)
        {
            //Performs spatial displacement for the Perlin Noise module
            this.point_z += this.point_shift_step * Time.timeScale;

            //Checks if this block is the first to be generated
            if (track_piece != null)
            {
                //Creates a new block
                TrackPiece new_track_piece = new TrackPiece();
                //Sets the new block as child of the previous block so that the asset perform the fitting
                new_track_piece.SetParentNode(track_piece);
                //Stores the reference to the new block
                track_piece = new_track_piece;
            }
            //If this is the first block
            else
            {
                track_piece = new TrackPiece();
            }

            //Applies the settings in the new block
            TrackPieceEditor track_piece_editor = track_piece.game_object.GetComponent<TrackPieceEditor>();
            track_piece_editor.seed = this.seed;
            track_piece_editor.point.z = this.point_z;
            //Calls internal configuration process of the block
            track_piece_editor.Setup();
            //Performs block generation based on configuration parameters
            (new TrackPieceGenerator()).Generate(track_piece);
            //Eliminate previous block reference from the current block, as it is no longer necessary to keep connection between the two
            track_piece.SetParentNode(null);
        }
	}

    /**
     * Seed to be used in generation.
     */
    public ulong seed = 666;

    /**
     * Blocks count.
     */
    public int blocks_count = 100;

    /**
     * Variable for the shift control of the blocks generation module.
     * This is related to the selection of a point in 3D space for the acquisition of noise based on the Perlin Noise algorithm.
     */
    private float point_z = 0f;
    public float point_shift_step = 1f;

}
