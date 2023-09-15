namespace SJLGPP.Combo
{
    public interface IComboController
    {
        bool IsFeverMode { get; }
        
        void AddComboCount();
        void OnDragBlock();
    }
}