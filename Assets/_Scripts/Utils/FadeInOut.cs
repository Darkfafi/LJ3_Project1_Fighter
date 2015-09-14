using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour {

	private bool _fading = false;

	private float _fadeSpeed = 0.005f;
	private float _fadeTargetValue = 0;

	private float _timeAskedForTimerFadeOut = 0;
	private float _timerFadeOut = float.NaN;
	private float _fadeSpeedTimer = 0.005f;
	private float _fadeTargetValueTimer = 0;


	public delegate void FloatInfo(float value);

	public event FloatInfo OnFadeStart;
	public event FloatInfo OnFade;
	public event FloatInfo OnFadeEnd;

	public float GetAlpha(){
		float returnAlpha = 1;
		if(GetComponent<SpriteRenderer>() != null){
			returnAlpha = GetComponent<SpriteRenderer>().color.a;
		}else if(GetComponent<CanvasRenderer>() != null){
			returnAlpha = GetComponent<CanvasRenderer>().GetColor().a;
		}
		return returnAlpha;
	}

	public void SetAlpha(float alphaValue){
		Color color = new Color ();
		if(GetComponent<SpriteRenderer>() != null){
			color = GetComponent<SpriteRenderer>().color;
			color.a = alphaValue;
			GetComponent<SpriteRenderer>().color = color;

			for(int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject Go = gameObject.transform.GetChild(i).gameObject;
				if(Go.GetComponent<SpriteRenderer>() != null){
					color = Go.GetComponent<SpriteRenderer>().color;
					color.a = alphaValue;
					Go.GetComponent<SpriteRenderer>().color = color;
				}
			}
		}else if(GetComponent<CanvasRenderer>() != null){
			color = GetComponent<CanvasRenderer>().GetColor();
			color.a = alphaValue;
			GetComponent<CanvasRenderer>().SetColor(color);

			for(int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject Go = gameObject.transform.GetChild(i).gameObject;
				if(Go.GetComponent<CanvasRenderer>() != null){
					color = Go.GetComponent<CanvasRenderer>().GetColor();
					color.a = alphaValue;
					Go.GetComponent<CanvasRenderer>().SetColor(color);
				}
			}
		}
	}

	public void Fade(float fadeToValue,float fadeSpeed = 0.005f){
		_fadeTargetValue = fadeToValue;
		_fadeSpeed = fadeSpeed;
		_fading = true;
		if(OnFadeStart != null){
			OnFadeStart(GetAlpha());
		}
	}
	public void FadeAfterTime(float timeInSeconds,float fadeToValue,float fadeSpeed = 0.005f){
		_timeAskedForTimerFadeOut = Time.time;
		_fadeTargetValueTimer = fadeToValue;
		_fadeSpeedTimer = fadeSpeed;
		_timerFadeOut = timeInSeconds;
	}
	// Update is called once per frame
	void Update () {

		if (!float.IsNaN(_timerFadeOut)) {
			if(Time.time > _timeAskedForTimerFadeOut + _timerFadeOut){
				_timerFadeOut = float.NaN;
				Fade(_fadeTargetValueTimer,_fadeSpeedTimer);
			}
		}

		if(_fading){
			int dir = 0;
			float alpha = GetAlpha();

			if(_fadeTargetValue < alpha){
				dir = -1;
			}else if(_fadeTargetValue > alpha){
				dir = 1;
			}
			alpha += dir * _fadeSpeed;
			SetAlpha(alpha);

			if(OnFade != null){
				OnFade(GetAlpha());
			}

			if(alpha >= _fadeTargetValue && dir == 1 || alpha <= _fadeTargetValue && dir == -1){
				_fading = false;
				if(OnFadeEnd != null){
					OnFadeEnd(GetAlpha());
				}
			}
		}
	}
}
