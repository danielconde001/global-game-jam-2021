﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FPSControllerLPFP;
using TMPro;

// ----- Low Poly FPS Pack Free Version -----
public class HandgunScriptLPFP : MonoBehaviour {
	public static HandgunScriptLPFP current;
	[SerializeField] private Image pistolImage;
	[SerializeField] private Sprite pistolUnfilledSprite;
	[SerializeField] private Sprite pistolFilledSprite;
	[SerializeField] private FpsControllerLPFP fpsControllerLPFP;
	[SerializeField] private AudioClip clipGunEmpty;
	[SerializeField] private AudioClip clipFlashlight;
	[SerializeField] private AudioClip clipGetAmmo;
	[SerializeField] private bool canUseGun = true;
	public bool CanUseGun
	{
		set {canUseGun = value;}
	}
	[SerializeField] private Light selfLight;
	[SerializeField] private LineRenderer selfLineRenderer;
	[SerializeField] private LayerMask shootableLayer;
	[SerializeField] private Image crosshair;
	[SerializeField] private Sprite crosshairDot;
	[SerializeField] private Sprite crosshairFull;
	[SerializeField] private GameObject zombieBulletFX;
	[SerializeField] private GameObject woodBulletFX;
	//Animator component attached to weapon
	Animator anim;

	[SerializeField] private Camera playerCamera;
	[SerializeField] private float pcDefaultFov = 60.0f;
	[SerializeField] private float pcAimFov = 45.0f;
	[Header("Gun Camera")]
	//Main gun camera
	public Camera gunCamera;

	[Header("Gun Camera Options")]
	//How fast the camera field of view changes when aiming 
	[Tooltip("How fast the camera field of view changes when aiming.")]
	public float fovSpeed = 15.0f;
	//Default camera field of view
	[Tooltip("Default value for camera field of view (40 is recommended).")]
	public float defaultFov = 40.0f;

	public float aimFov = 15.0f;

	[Header("UI Weapon Name")]
	[Tooltip("Name of the current weapon, shown in the game UI.")]
	public string weaponName;
	private string storedWeaponName;

