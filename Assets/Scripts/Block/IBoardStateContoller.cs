using UniRx;

namespace SJLGPP.Block
{
    public interface IBoardStateContoller
    {
        IReadOnlyReactiveProperty<bool> IgnoreDropBlockProcess { get; }

        bool AbleToBlockControll { get; }

        void SetBombEffectState();
        void CompleteBombEffectState();
    }
}