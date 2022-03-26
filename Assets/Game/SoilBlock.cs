﻿using System;
using UnityEngine;

namespace Game
{
    public class SoilBlock : MonoBehaviour
    {
        private GameObject _wateredSoil;
        private GameObject _drySoil;
        private SeedGrowth _seedInGround;
        private float _emptyTime;
        private Plant _unharvestedPlant;

        public event Action<BlockState> BlockStateChanged;

        public enum SoilState
        {
            Free,
            HasSeeds,
            ReadyToHarvest
        }
        
        public struct BlockState
        {
            public SoilState soilState;
            
            public SeedGrowth.LevelData levelData;
        }

        private void Awake()
        {
            _wateredSoil = GetComponentInChildren<WateredSoil>().gameObject;
            _drySoil = GetComponentInChildren<DrySoil>().gameObject;
        }

        private void Start()
        {
            _wateredSoil.SetActive(false);
            _drySoil.SetActive(true);
        }

        private void Update()
        {
            if (IsFree())
            {
                _emptyTime += Time.deltaTime;
            }
            else
            {
                _emptyTime = 0f;
            }

            if (_emptyTime > 60 * 5)
            {
                ReplaceWithGrass();
            }
        }

        private void ReplaceWithGrass()
        {
            Destroy(gameObject);
            Instantiate(AssetLibrary.Instance.grassTemplate, transform.position, transform.rotation, transform.parent);
        }

        public void Water()
        {
            WetUp();
        }

        private void WetUp()
        {
            _drySoil.SetActive(false);
            _wateredSoil.SetActive(true);

            if (_seedInGround)
            {
                _seedInGround.Water();
            }
        }

        public void Seed()
        {
            SeedWithTemplate(AssetLibrary.Instance.seedtemplate);
        }

        private void DryUp()
        {
            _wateredSoil.SetActive(false);
            _drySoil.SetActive(true);
        }

        public void SeedWithTemplate(GameObject seedTemplate)
        {
            if (_seedInGround) return;
            if (_unharvestedPlant) return;

            DryUp();

            SeedUp(seedTemplate);
            
            BlockStateChanged?.Invoke(new BlockState
            {
                soilState = SoilState.HasSeeds,
                levelData = _seedInGround.GetLevelData()
            });
        }

        private void SeedUp(GameObject seedTemplate)
        {
            var seed = Instantiate(seedTemplate);
            seed.transform.position = transform.position + Vector3.up * 2f;

            _seedInGround = seed.GetComponent<SeedGrowth>();

            _seedInGround.GrownUp += OnSeedsGrownUp;
            _seedInGround.Died += OnSeedsDied;
            _seedInGround.NoWater += OnWaterRanOut;
            _seedInGround.LevelsUpdated += OnLevelsUpdated;
        }

        private void OnWaterRanOut()
        {
            if (!_drySoil.activeSelf)
            {
                DryUp();
            }
        }

        private void OnLevelsUpdated(SeedGrowth.LevelData levelData)
        {
            BlockStateChanged?.Invoke(new BlockState
            {
                soilState = SoilState.HasSeeds,
                levelData = levelData
            });
        }

        private void RemoveSeedState()
        {
            _seedInGround.GrownUp -= OnSeedsGrownUp;
            _seedInGround.Died -= OnSeedsDied;
            _seedInGround.NoWater -= OnWaterRanOut;
            _seedInGround.LevelsUpdated -= OnLevelsUpdated;
            _seedInGround = null;
        }

        private void OnSeedsDied()
        {
            RemoveSeedState();
            DryUp();
            
            BlockStateChanged?.Invoke(new BlockState
            {
                soilState = SoilState.Free,
                levelData = new SeedGrowth.LevelData
                {
                    healthLevel = 0,
                    waterLevel = 0,
                    waterState = WaterLevelIndicator.WaterLevelIndicatorState.Wet
                }
            });
        }

        private void OnSeedsGrownUp(Plant plant)
        {
            RemoveSeedState();
            
            _unharvestedPlant = plant;
            _unharvestedPlant.SetAsInSoil();
            _unharvestedPlant.Harvested += OnPlantHarvested;

            BlockStateChanged?.Invoke(new BlockState
            {
                soilState = SoilState.ReadyToHarvest,
                levelData = new SeedGrowth.LevelData
                {
                    healthLevel = 4,
                    waterLevel = 4,
                    waterState = WaterLevelIndicator.WaterLevelIndicatorState.Wet
                }
            });

            DryUp();
        }

        private void RemoveUnharvestedPlantState()
        {
            _unharvestedPlant.Harvested -= OnPlantHarvested;
            _unharvestedPlant = null;
        }

        private void OnPlantHarvested()
        {
            RemoveUnharvestedPlantState();

            BlockStateChanged?.Invoke(new BlockState
            {
                soilState = SoilState.Free,
                levelData = new SeedGrowth.LevelData
                {
                    healthLevel = 0,
                    waterLevel = 0,
                    waterState = WaterLevelIndicator.WaterLevelIndicatorState.Wet
                }
            });
        }

        public bool IsFree()
        {
            return !_seedInGround && !_unharvestedPlant;
        }

        public bool HasSeed()
        {
            return _seedInGround;
        }

        public void AddNutrient()
        {
            _seedInGround.AddNutrient();
        }

        public void AddSpeedNutrient()
        {
            _seedInGround.AddSpeedNutrient();
        }
    }
}