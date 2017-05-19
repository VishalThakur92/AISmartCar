using UnityEngine;
using System.Collections;
using TrackGen;

/**
 * Script responsible for generating track blocks in real time.
 */
public class RealTimeTrack : MonoBehaviour
{
    void Update()
    {
        //Checks whether another block must be generated
        this.block_creation_timer += Time.deltaTime;
        if (this.block_creation_timer >= this.block_creation_frequency)
        {
            this.block_creation_timer = 0;

            //Performs spatial displacement for the Perlin Noise module
            this.point_z += this.point_shift_step * Time.timeScale;

            //Checks if this block is the first to be generated
            if (this.track_piece != null)
            {
                //Creates a new block
                TrackPiece new_track_piece = new TrackPiece();
                //Sets the new block as child of the previous block so that the asset perform the fitting
                new_track_piece.SetParentNode(this.track_piece);
                //Stores the reference to the new block
                this.track_piece = new_track_piece;
            }
            //If this is the first block
            else
            {
                this.track_piece = new TrackPiece();
            }

            //Adds self destruction script
            if (self_destruct_blocks)
            {
                SelfDestruction self_destruction = this.track_piece.GetGameObject().AddComponent<SelfDestruction>();
                //Sets the life span
                self_destruction.life_span = this.blocks_life_span;
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
     * Local reference to the last generated block.
     */
    private TrackPiece track_piece = null;

    /**
     * Variable for the shift control of the blocks generation module.
     * This is related to the selection of a point in 3D space for the acquisition of noise based on the Perlin Noise algorithm.
     */
    private float point_z = 0f;
    public float point_shift_step = 1f;

    /**
     * New blocks generation frequency.
     */
    public float block_creation_frequency = 0.5f;
    private float block_creation_timer = 0f;

    /**
     * Do you want the blocks to self-destruct?
     */
    public bool self_destruct_blocks = true;

    /**
     * Block life span. (Only applicable if self-destruction is active)
     */
    public float blocks_life_span = 30f;
}
