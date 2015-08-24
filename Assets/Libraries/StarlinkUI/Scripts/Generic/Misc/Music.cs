using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
	static bool mLoaded = false;
	static float mVolume = 0.3f;

	/// <summary>
	/// Volume affecting music.
	/// </summary>

	static public float volume
	{
		get
		{
			if (!mLoaded)
			{
				mLoaded = true;
				mVolume = PlayerPrefs.GetFloat("Music", 0.3f);
			}
			return mVolume;
		}
		set
		{
			if (mVolume != value)
			{
				mLoaded = true;
				mVolume = value;
				PlayerPrefs.SetFloat("Music", value);
			}
		}
	}

	public AudioClip[] clips;

	AudioSource mAudio;
	float mSilence = 0f;
	float mLastVolume = -1f;
	AudioClip mLastClip = null;

	void Awake ()
	{
		mAudio = GetComponent<AudioSource>();
		if (clips.Length == 0) enabled = false;
		else Play();
	}

	/// <summary>
	/// Monitor for sound volume changes and for music ending.
	/// </summary>

	void Update ()
	{
		if (mAudio != null)
		{
			if (mLastVolume != volume)
			{
				mLastVolume = mVolume;
				mAudio.volume = mVolume;
				mAudio.enabled = mVolume > 0.01f;
			}

			if (mVolume > 0.01f && !mAudio.isPlaying)
			{
				if (mSilence == 0f)
				{
					mSilence = Random.Range(50f, 150f);
				}
				else
				{
					mSilence -= RealTime.deltaTime;

					if (mSilence <= 0f)
					{
						mSilence = 0f;
						Play();
					}
				}
			}
		}
	}

	/// <summary>
	/// Play a random tune.
	/// </summary>

	void Play ()
	{
		if (clips.Length == 1)
		{
			mLastClip = clips[0];
			mAudio.clip = clips[0];
		}
		else
		{
			AudioClip clip = null;

			for (int i = 0; i < 100; ++i)
			{
				int index = Random.Range(0, clips.Length);
				clip = clips[index];
				
				if (clip != mLastClip)
				{
					mLastClip = clip;
					mAudio.clip = clip;
					break;
				}
			}
		}
		mAudio.Play();
	}
}
