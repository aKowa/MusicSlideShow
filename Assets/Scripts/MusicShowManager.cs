using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicShowManager : MonoBehaviour
{
	public Image image;
	public Image imageFade;
	public Text text;
	[Range(0, 0.5f)]
	public float relativeFadeSpeed = 0.1f;
	public AudioSource audioSource;
	public int[] years;
	public float minDuration = 5f;
	public float maxDuration = 10f;
	private int yearID = -1;
	private MusicShow currentMusicShow;
	private float fadeTime;
	private float normSpriteDuration;


	private void Start ()
	{
		yearID = -1;
		SetNextMusicShow();
	}


	private IEnumerator ShowText()
	{
		image.enabled = false;
		imageFade.enabled = false;
		text.text = currentMusicShow.year.ToString();
		fadeTime = currentMusicShow.DurationPerSprite() * relativeFadeSpeed;
		normSpriteDuration = currentMusicShow.DurationPerSprite() - (fadeTime * 2);

		for (float t = 0; t <= 1; t += fadeTime*Time.deltaTime)
		{
			text.color = Color.Lerp(Color.clear, Color.white, t);
			yield return null;
		}

		yield return new WaitForSeconds( normSpriteDuration );

		for (float t = 0; t <= 1; t += fadeTime * Time.deltaTime)
		{
			text.color = Color.Lerp ( Color.white, Color.clear, t );
			yield return null;
		}
		imageFade.enabled = true;
		StartCoroutine (ShowSprites());
	}


	private IEnumerator ShowSprites()
	{
		var ti = Time.time;
		var sprite = currentMusicShow.GetNextSprite();
		if ( sprite == null ) yield break;
		imageFade.sprite = sprite;

		for (float t=0; t <= 1; t += fadeTime * Time.deltaTime)
		{
			image.color = Color.Lerp(Color.white, Color.clear, t);
			imageFade.color = Color.Lerp(Color.clear, Color.white, t);
			yield return null;
		}
		image.enabled = true;
		image.sprite = sprite;
		image.color = Color.white;
		imageFade.color = Color.clear;
		yield return new WaitForSeconds( normSpriteDuration );
		ti -= Time.time;
		Debug.Log("Time needed: " + ti);
		StartCoroutine( ShowSprites() );
	}


	private IEnumerator Play()
	{
		var clip = currentMusicShow.GetNextClip ();

		if (clip == null)
		{
			SetNextMusicShow();
			yield break;
		}

		audioSource.clip = clip;
		audioSource.Play();
		yield return new WaitForSeconds ( audioSource.clip.length );
		StartCoroutine( Play() );
	}


	public void SetNextMusicShow()
	{
		++yearID;
		if (yearID < years.Length)
		{
			var sprites = (Sprite[])Resources.LoadAll<Sprite> ( years[yearID].ToString () );
			var clips = (AudioClip[])Resources.LoadAll<AudioClip> ( years[yearID].ToString () );
			currentMusicShow = new MusicShow ( clips, sprites, years[yearID] );

			StopAllCoroutines ();
			StartCoroutine ( Play () );
			StartCoroutine ( ShowText() );

			Debug.Log ( "Year: " + years[yearID] + "  Duration: " + currentMusicShow.Duration + "s" + "  Duration per sprite: " + currentMusicShow.DurationPerSprite() + "s" );
			if (currentMusicShow.DurationPerSprite() < minDuration)
			{
				Debug.LogError("Duration per sprite too low. Use less sprites or longer music!");
			}
			else if ( currentMusicShow.DurationPerSprite() > maxDuration )
			{
				Debug.LogError ( "Duration per sprite too high. Use more sprites or shorten music!" );
			}
		}
		else
		{
			OnEnd();
		}
	}

	private void OnEnd()
	{
		Debug.Log("Reload");
		SceneManager.LoadScene(0);
	}
}
