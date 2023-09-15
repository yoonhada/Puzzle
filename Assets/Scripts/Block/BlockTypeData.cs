using System;
using Scripts.Block;
using UnityEngine;

namespace SJLGPP.Block
{
    [Serializable]
    public class BlockTypeDatas
    {
        public BlockTypeData[] datas;
    }

    [Serializable]
    public class BlockTypeData
    {
        [SerializeField]
        private bool _isSelectable;

        [SerializeField]
        private bool _isConnectable;

        [SerializeField]
        private bool _isPangable;

        [SerializeField]
        private string _idleSpriteName;

        [SerializeField]
        private string _selectedSpriteName;
        [SerializeField]
        private BlockType _blockType;
        public BlockType BlockType
        {
            get { return _blockType; }
        }
        public bool IsSelectable
        {
            get { return _isSelectable; }
        }
        public bool IsConnectable
        {
            get { return _isConnectable; }
        }
        public bool IsPangable
        {
            get { return _isPangable; }
        }
        public string IdleSpriteName
        {
            get { return _idleSpriteName; }
        }
        public string SelectedSpriteName
        {
            get { return _selectedSpriteName; }
        }
    }
}