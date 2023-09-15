using Zenject;

namespace SJLGPP.Combo
{
    public class ComboController : IComboController
    {
        #region private

        [Inject]
        private IComboUseCase _comboUseCase;

        #endregion

        public bool IsFeverMode
        {
            get { return _comboUseCase.IsFeverMode; }
        }

        /// <summary>
        ///     블럭 드래그를 완료했으면 콤보를 증가시키기 위해 호출합니다.
        /// </summary>
        public void AddComboCount ()
        {
            _comboUseCase.AddComboCount();
        }

        /// <summary>
        ///     블럭을 드래그 할때마다 호출됩니다.
        /// </summary>
        public void OnDragBlock ()
        {
            _comboUseCase.OnDragBlock();
        }
    }
}