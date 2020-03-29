using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggers : MonoBehaviour
{

	//[SerializeField] AudioSource Water;
	public AudioSource Water;
	public AudioSource Wind;
	public AudioSource Birds;

	Player player;
	public float volume;

	public enum SoundStates
	{
		WATER,
		WIND,
		BIRD,
		NOTHING,

	}

	public SoundStates state;

	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();


		state = SoundStates.NOTHING;

		Water.Play();
		Birds.Play();
		MuteSound();
		
	}

	// Update is called once per frame
	void Update()
	{
		checkState();

		switch (state)

		{
			case SoundStates.WATER:
				{
					
					Water.volume = Mathf.Lerp(Water.volume, 0.8f, Time.unscaledDeltaTime / 0.2f);
					//Water.volume = 0.8f;
					break;
				}
			case SoundStates.WIND:
				{
					Water.volume = Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
					//  Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
					break;
				}
			case SoundStates.BIRD:
				{	
					Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
					//  Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
					break;
				}
			case SoundStates.NOTHING:
				{
					Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
					//Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
					break;
				}
		}
	}



	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Water")
		{
			Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
		}

		if (other.tag == "SoundWind")
		{
			Mathf.Lerp(Wind.volume, 0.0f, Time.unscaledDeltaTime / 1f);
		}

		if (other.tag == "SoundBird")
		{
			Mathf.Lerp(Birds.volume, 0.0f, Time.unscaledDeltaTime / 1f);
		}


	}

	void volumeUpdate()
	{

		Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
		Mathf.Lerp(Wind.volume, 0.0f, Time.unscaledDeltaTime / 1f);
		Mathf.Lerp(Birds.volume, 0.0f, Time.unscaledDeltaTime / 1f);

	}

	void MuteSound()
	{
		Birds.volume = 0.0f;
		Water.volume = 0.0f;
		Wind.volume = 0.0f;
	}

	void checkState()
	{
		if (player.WaterSounds == true)
		{
			state = SoundStates.WATER;
		}

		if (player.BirdSounds == true)
		{
			state = SoundStates.BIRD;
		}
	}

}