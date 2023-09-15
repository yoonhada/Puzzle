using System;
using System.Collections.Generic;
using Scripts.Block;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
    private const int COUNT = 5;
    private const int BlockHeightCount = 6;
    private const int BlockWidthCount = 7;
    public static readonly Vector2 BlockOffsetPos = new(82f, 94.5f);
    public static readonly Vector2 BlockStartPos = new(0, 79f);

    public Block _objBlock;
    private readonly BlockType[,] _blockType = new BlockType[BlockHeightCount, BlockWidthCount];
    private readonly List<BlockType> listBlockTypes = new();

    private float _makeStartX;

    [NonSerialized]
    public List<Block>[] ListBlocks = new List<Block>[BlockWidthCount];

    private void Start ()
    {
        RequestStartBlock();

        _makeStartX = ( BlockWidthCount - 1 ) * 0.5f * BlockOffsetPos.x;

        var count = 0;
        for ( var x = 0; x < BlockWidthCount; x++ )
        {
            var offsetY = 0f;
            if ( ( x & 0x01 ) == 0 )
            {
                offsetY = BlockOffsetPos.y * 0.5f;
            }

            ListBlocks[x] = new List<Block>();

            for ( var y = 0; y < BlockHeightCount; y++ )
            {
                var go = Instantiate( _objBlock.gameObject );
                var block = go.GetComponent<Block>();
                block.Create( y, x, _blockType[y, x] );
            }
        }
    }

    private void RequestStartBlock ()
    {
        listBlockTypes.Clear();
        var list = new List<int>();
        for ( var i = 0; i < COUNT; i++ )
        {
            list.Add( i + ( int )BlockType.NormalRed );
        }

        for ( var i = 0; i < COUNT; i++ )
        {
            var rand = Random.Range( 0, list.Count );
            listBlockTypes.Add( ( BlockType )list[rand] );
            list.RemoveAt( rand );
        }

        for ( var y = 0; y < BlockHeightCount; y++ )
        {
            for ( var x = 0; x < BlockWidthCount; x++ )
            {
                _blockType[y, x] = ( BlockType )Random.Range( ( int )BlockType.NormalRed, ( int )BlockType.NormalEnd );
            }
        }
    }

    public void BlockStart ()
    {
        for ( var y = 0; y < BlockHeightCount; y++ )
        {
            for ( var x = 0; x < BlockWidthCount; x++ )
            {
                ListBlocks[x][y].Reposition( 1f + y * 0.12f );
            }
        }
    }
}