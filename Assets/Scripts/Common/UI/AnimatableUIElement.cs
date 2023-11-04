using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.UI
{
    public abstract class AnimatableUIElement : UIElement
    {
        public abstract UniTask ActivateAsync(CancellationToken token);

        public abstract UniTask DeactivateAsync(CancellationToken token);
    }
}