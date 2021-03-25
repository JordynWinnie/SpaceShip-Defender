﻿using UnityEngine;

namespace EnemyScripts
{
    public class AsteroidScript : EnemyBase
    {
        //declares an Enum state to allow spawnning of future asteroids
        public enum AsteroidSize
        {
            Large,
            Medium,
            Small
        }

        //sets a Sprite array for all possible sprites.
        [SerializeField] private Sprite[] asteroidSprites;

        /* Sets the next asteroid to spawn
     * If it is a large asteriod, the next one should be medium
     * medium -> small
     * small -> nothing
     */
        [SerializeField] private GameObject asteroidToSpawn;

        //declares current asteroid size, can be changed in inspector
        public AsteroidSize asteroidSize;

        //sets the speed in which asteroids rotate (for cosmetic purposes only)
        public float rotateSpeed = 10f;

        //stores original velocity in Vector2 so it can be reused after a timeFreeze powerup
        private Vector2 originalVelocity;
        private Rigidbody2D rb2d;
        public override void Start()
        {
            base.Start();
            //Changes the scaling of the asteroid based on the enum provided:
            switch (asteroidSize)
            {
                case AsteroidSize.Large:
                    transform.localScale = new Vector3(3, 3, 3);
                    break;

                case AsteroidSize.Medium:
                    transform.localScale = new Vector3(2, 2, 2);
                    break;

                case AsteroidSize.Small:
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
            }

            //Changes the sprite to a random sprite provided in the SpriteArray:
            GetComponentInChildren<SpriteRenderer>().sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];

            //Sets a random velocity for the asteroid to move
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.velocity = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));

            //stores original velocity to be reapplied after timeFreeze
            originalVelocity = GetComponent<Rigidbody2D>().velocity;
        }

        private void Update()
        {
            //Actively rotates object by its axis:
            //https://stackoverflow.com/questions/28648071/rotate-object-in-unity-3d
            transform.Rotate(0, 0, 10 * Time.deltaTime * rotateSpeed);

            if (isFrozen)
            {
                rb2d.velocity = Vector2.zero;
                return;
            }
            rb2d.velocity = originalVelocity;
        }

        /// <summary>
        ///     SpawnChildAsteroids will be called when the asteroid is going to be destroyed
        ///     it will spawn the appropriate number of asteroids based on the current asteroid size
        ///     Large asteroids break into 1 - 4 medium asteroids
        ///     Medium asteroids break into 1 - 6 small asteroids
        ///     Small asteroids do not do anything
        /// </summary>
        private void SpawnChildAsteroids()
        {
            switch (asteroidSize)
            {
                case AsteroidSize.Large:
                    //Determines random number of Medium asteroids to spawn
                    var mediumAsteroidsToSpawn = Random.Range(1, 4);
                    //Instantiates based on random number, on the current asteroid's position
                    for (var i = 0; i < mediumAsteroidsToSpawn; i++)
                        Instantiate(asteroidToSpawn, transform.position, Quaternion.identity, GameManager.Instance.EnemyParent).GetComponent<AsteroidScript>().isFrozen = isFrozen;
                    break;

                case AsteroidSize.Medium:
                    //Same logic as above.
                    var smallAsteroidsToSpawn = Random.Range(1, 6);
                    for (var i = 0; i < smallAsteroidsToSpawn; i++)
                        Instantiate(asteroidToSpawn, transform.position, Quaternion.identity, GameManager.Instance.EnemyParent).GetComponent<AsteroidScript>().isFrozen = isFrozen;
                    break;
            }

            Destroy(gameObject);
        }

        protected override void EnemyDeath()
        {
            GameManager.Instance.PlayExplosionAnimation(currentCollision.transform,
                OnDeathAnimation.ExplosionTypes.BigExplosion);
            //trigger's the asteroid's spawnChildAsteroids method to break the asteroid:
            gameObject.GetComponent<AsteroidScript>().SpawnChildAsteroids();
        
            base.EnemyDeath();
        }
    }
}