using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFollow : MonoBehaviour
{
	public static CameraFollow current;

    private Transform playerGO;
	private PostProcessVolume volume;
	private Vignette vignette;
	private ChromaticAberration chromaticAberration;
	private Bloom bloom;

	private float shakeDuration;
	private float shakeMagnitude = 1f;
	private float dampingSpeed = 2f;

    public Vector3 offset;
    public float smoothFactor;

	private void Awake()
	{
		current = this;
	}

	private void Start()
	{
		volume = GetComponent<PostProcessVolume>();

		volume.profile.TryGetSettings(out vignette);
		volume.profile.TryGetSettings(out chromaticAberration);
		volume.profile.TryGetSettings(out bloom);

		playerGO = GameObject.FindGameObjectWithTag("Player").transform;

		GameManager.current.onEnemyDeath += TriggerShake;
		GameManager.current.onPlayerDeath += BloomOut;
		GameManager.current.onExitDoorEntered += BloomOut;
	}

	void Update()
	{
		Follow();

		if (shakeDuration > 0)
		{
			transform.position += Random.insideUnitSphere * shakeMagnitude;

			shakeDuration -= Time.deltaTime * dampingSpeed;
		}
	}

	private void Follow()
	{
		if (playerGO != null)
		{
			Vector3 targetPosition = playerGO.position + offset;
			Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

			transform.position = smoothPosition;
		}
	}

	public void TriggerShake()
	{
		shakeDuration = 0.25f; ;
	}

	public void TriggerShake2(float amount)
	{
		shakeDuration = amount;
	}

	public void ChangeVignetteValue(float amount)
	{
		vignette.intensity.value = amount;
	}

	public void ChangeChromaticValue(float amount)
	{
		chromaticAberration.intensity.value = amount;
	}

	public void BloomOut()
	{
		StartCoroutine(BloomOut_Coroutine());
	}

	private IEnumerator BloomOut_Coroutine()
	{
		float startTime = Time.time;

		while (Time.time < startTime + 5f)
		{
			bloom.diffusion.value += Time.deltaTime * 5f;
			bloom.intensity.value += Time.deltaTime * 5f;
			yield return null;
		}
		yield break;
	}

}
