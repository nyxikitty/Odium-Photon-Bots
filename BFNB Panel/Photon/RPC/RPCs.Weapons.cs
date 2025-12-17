namespace BLF_Odium_Network_Bots.Photon
{
    public partial class RPCs
    {
        // RPC 23: Create grenade locally
        public void LocalCreateGrenade(Vec3 position, Vec3 velocity, float forcedDelay, byte grenadeWeaponType)
        {
            SendRPC(23, position, velocity, forcedDelay, grenadeWeaponType);
        }

        // RPC 24: Local player hurt
        public void LocalHurt(int damagerID, float damage, Vec3 localPosition, byte damagerWeapon, float newHealth)
        {
            SendRPC(24, damagerID, damage, localPosition, damagerWeapon, newHealth);
        }

        // RPC 25: Local reload animation
        public void LocalReload()
        {
            SendRPC(25);
        }

        // RPC 26: Spawn throwing weapon locally
        public void LocalSpawnThrowingWeapon(Vec3 position, Vec3 velocity, byte weaponType)
        {
            SendRPC(26, position, velocity, weaponType);
        }

        // RPC 30: Melee attack animation
        public void MpMeleeAnimation()
        {
            SendRPC(30);
        }

        // RPC 31: Throw grenade animation
        public void MpThrowGrenadeAnimation()
        {
            SendRPC(31);
        }

        // RPC 51: Shoot weapon
        public void RpcShoot(int actorID, float damage, Vec3 position, Vec3 direction, byte numberOfBullets, byte spread, double timeShot, int weaponType)
        {
            SendRPC(51, actorID, damage, position, direction, numberOfBullets, spread, timeShot, weaponType);
        }

        // RPC 76: Weapon camo changed
        public void WeaponCamoChanged(int value)
        {
            SendRPC(76, value);
        }

        // RPC 77: Weapon type changed
        public void WeaponTypeChanged(byte value)
        {
            SendRPC(77, value);
        }
    }
}
