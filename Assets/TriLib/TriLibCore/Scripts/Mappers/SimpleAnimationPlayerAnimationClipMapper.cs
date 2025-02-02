using TriLibCore.General;
using TriLibCore.Playables;
using UnityEngine;

namespace TriLibCore.Mappers
{
    /// <summary>
    /// Implements an <see cref="AnimationClipMapper"/> that creates a <see cref="SimpleAnimationPlayer"/> 
    /// for playing animation clips by index or name. When the animation type is set to Generic or Humanoid,
    /// and at least one animation clip is available, this mapper adds a <see cref="SimpleAnimationPlayer"/> to 
    /// the model's root GameObject and assigns the animation clips to it.
    /// </summary>
    [CreateAssetMenu(menuName = "TriLib/Mappers/Animation Clip/Simple Animation Player Animation Clip Mapper", fileName = "SimpleAnimationPlayerAnimationClipMapper")]
    public class SimpleAnimationPlayerAnimationClipMapper : AnimationClipMapper
    {
        /// <inheritdoc />
        /// <remarks>
        /// If the <see cref="AssetLoaderOptions.AnimationType"/> is either Generic or Humanoid, and there are one or more 
        /// animation clips provided (<paramref name="sourceAnimationClips"/> is not empty), the mapper adds a 
        /// <see cref="SimpleAnimationPlayer"/> component to the root GameObject (from <see cref="AssetLoaderContext.RootGameObject"/>)
        /// and assigns the animation clips to its <see cref="SimpleAnimationPlayer.AnimationClips"/> array. The animation player 
        /// is initially disabled to allow for further configuration if necessary.
        /// The original animation clips are always returned.
        /// </remarks>
        /// <param name="assetLoaderContext">
        /// The context used to load the model, which includes settings, references to the root GameObject, and other loading data.
        /// </param>
        /// <param name="sourceAnimationClips">
        /// An array of animation clips loaded from the model.
        /// </param>
        /// <returns>
        /// The same array of animation clips that was passed in (<paramref name="sourceAnimationClips"/>).
        /// </returns>
        public override AnimationClip[] MapArray(AssetLoaderContext assetLoaderContext, AnimationClip[] sourceAnimationClips)
        {
            if ((assetLoaderContext.Options.AnimationType == AnimationType.Generic ||
                 assetLoaderContext.Options.AnimationType == AnimationType.Humanoid) && sourceAnimationClips.Length > 0)
            {
                var simpleAnimationPlayer = assetLoaderContext.RootGameObject.AddComponent<SimpleAnimationPlayer>();
                simpleAnimationPlayer.AnimationClips = sourceAnimationClips;
                simpleAnimationPlayer.enabled = false;
            }
            return sourceAnimationClips;
        }
    }
}
