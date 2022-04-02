using UnityEngine;

namespace Game
{
    public class Sounds : MonoSingleton<Sounds>
    {
        public AudioClip poof;
        public AudioClip poofLoud;
        public AudioClip horn;
        private const float HornVolume = .6f;

        public AudioClip truckMoving;

        public AudioClip kaching;
        private const float KachingVolume = .4f;

        public AudioClip plop;
        private const float PlopVolume = .6f;

        public AudioClip plop2;
        private const float Plop2Volume = .6f;

        public AudioClip blublub;
        private const float BlubVolume = .4f;

        public AudioClip wosh;
        private const float WoshVolume = .8f;

        public void PlayHoeSound(Vector3 position)
        {
            PlaySound(poof, position);
        }

        public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }

        public void PlayPickupItemSound(Vector3 transformPosition)
        {
            PlaySound(plop, transformPosition, PlopVolume);
        }

        public void PlayDropItemSound(Vector3 transformPosition)
        {
            PlaySound(plop2, transformPosition, PlopVolume);
        }

        public void PlayFailedWaterSound(Vector3 position)
        {
            PlaySound(poof, position, 1f);
        }

        public void PlayWaterSound(Vector3 transformPosition)
        {
            PlaySound(blublub, transformPosition, BlubVolume);
        }

        public void PlayerWaterRechargeSound(Vector3 transformPosition)
        {
            PlaySound(blublub, transformPosition);
        }
        
        public void PlayWooshSound(Vector3 transformPosition)
        {
            PlaySound(wosh, transformPosition, WoshVolume);
        }

        public void PlayPlaceTileSound(Vector3 transformPosition)
        {
            PlaySound(poof, transformPosition);
        }

        public void PlayFailedToPlaceTileSound(Vector3 transformPosition)
        {
            PlaySound(poof, transformPosition);
        }

        public void PlayTilePileDoneSound(Vector3 transformPosition)
        {
            PlaySound(poof, transformPosition, 1f);
            PlaySound(plop2, transformPosition, .3f);
        }

        public void PlayPickBushSound(Vector3 transformPosition)
        {
            PlaySound(plop, transformPosition, PlopVolume);
        }

        public void PlayUseBucketSound(Vector3 transformPosition)
        {
            PlaySound(plop2, transformPosition, Plop2Volume);
        }

        public void PlayFailedToUseBucketSound(Vector3 transformPosition)
        {
            PlaySound(plop2, transformPosition, Plop2Volume);
        }

        public void PlayAddedToSeedSack(Vector3 transformPosition)
        {
            PlaySound(plop, transformPosition, PlopVolume);
        }

        public void PlayTruckHornSound(Vector3 transformPosition)
        {
            PlaySound(horn, transformPosition, HornVolume);
        }

        public void PlayTruckMovingSound(Vector3 transformPosition)
        {
            PlaySound(truckMoving, transformPosition);
        }

        public void PlayKachingSound(Vector3 position)
        {
            PlaySound(kaching, position, KachingVolume);
        }

        public void PlayPickJobSound(Vector3 position)
        {
            PlaySound(plop, position, PlopVolume);
        }

        public void PlayBuyItemSound(Vector3 position)
        {
            PlayKachingSound(position);
        }

        public void PlayMoveCursorSound(Vector3 transformPosition)
        {
            PlaySound(wosh, transformPosition, .5f);
        }

        public void PlayGrowPlantSound(Vector3 transformPosition)
        {
            PlaySound(plop, transformPosition, PlopVolume * 1.5f);
        }

        public void PlayRemoveFlowerSound(Vector3 transformPosition)
        {
            PlaySound(poof, transformPosition, 1f);
        }

        public void PlayDestroyTileSound(Vector3 transformPosition)
        {
            PlaySound(poof, transformPosition, 1f);
        }

        public void PlayTouchdownSound(Vector3 transformPosition)
        {
            PlaySound(poofLoud, transformPosition, 1f);
        }

        public void PlayPlayerJoinedSound(Vector3 transformPosition)
        {
            PlaySound(plop2, transformPosition, .175f);
        }
    }
}