	[Header("Weapon Sway")]
	//Enables weapon sway
	[Tooltip("Toggle weapon sway.")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float aimMaxSwayAmount = 0.02f;
	private float currentMaxSwayAmount;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;

	[Header("Weapon Settings")]
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private float shootDelay;
	[SerializeField] private float maxRecoil;
	[SerializeField] private float maxAimRecoil;
	[SerializeField] private int damage;
	private bool canShoot = true;
	public float sliderBackTimer = 1.58f;
	private bool hasStartedSliderBack;

	//Eanbles auto reloading when out of ammo
	[Tooltip("Enables auto reloading when out of ammo.")]
	public bool autoReload;
	//Delay between shooting last bullet and reloading
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	//Holstering weapon
	private bool hasBeenHolstered = false;
	//If weapon is holstered
	private bool holstered;
	public bool Holstered
	{
		get {return holstered;}
	}
	//Check if running
	private bool isRunning;
	//Check if aiming
	private bool isAiming;
	public bool IsAiming
	{
		get {return isAiming;}
	}
	//Check if walking
	private bool isWalking;
	//Check if inspecting weapon
	private bool isInspecting;

	//How much ammo is currently left
	private int currentAmmo;
	private int totalAmmo;
	//Totalt amount of ammo
	[Tooltip("How much ammo the weapon should have.")]
	public int ammo;
	//Check if out of ammo
	private bool outOfAmmo;

	[Header("Bullet Settings")]
	//Bullet
	[Tooltip("How much force is applied to the bullet when shooting.")]
	public float bulletForce = 400;
	[Tooltip("How long after reloading that the bullet model becomes visible " +
		"again, only used for out of ammo reload aniamtions.")]
	public float showBulletInMagDelay = 0.6f;
	[Tooltip("The bullet model inside the mag, not used for all weapons.")]
	public SkinnedMeshRenderer bulletInMagRenderer;

	[Header("Grenade Settings")]
	public float grenadeSpawnDelay = 0.35f;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	//min should always bee 1
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	[Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	[Header("Audio Source")]
	//Main audio source
	public AudioSource mainAudioSource;
	//Audio source used for shoot sound
	public AudioSource shootAudioSource;

	[Header("UI Components")]
	public Text timescaleText;
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	[System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform bulletPrefab;
		public Transform casingPrefab;
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;
	
	[System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		//Array holding casing spawn points 
		//Casing spawn point array
		public Transform casingSpawnPoint;
		//Bullet prefab spawn from this point
		public Transform bulletSpawnPoint;
		//Grenade prefab spawn from this point
		public Transform grenadeSpawnPoint;
		public Transform fakeBulletEndPoint;
	}
	public spawnpoints Spawnpoints;

	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;

	private bool soundHasPlayed = false;

	public void SetupDeath()
	{
		HolsterGun();
	}

	public void GiveAmmo(int amount)
	{
		totalAmmo += amount;
		shootAudioSource.PlayOneShot(clipGetAmmo);
		UpdateAmmoText();
	}

	public void HolsterGun()
	{
		holstered = true;

		crosshair.sprite = crosshairDot;
		ammoText.text = "";
		mainAudioSource.clip = SoundClips.holsterSound;
		mainAudioSource.Play();
		pistolImage.sprite = pistolUnfilledSprite;

		hasBeenHolstered = true;
	}

	public void UnholsterGun()
	{
		holstered = false;
		
		crosshair.sprite = crosshairFull;
		UpdateAmmoText();
		mainAudioSource.clip = SoundClips.takeOutSound;
		mainAudioSource.Play ();
		pistolImage.sprite = pistolFilledSprite;

		hasBeenHolstered = false;
	}

	private void Awake () 
	{
		//Set the animator component
		anim = GetComponent<Animator>();
		//Set current ammo to total ammo value
		currentAmmo = ammo;

		muzzleflashLight.enabled = false;
	}

	private void Start () {
		//Save the weapon name
		storedWeaponName = weaponName;
		//Get weapon name from string to text
		currentWeaponText.text = weaponName;
		//Set total ammo text from total ammo int
		totalAmmoText.text = ammo.ToString();

		//Weapon sway
		initialSwayPosition = transform.localPosition;

		//Set the shoot sound to audio source
		shootAudioSource.clip = SoundClips.shootSound;

		currentMaxSwayAmount = maxSwayAmount;

		UpdateAmmoText();
		holstered = true;
		crosshair.sprite = crosshairDot;
		hasBeenHolstered = true;
		ammoText.text = "";
		current = this;
	}

	private void LateUpdate () {
		//Weapon sway
		if(canUseGun)
		{
			if (weaponSway == true) {
				float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
				float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;
				//Clamp movement to min and max values
				movementX = Mathf.Clamp 
					(movementX, -currentMaxSwayAmount, currentMaxSwayAmount);
				movementY = Mathf.Clamp 
					(movementY, -currentMaxSwayAmount, currentMaxSwayAmount);
				//Lerp local pos
				Vector3 finalSwayPosition = new Vector3 
					(movementX, movementY, 0);
				transform.localPosition = Vector3.Lerp 
					(transform.localPosition, finalSwayPosition + 
					initialSwayPosition, Time.deltaTime * swaySmoothValue);
			}
		}
	}
	
	private void Update () {

		//Aiming
		//Toggle camera FOV when right click is held down
		if(canUseGun)
		{
			if(Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting && !holstered) 
			{
				crosshair.color = Color.clear;
				gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView,
					aimFov, fovSpeed * Time.deltaTime);

				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, pcAimFov, fovSpeed * Time.deltaTime);
				
				currentMaxSwayAmount = aimMaxSwayAmount;
				isAiming = true;

				anim.SetBool ("Aim", true);

				if (!soundHasPlayed) 
				{
					mainAudioSource.clip = SoundClips.aimSound;
					mainAudioSource.Play ();
		
					soundHasPlayed = true;
				}
			} 
			else 
			{
				//When right click is released
				crosshair.color = Color.white;
				gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
					defaultFov,fovSpeed * Time.deltaTime);
				
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, pcDefaultFov, fovSpeed * Time.deltaTime);

				currentMaxSwayAmount = maxSwayAmount;
				isAiming = false;
				soundHasPlayed = false;
		
				anim.SetBool ("Aim", false);
			}
			//Aiming end

			//If randomize muzzleflash is true, genereate random int values
			if (randomMuzzleflash == true) {
				randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
			}

			/*
			//Timescale settings
			//Change timescale to normal when 1 key is pressed
			if (Input.GetKeyDown (KeyCode.Alpha1)) 
			{
				Time.timeScale = 1.0f;
				timescaleText.text = "1.0";
			}
			//Change timescale to 50% when 2 key is pressed
			if (Input.GetKeyDown (KeyCode.Alpha2)) 
			{
				Time.timeScale = 0.5f;
				timescaleText.text = "0.5";
			}
			//Change timescale to 25% when 3 key is pressed
			if (Input.GetKeyDown (KeyCode.Alpha3)) 
			{
				Time.timeScale = 0.25f;
				timescaleText.text = "0.25";
			}
			//Change timescale to 10% when 4 key is pressed
			if (Input.GetKeyDown (KeyCode.Alpha4)) 
			{
				Time.timeScale = 0.1f;
				timescaleText.text = "0.1";
			}
			//Pause game when 5 key is pressed
			if (Input.GetKeyDown (KeyCode.Alpha5)) 
			{
				Time.timeScale = 0.0f;
				timescaleText.text = "0.0";
			}
			*/

			//Set current ammo text from ammo int
			currentAmmoText.text = currentAmmo.ToString ();

			//Continosuly check which animation 
			//is currently playing
			AnimationCheck ();

			/*
			//Play knife attack 1 animation when Q key is pressed
			if (Input.GetKeyDown (KeyCode.Q) && !isInspecting) 
			{
				anim.Play ("Knife Attack 1", 0, 0f);
			}
			//Play knife attack 2 animation when F key is pressed
			if (Input.GetKeyDown (KeyCode.F) && !isInspecting) 
			{
				anim.Play ("Knife Attack 2", 0, 0f);
			}
				
			//Throw grenade when pressing G key
			if (Input.GetKeyDown (KeyCode.G) && !isInspecting) 
			{
				StartCoroutine (GrenadeSpawnDelay ());
				//Play grenade throw animation
				anim.Play("GrenadeThrow", 0, 0.0f);
			}
			*/

			if(Input.GetKeyDown (KeyCode.F))
			{
				if(selfLight.enabled)
					selfLight.enabled = false;
				else
					selfLight.enabled = true;
				
				shootAudioSource.PlayOneShot(clipFlashlight);
			}

			//If out of ammo
			if (currentAmmo == 0) 
			{
				//Show out of ammo text
				currentWeaponText.text = "OUT OF AMMO";
				//Toggle bool
				outOfAmmo = true;
				//Auto reload if true
				if (autoReload == true && !isReloading) 
				{
					StartCoroutine (AutoReload ());
				}
					
				//Set slider back
				anim.SetBool ("Out Of Ammo Slider", true);
				//Increase layer weight for blending to slider back pose
				anim.SetLayerWeight (1, 1.0f);
			} 
			else 
			{
				//When ammo is full, show weapon name again
				currentWeaponText.text = storedWeaponName.ToString ();
				//Toggle bool
				outOfAmmo = false;
				//anim.SetBool ("Out Of Ammo", false);
				anim.SetLayerWeight (1, 0.0f);
			}

			//Shooting 
			if (Input.GetMouseButton (0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && !holstered && canShoot) 
			{
				StartCoroutine(ShootDelayTimer());
				anim.Play ("Fire", 0, 0f);
		
					
				//Remove 1 bullet from ammo
				currentAmmo -= 1;
				UpdateAmmoText();

				shootAudioSource.clip = SoundClips.shootSound;
				shootAudioSource.Play ();

				//Light flash start
				StartCoroutine(MuzzleFlashLight());

				if (!isAiming) //if not aiming
				{
					anim.Play ("Fire", 0, 0f);

					if(enableMuzzleflash)
						muzzleParticles.Emit (1);

					if (enableSparks == true) 
					{
						//Emit random amount of spark particles
						sparkParticles.Emit (Random.Range (minSparkEmission, maxSparkEmission + 1));
					}
				} 
				else //if aiming
				{
					anim.Play ("Aim Fire", 0, 0f);
						
					//If random muzzle is false
					if (!randomMuzzleflash) {
						if(enableMuzzleflash)
							muzzleParticles.Emit (1);
						//If random muzzle is true
					} 
					else if (randomMuzzleflash == true) 
					{
						//Only emit if random value is 1
						if (randomMuzzleflashValue == 1) 
						{
							if (enableSparks == true) 
							{
								//Emit random amount of spark particles
								sparkParticles.Emit (Random.Range (minSparkEmission, maxSparkEmission + 1));
							}
							if (enableMuzzleflash == true) 
							{
								muzzleParticles.Emit (1);
								//Light flash start
								StartCoroutine (MuzzleFlashLight ());
							}
						}
					}
				}
				//add recoil spread here
				Vector3 recoil;
				if(isAiming)
					recoil = new Vector3(Random.Range(-maxAimRecoil, maxAimRecoil), Random.Range(-maxAimRecoil, maxAimRecoil), Random.Range(-maxAimRecoil, maxAimRecoil));
				else
					recoil = new Vector3(Random.Range(-maxRecoil, maxRecoil), Random.Range(-maxRecoil, maxRecoil), Random.Range(-maxRecoil, maxRecoil));

				selfLineRenderer.SetPosition(0, Spawnpoints.bulletSpawnPoint.position);

				RaycastHit hit;
				if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward + recoil, out hit, 100.0f, shootableLayer))
				{
					//add hit calculation here
					if(hit.collider != null)
					{
						if(hit.collider.gameObject.GetComponent<EntityHealth>() != null)
						{
							hit.collider.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);

							if(hit.collider.gameObject.GetComponent<EnemyHealth>() != null || hit.collider.gameObject.GetComponent<ShootableHead>() != null )
							{	
								var q = Quaternion.FromToRotation(Vector3.forward, hit.normal);
								GameObject FXclone = (GameObject)Instantiate(zombieBulletFX, hit.point, q);
							}
							else
							{
								var q = Quaternion.FromToRotation(Vector3.forward, hit.normal);
								GameObject FXclone = (GameObject)Instantiate(woodBulletFX, hit.point, q);
							}

						}
						else
						{
							var q = Quaternion.FromToRotation(Vector3.forward, hit.normal);
							GameObject FXclone = (GameObject)Instantiate(woodBulletFX, hit.point, q);
						}
					}

					selfLineRenderer.SetPosition(1, hit.point);
				}
				else
				{
					selfLineRenderer.SetPosition(1, (Spawnpoints.fakeBulletEndPoint.position + recoil));
				}
				/*
				//Spawn bullet at bullet spawnpoint
				var bullet = (Transform)Instantiate (
					Prefabs.bulletPrefab,
					Spawnpoints.bulletSpawnPoint.transform.position,
					Spawnpoints.bulletSpawnPoint.transform.rotation);

				//Add velocity to the bullet
				bullet.GetComponent<Rigidbody>().velocity = 
				bullet.transform.forward * bulletForce;
				*/

