using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SweetSugar.Scripts.Blocks;
using SweetSugar.Scripts.Level;
using NotImplementedException = System.NotImplementedException;

namespace SweetSugar.Scripts.Spawner
{
    public class Disperser : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer SpawnItemRenderer, SpawnItemRenderer_1, SpawnItemRenderer_2;
        [SerializeField]
        private Sprite[] SpawnersSprite;
        [SerializeField]
        private Transform MainObject;

        public Animator _animator;
        public void SetSpawnRenderer(SingleSpawn spawnersType)
        {
            switch(spawnersType.SpawnersType)
            {
                case Spawners.None:
                    break;
                case Spawners.SpiralBlock:
                    SpawnItemRenderer.sprite = SpawnersSprite[0];
                    SpawnItemRenderer_1.sprite = SpawnersSprite[0];
                    break;
                case Spawners.TimeBomb:
                    SpawnItemRenderer.sprite = SpawnersSprite[1];
                    SpawnItemRenderer_1.sprite = SpawnersSprite[1];
                    break;
                case Spawners.Ingredient:
                    SpawnItemRenderer_1.sprite = SpawnersSprite[2];
                    SpawnItemRenderer.sprite = SpawnersSprite[2];
                    break;
                case Spawners.HORIZONTAL_STRIPED:
                    SpawnItemRenderer_1.sprite = SpawnersSprite[3];
                    SpawnItemRenderer.sprite = SpawnersSprite[3];
                    break;
                case Spawners.VERTICAL_STRIPED:
                    SpawnItemRenderer_1.sprite = SpawnersSprite[4];
                    SpawnItemRenderer.sprite = SpawnersSprite[4];
                    break;
                case Spawners.MARMALADE:
                    SpawnItemRenderer_1.sprite = SpawnersSprite[5];
                    SpawnItemRenderer.sprite = SpawnersSprite[5];
                    break;
                case Spawners.MULTICOLOR:
                    SpawnItemRenderer_1.sprite = SpawnersSprite[6];
                    SpawnItemRenderer.sprite = SpawnersSprite[6];
                    break;
            }
            switch (spawnersType.SpawnersType_2)
            {
                case Spawners.None:
                    SpawnItemRenderer_1.gameObject.SetActive(false);
                    SpawnItemRenderer_2.gameObject.SetActive(false);
                    break;
                case Spawners.SpiralBlock:

                    SpawnItemRenderer_2.sprite = SpawnersSprite[0];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.TimeBomb:

                    SpawnItemRenderer_2.sprite = SpawnersSprite[1];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.Ingredient:

                    SpawnItemRenderer_2.sprite = SpawnersSprite[2];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.HORIZONTAL_STRIPED:
                    SpawnItemRenderer_2.sprite = SpawnersSprite[3];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.VERTICAL_STRIPED:
                    SpawnItemRenderer_2.sprite = SpawnersSprite[4];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.MARMALADE:
                    SpawnItemRenderer_2.sprite = SpawnersSprite[5];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
                case Spawners.MULTICOLOR:
                    SpawnItemRenderer_2.sprite = SpawnersSprite[6];
                    SpawnItemRenderer.gameObject.SetActive(false);
                    break;
            }
        }

        public void SetRotation(RotationType type)
        {
            switch (type)
            {
                case RotationType.Top:
                    MainObject.rotation = Quaternion.Euler(0,0,0);
                    break;
                case RotationType.Bottom:
                    MainObject.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case RotationType.Left:
                    MainObject.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case RotationType.right:
                    MainObject.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                default:
                    break;
            }
        }

        public void Animate()
        {
            _animator.SetTrigger("throw");
        }
    }
}