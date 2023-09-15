using System;
using UnityEngine;

namespace SJLGPP.Block
{
    [Serializable]
    public class BlockConfigureData
    {
        public int BOARD_ROW = 6; //블럭 행 수
        public int BOARD_COLUMN = 7; //블럭 열 수
        public int BLOCK_INITIALIZE_COUNT = 60; //초기 블럭 풀 갯수
        public int BOARD_LEFT_POSITION = -288; //보드 왼쪽 위치
        public int BOARD_BOTTOM_POSITION = -305; //보드 아래쪽 위치
        public int BLOCK_VERTICAL_DISTANCE = 112; //블럭간 세로 간격
        public int BLOCK_HORIZONTAL_DISTANCE = 96; //블럭간 가로 간격
        public int INCLINE_OFFSET = 58; //짝수 컬럼 OFFSET
        public int BLOCK_MIN_CONNECTION = 3; //블럭 터지기 위한 연결 최소 갯수
        public int DELTAPANG_START_ORDER = 3; //델타팡 만족 순서
        public float DELTAPANG_FIRST_DELAY = 0.1f; //맨처음 델타팡의 딜레이
        public float DELTAPANG_LEFT_DELAY = 0.05f; //처음 이후 델타팡의 딜레이
        public float MOVE_SECOND_PER_ROW = 0.15f; //1칸당 블럭 내려오는 시간
        public float MOVE_SECOND_MIN = 0.2f; //블럭내려오는 최소시간
        public float MOVE_SECOND_MAX = 0.3f; //블럭내려오는 최대시간
        public float CONNECTION_COUNT_CONDITION_FOR_BOMB = 5; //폭탄을 만들기 위해 필요한 블럭연결수
        public float CONNECTION_COUNT_CONDITION_FOR_SUPER_BOMB = 6; //수퍼폭탄을 만들기 위해 필요한 블럭연결수
        public int SUPER_BOMB_SCOPE = 3; //수퍼폭탄 터지는 범위

        //홀수 열 기준 주변 블럭 인덱스
        public Vector2Int[] AROUND_OFFSET_FOR_ODD =
            { new(0, 1), new(1, 1), new(1, 0), new(0, -1), new(-1, 0), new(-1, 1) };
        //짝수 열 기준 주변 블럭 인덱스
        public Vector2Int[] AROUND_OFFSET_FOR_EVEN =
            { new(0, 1), new(1, 0), new(1, -1), new(0, -1), new(-1, -1), new(-1, 0) };

        public int BLOCK_DIRECTION_INDEX_UP;
        public int BLOCK_DIRECTION_INDEX_LEFT_UP = 1;
        public int BLOCK_DIRECTION_INDEX_LEFT_DOWN = 2;
        public int BLOCK_DIRECTION_INDEX_DOWN = 3;
        public int BLOCK_DIRECTION_INDEX_RIGHT_DOWN = 4;
        public int BLOCK_DIRECTION_INDEX_RIGHT_UP = 5;

        public Vector2Int GetNextDirectionIndex ( Vector2Int index, int direction )
        {
            var offsets = index.x % 2 == 0 ? AROUND_OFFSET_FOR_EVEN : AROUND_OFFSET_FOR_ODD;

            return index + offsets[direction];
        }
    }
}