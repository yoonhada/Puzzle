using UniRx;

namespace SJLGPP.Block
{
    public class BoardStateContoller : IBoardStateContoller
    {
        #region properties
        public  IReadOnlyReactiveProperty<bool> IgnoreDropBlockProcess => _ignoreDropBlockProcess;
        private ReactiveProperty<bool>          _ignoreDropBlockProcess = new ();
        public  bool                            AbleToBlockControll => _ableToBlockControll;
        private bool                            _ableToBlockControll = true;
        #endregion

        
        
        /// <summary>
        /// 폭탄이 터지는 연출이 시작될때 처리입니다.
        /// </summary>
        public void SetBombEffectState()
        {
            _ignoreDropBlockProcess.Value = true;
            _ableToBlockControll          = false;
        }

        /// <summary>
        /// 폭탄이 터지는 연출이 완료될때 처리입니다.
        /// </summary>
        public void CompleteBombEffectState()
        {
            _ignoreDropBlockProcess.Value = false;
            _ableToBlockControll          = true;
        }
    }
}