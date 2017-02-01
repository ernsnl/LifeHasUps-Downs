using JetBrains.Annotations;

namespace Assets.Scripts.Models.Obstacles
{
    public interface IObstacle
    {       
        void AfterExplosion();
        void AfterAttack();
        void Update();
        void FixedUpdate();
    }
}