				//Spawn casing prefab at spawnpoint
				Instantiate (Prefabs.casingPrefab, 
					Spawnpoints.casingSpawnPoint.transform.position, 
					Spawnpoints.casingSpawnPoint.transform.rotation);
			}
			else if(Input.GetMouseButtonDown (0) && outOfAmmo && !isReloading && !isInspecting && !isRunning && !holstered && canShoot) 
			{
				shootAudioSource.PlayOneShot(clipGunEmpty);
			}

			/*
			//Inspect weapon when pressing T key
			if (Input.GetKeyDown (KeyCode.T)) 
			{
				anim.SetTrigger ("Inspect");
			}

			//Toggle weapon holster when pressing E key
			if (Input.GetKeyDown (KeyCode.E) && !hasBeenHolstered) 
			{
				holstered = true;

				mainAudioSource.clip = SoundClips.holsterSound;
				mainAudioSource.Play();

				hasBeenHolstered = true;
			} 
			else if (Input.GetKeyDown (KeyCode.E) && hasBeenHolstered) 
			{
				holstered = false;

				mainAudioSource.clip = SoundClips.takeOutSound;
				mainAudioSource.Play ();

				hasBeenHolstered = false;
			}
			*/

			//Holster anim toggle
			if (holstered == true) 
			{
				anim.SetBool ("Holster", true);
			} 
			else 
			{
				anim.SetBool ("Holster", false);
			}

			//Reload 
			if (Input.GetKeyDown (KeyCode.R) && !isReloading && !isInspecting && !holstered && totalAmmo > 0 && currentAmmo < ammo)
			{
				//Reload
				Reload ();

				if (!hasStartedSliderBack) 
				{
					hasStartedSliderBack = true;
					StartCoroutine (HandgunSliderBackDelay());
				}
			}

			/*
			//Walking when pressing down WASD keys
			if (Input.GetKey (KeyCode.W) && !isRunning || 
				Input.GetKey (KeyCode.A) && !isRunning || 
				Input.GetKey (KeyCode.S) && !isRunning || 
				Input.GetKey (KeyCode.D) && !isRunning) 
			{
				anim.SetBool ("Walk", true);
			} else {
				anim.SetBool ("Walk", false);
			}
			*/
			anim.SetBool("Walk", fpsControllerLPFP.IsMoving);

			//Running when pressing down W and Left Shift key
			if (fpsControllerLPFP.CanRun && (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) && !isAiming && !isReloading && fpsControllerLPFP.IsMoving) 
			{
				isRunning = true;
			} else {
				isRunning = false;
			}
			
			//Run anim toggle
			if (isRunning == true) {
				anim.SetBool ("Run", true);
			} else {
				anim.SetBool ("Run", false);
			}
		}
		else
		{
			if(isAiming)
			{
				//When right click is released
				crosshair.color = Color.white;
				gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
					defaultFov,fovSpeed * Time.deltaTime);
				
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, pcDefaultFov, fovSpeed * Time.deltaTime);

				currentMaxSwayAmount = maxSwayAmount;
				isAiming = false;
				soundHasPlayed = false;
		
				anim.SetBool ("Aim", false);
			}

			isRunning = false;
			anim.SetBool ("Run", false);
			anim.SetBool("Walk", fpsControllerLPFP.IsMoving);
		}
	}

	private IEnumerator HandgunSliderBackDelay () {
		//Wait set amount of time
		yield return new WaitForSeconds (sliderBackTimer);
		//Set slider back
		anim.SetBool ("Out Of Ammo Slider", false);
		//Increase layer weight for blending to slider back pose
		anim.SetLayerWeight (1, 0.0f);

		hasStartedSliderBack = false;
	}

	private IEnumerator GrenadeSpawnDelay () {
		//Wait for set amount of time before spawning grenade
		yield return new WaitForSeconds (grenadeSpawnDelay);
		//Spawn grenade prefab at spawnpoint
		Instantiate(Prefabs.grenadePrefab, 
			Spawnpoints.grenadeSpawnPoint.transform.position, 
			Spawnpoints.grenadeSpawnPoint.transform.rotation);
	}

	private IEnumerator AutoReload () {

		if (!hasStartedSliderBack) 
		{
			hasStartedSliderBack = true;

			StartCoroutine (HandgunSliderBackDelay());
		}
		//Wait for set amount of time
		yield return new WaitForSeconds (autoReloadDelay);

		if (outOfAmmo == true) {
			//Play diff anim if out of ammo
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				//Start show bullet delay
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
		UpdateAmmoText();
	}

	//Reload
	private void Reload () {
		
		if (outOfAmmo == true) 
		{
			//Play diff anim if out of ammo
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				//Start show bullet delay
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		else 
		{
			//Play diff anim if ammo left
			anim.Play ("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play ();

			//If reloading when ammo left, show bullet in mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = true;
			}
		}
		//Restore ammo when reloading
		if(currentAmmo > 0)
		{
			totalAmmo += currentAmmo;
			currentAmmo = 0;
		}
		
		if(totalAmmo > ammo)
		{
			currentAmmo = ammo;
			totalAmmo -= ammo;
		}
		else
		{
			currentAmmo = totalAmmo;
			totalAmmo = 0;
		}

		outOfAmmo = false;
		UpdateAmmoText();
	}

	//Enable bullet in mag renderer after set amount of time
	private IEnumerator ShowBulletInMag () {
		//Wait set amount of time before showing bullet in mag
		yield return new WaitForSeconds (showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = true;
	}

	//Show light when shooting, then disable after set amount of time
	private IEnumerator MuzzleFlashLight () 
	{
		selfLineRenderer.enabled = true;
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
		selfLineRenderer.enabled = false;
	}

	private IEnumerator ShootDelayTimer()
	{
		canShoot = false;
		yield return new WaitForSeconds(shootDelay);
		canShoot = true;
	}

	//Check current animation playing
	private void AnimationCheck () 
	{
		//Check if reloading
		//Check both animations
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") || 
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Ammo Left")) 
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}

		//Check if inspecting weapon
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Inspect")) 
		{
			isInspecting = true;
		} 
		else 
		{
			isInspecting = false;
		}
	}

	private void UpdateAmmoText()
	{
		if(!holstered)
			ammoText.text = currentAmmo + " | " + totalAmmo;
	}
}
// ----- Low Poly FPS Pack Free Version -----