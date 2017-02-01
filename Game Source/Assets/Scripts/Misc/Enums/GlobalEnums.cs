using System.ComponentModel;

namespace Assets.Scripts.Misc.Enums
{

    public enum GlobalEnums : byte
    {
        CurvePoints = 1
    }

    public enum ConstantChar : int
    {
        MaximumHealth = 12
    }

    public enum OriginBullet : byte
    {
        [Description("Enemy")]
        Enemy = 0,
        [Description("Player")]
        Player = 1
    }

    public enum BeeType : byte
    {
        Attacker = 0,
        Worker = 1
    }
}