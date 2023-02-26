using System;
using AnimationOrTween;
using UnityEngine;

public abstract class UITweener : IgnoreTimeScale
{
	public enum Method
	{
		Linear = 0,
		EaseIn = 1,
		EaseOut = 2,
		EaseInOut = 3
	}

	public enum Style
	{
		Once = 0,
		Loop = 1,
		PingPong = 2
	}

	public Method method;

	public Style style;

	public float delay;

	public float duration = 1f;

	public bool steeperCurves;

	public int tweenGroup;

	public GameObject eventReceiver;

	public string callWhenFinished;

	private float mStartTime;

	private float mDuration;

	private float mAmountPerDelta = 1f;

	private float mFactor;

	public float amountPerDelta
	{
		get
		{
			if (mDuration != duration)
			{
				mDuration = duration;
				mAmountPerDelta = Mathf.Abs((!(duration > 0f)) ? 1000f : (1f / duration));
			}
			return mAmountPerDelta;
		}
	}

	public float tweenFactor
	{
		get
		{
			return mFactor;
		}
	}

	public Direction direction
	{
		get
		{
			return (!(mAmountPerDelta < 0f)) ? Direction.Forward : Direction.Reverse;
		}
	}

	private void Start()
	{
		mStartTime = Time.realtimeSinceStartup + delay;
		Update();
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (Time.realtimeSinceStartup < mStartTime)
		{
			return;
		}
		mFactor += amountPerDelta * num;
		if (style == Style.Loop)
		{
			if (mFactor > 1f)
			{
				mFactor -= Mathf.Floor(mFactor);
			}
		}
		else if (style == Style.PingPong)
		{
			if (mFactor > 1f)
			{
				mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
				mAmountPerDelta = 0f - mAmountPerDelta;
			}
			else if (mFactor < 0f)
			{
				mFactor = 0f - mFactor;
				mFactor -= Mathf.Floor(mFactor);
				mAmountPerDelta = 0f - mAmountPerDelta;
			}
		}
		Sample(mFactor);
		if (style != 0 || (!(mFactor > 1f) && !(mFactor < 0f)))
		{
			return;
		}
		mFactor = Mathf.Clamp01(mFactor);
		if (string.IsNullOrEmpty(callWhenFinished))
		{
			base.enabled = false;
			return;
		}
		if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
		{
			eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
		}
		if ((mFactor == 1f && mAmountPerDelta > 0f) || (mFactor == 0f && mAmountPerDelta < 0f))
		{
			base.enabled = false;
		}
	}

	public void Sample(float factor)
	{
		float num = Mathf.Clamp01(factor);
		if (method == Method.EaseIn)
		{
			num = 1f - Mathf.Sin((float)Math.PI / 2f * (1f - num));
			if (steeperCurves)
			{
				num *= num;
			}
		}
		else if (method == Method.EaseOut)
		{
			num = Mathf.Sin((float)Math.PI / 2f * num);
			if (steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (method == Method.EaseInOut)
		{
			num -= Mathf.Sin(num * ((float)Math.PI * 2f)) / ((float)Math.PI * 2f);
			if (steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		OnUpdate(num);
	}

	public void Play(bool forward)
	{
		mAmountPerDelta = Mathf.Abs(amountPerDelta);
		if (!forward)
		{
			mAmountPerDelta = 0f - mAmountPerDelta;
		}
		base.enabled = true;
	}

	[Obsolete("Use Tweener.Play instead")]
	public void Animate(bool forward)
	{
		Play(forward);
	}

	public void Reset()
	{
		mFactor = ((!(mAmountPerDelta < 0f)) ? 0f : 1f);
	}

	public void Toggle()
	{
		if (mFactor > 0f)
		{
			mAmountPerDelta = 0f - amountPerDelta;
		}
		else
		{
			mAmountPerDelta = Mathf.Abs(amountPerDelta);
		}
		base.enabled = true;
	}

	protected abstract void OnUpdate(float factor);

	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T val = go.GetComponent<T>();
		if ((UnityEngine.Object)val == (UnityEngine.Object)null)
		{
			val = go.AddComponent<T>();
		}
		val.duration = duration;
		val.mFactor = 0f;
		val.style = Style.Once;
		val.enabled = true;
		if (duration <= 0f)
		{
			val.Sample(1f);
			val.enabled = false;
		}
		return val;
	}
}
