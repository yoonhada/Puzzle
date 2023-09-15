using Scripts.Block;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int _indexX;
    private int _indexY;

    public BlockType Type { get; set; }

    public void Reposition ( float in_duration )
    {
        var offsetY = 0f;

        if ( ( _indexX & 0x01 ) == 1 )
        {
            offsetY = BlockController.BlockOffsetPos.y * 0.5f;
        }

        transform.localPosition = new Vector3( BlockController.BlockStartPos.x,
            BlockController.BlockStartPos.y + BlockController.BlockOffsetPos.y * _indexY + offsetY,
            transform.localPosition.z );
    }

    public void Create ( int y, int x, BlockType type )
    {
        Type = type;

        _indexY = y;
        _indexX = x;
    }
}