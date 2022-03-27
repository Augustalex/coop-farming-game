using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class RiverSoil : MonoBehaviour
{
        private RiverSeedGrowth _seedInGround;

        public event Action<RiverSoil.RiverState> BlockStateChanged;

        public enum RiverSoilState
        {
            Free,
            Occupied,
        }
        
        public struct RiverState
        {
            public RiverSoilState soilState;
            public RiverSeedGrowth.RiverLevelData levelData;
        }

        public void SeedWithTemplate(GameObject seedTemplate)
        {
            if (_seedInGround) return;

            SeedUp(seedTemplate);
            
            BlockStateChanged?.Invoke(new RiverState
            {
                soilState = RiverSoilState.Occupied,
                levelData = _seedInGround.GetLevelData()
            });
        }

        private void SeedUp(GameObject seedTemplate)
        {
            var seed = Instantiate(seedTemplate);
            seed.transform.position = transform.position + Vector3.up * 2f;

            _seedInGround = seed.GetComponent<RiverSeedGrowth>();

            _seedInGround.Died += OnSeedsDied;
            _seedInGround.LevelsUpdated += OnLevelsUpdated;
        }
        
        private void OnLevelsUpdated(RiverSeedGrowth.RiverLevelData levelData)
        {
            BlockStateChanged?.Invoke(new RiverState
            {
                soilState = RiverSoilState.Occupied,
                levelData = levelData
            });
        }

        private void RemoveSeedState()
        {
            _seedInGround.Died -= OnSeedsDied;
            _seedInGround.LevelsUpdated -= OnLevelsUpdated;
            _seedInGround = null;
        }

        private void OnSeedsDied()
        {
            RemoveSeedState();
            
            BlockStateChanged?.Invoke(new RiverState
            {
                soilState = RiverSoilState.Free,
                levelData = new RiverSeedGrowth.RiverLevelData
                {
                    healthLevel = 0,
                }
            });
        }

        public bool IsFree()
        {
            return !_seedInGround;
        }

        public bool HasSeed()
        {
            return _seedInGround;
        }

        public void AddSpeedNutrient()
        {
            _seedInGround.AddSpeedNutrient();
        }
}
