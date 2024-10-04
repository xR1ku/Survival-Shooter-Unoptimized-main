using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    private float timer;
    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;
    private ParticleSystem gunParticles;
    private LineRenderer gunLine;
    private AudioSource gunAudio;
    private Light gunLight;
    private float effectsDisplayTime = 0.2f;

    // Tracks whether effects are currently active
    private bool effectsActive = false;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        // Update the timer with deltaTime
        timer += Time.deltaTime;

        // Only proceed with shooting logic if time allows and game is not paused
        if (Time.timeScale == 0) return;  // Prevent Update logic when game is paused

        // Check if the player pressed the fire button and time between bullets has passed
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        // Disable the effects once enough time has passed since the last shot
        if (effectsActive && timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        if (!effectsActive) return; // Skip if effects are already disabled

        gunLine.enabled = false;
        gunLight.enabled = false;
        effectsActive = false;
    }

    void Shoot()
    {
        timer = 0f;

        // Play the shooting sound once
        gunAudio.Play();

        // Enable light and line renderer for visual effect
        gunLight.enabled = true;
        gunParticles.Stop(); // Stop previous particles before playing new ones
        gunParticles.Play();

        // Show line renderer
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set up the shoot ray (starts at gun position, goes forward)
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast, checking if we hit something on the shootable layer
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try to get the enemy health component
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            // If the enemy was hit, apply damage
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            // Set the second position of the line renderer to the hit point
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            // If no hit, extend the line to the max range
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

        // Indicate that effects are active so we can turn them off later
        effectsActive = true;
    }
}
