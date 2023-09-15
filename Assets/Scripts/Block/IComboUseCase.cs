using UniRx;

namespace SJLGPP.Combo
{
    public interface IComboUseCase
    {
        bool                                  IsFeverMode { get; }
        public IReadOnlyReactiveProperty<int> FeverGrade  { get; }
        public IReadOnlyReactiveProperty<int> FeverStep   { get; }
        public IReadOnlyReactiveProperty<int> ComboCount  { get; }
        
        void AddComboCount();
        void OnDragBlock();
    }
}