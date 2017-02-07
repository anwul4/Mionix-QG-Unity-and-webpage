using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class txtStreamReader : MonoBehaviour {
	
	private string filePath = "C:\\Users\\wulff\\Downloads\\";
	private string fileName = "StreamMionixData.txt";
	private float readTimeOut = 0.0f; 
	private StreamReader reader;
	private double hr; 
	private double gsr;
	private double maxHr;
	private double avgHr;
	private double td;
	private double rawHr;
	private double rawTouch;
	private double totTime;
	private double totDist;
	private double totScroll;
	private double totClick;
	private double stTime;
	private double stDist;
	private double stScroll; 
	private double stClick;
	private double mMoveSpeed;
	private double mAvgSpeed;
	private double mMaxSpeed;
	private double clickRate;
	private double clickRateAvg;
	private double clickRateMAx; 
	private double scrollRate;
	private double scrollRateAvg; 
	private double scrollRateMax;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void StartStream()
	{
		readTimeOut += Time.deltaTime; 

		try
		{
			using(reader = new StreamReader(filePath+fileName))
			{
				string read; 
				if(readTimeOut > 0.2f)
				{
					while((read = reader.ReadLine())!= null)
					{
						if(read.Contains("Live Heart Rate:,"))
						{
							string[] hrString = read.Split(',');
							hr = double.Parse(hrString[1]);
						}
						if(read.Contains("GSR:,"))
						{
							string[] gsrString = read.Split(',');
							gsr = double.Parse(gsrString[1]);
						}
						if(read.Contains("Max Heart Rate:, "))
						{
							string[] mHrString = read.Split(',');
							maxHr = double.Parse(mHrString[1]);
						}
						if(read.Contains("Average Heart Rate:,"))
						{
							string[] avgHrString = read.Split(',');
							avgHr = double.Parse(avgHrString[1]);
						}
						if(read.Contains("Time since Last Sample"))
						{
							string[] tds = read.Split(',');
							td = double.Parse(tds[1]);
						}
						if(read.Contains("Raw Heart Rate"))
						{
							string[] str = read.Split(',');
							rawHr = double.Parse(str[1]);
						}
						if(read.Contains("Raw Touch"))
						{
							string[] str = read.Split(',');
							rawTouch = double.Parse(str[1]);
						}
						if(read.Contains("Time Since Factory"))
						{
							string[] str = read.Split(',');
							totTime = double.Parse(str[1]);
						}
						if(read.Contains("Distance Since Factory"))
						{
							string[] str = read.Split(',');
							totDist = double.Parse(str[1]);
						}
						if(read.Contains("Number of step scrolled"))
						{
							string[] str = read.Split(',');
							totScroll = double.Parse(str[1]);
						}
						if(read.Contains("Number of Clicks since"))
						{
							string[] str = read.Split(',');
							totClick = double.Parse(str[1]);
						}
						if(read.Contains("Streak Time"))
						{
							string[] str = read.Split(',');
							stTime = double.Parse(str[1]);
						}
						if(read.Contains("Streak Distance"))
						{
							string[] str = read.Split(',');
							stDist = double.Parse(str[1]);
						}
						if(read.Contains("Streak Scrolls"))
						{
							string[] str = read.Split(',');
							stScroll = double.Parse(str[1]);
						}
						if(read.Contains("Streak Clicks"))
						{
							string[] str = read.Split(',');
							stClick = double.Parse(str[1]);
						}
						if(read.Contains("Mouse Move"))
						{
							string[] str = read.Split(',');
							mMoveSpeed = double.Parse(str[1]);
						}
						if(read.Contains("Mouse Avg"))
						{
							string[] str = read.Split(',');
							mAvgSpeed = double.Parse(str[1]);
						}
						if(read.Contains("Mouse Max"))
						{
							string[] str = read.Split(',');
							mMaxSpeed = double.Parse(str[1]);
						}
						if(read.Contains("Click Rate Live"))
						{
							string[] str = read.Split(',');
							clickRate = double.Parse(str[1]);
						}
						if(read.Contains("Click Rate Avg"))
						{
							string[] str = read.Split(',');
							clickRateAvg = double.Parse(str[1]);
						}
						if(read.Contains("Click Rate Max"))
						{
							string[] str = read.Split(',');
							clickRateMAx = double.Parse(str[1]);
						}
						if(read.Contains("Scroll Rate Live"))
						{
							string[] str = read.Split(',');
							scrollRate = double.Parse(str[1]);
						}
						if(read.Contains("Scroll Rate Avg"))
						{
							string[] str = read.Split(',');
							scrollRateAvg = double.Parse(str[1]);
						}
						if(read.Contains("Scroll Rate Max"))
						{
							string[] str = read.Split(',');
							scrollRateMax = double.Parse(str[1]);
						}
					}
					readTimeOut = 0.0f; 
				}
			}
		}
		catch (System.Exception e) {
			Debug.Log ("the file could not be read");
			Debug.Log (e.Message); 
		}
	}
	public double GetLiveHr()
	{
		return hr;
	}
	public double GetAvgHr()
	{
		return avgHr;
	}
	public double GetMaxHr()
	{
		return maxHr;
	}
	public double GetLivGsr()
	{
		return gsr;
	}
	public double GetSampleDeltaTime()
	{
		return td; 
	}
	public double GetRawHr()
	{
		return rawHr;
	}
	public double GetRawTouch()
	{
		return rawTouch;
	}
	public double GetTotalTime()
	{
		return totTime;
	}
	public double GetTotalDistance()
	{
		return totDist;
	}
	public double GetTotalScrolls()
	{
		return totScroll;
	}
	public double GetTotalClicks()
	{
		return totClick;
	}
	public double GetStreakTime()
	{
		return stTime;
	}
	public double GetStreakDistance()
	{
		return stDist;
	}
	public double GetStreakScrolls()
	{
		return stScroll;
	}
	public double GetStreakClicks()
	{
		return stClick;
	}
	public double GetMouseMoveSpeed()
	{
		return mMoveSpeed;
	}
	public double GetMouseAvgSpeed()
	{
		return mAvgSpeed;
	}
	public double GetMouseMaxSpeed()
	{
		return mMaxSpeed;
	}
	public double GetClickRate()
	{
		return clickRate; 
	}
	public double GetClickRateAvg()
	{
		return clickRateAvg;
	}
	public double GetClickRateMax()
	{
		return clickRateMAx;
	}
	public double GetScrollRate()
	{
		return scrollRate;
	}
	public double GetScrollRateAvg()
	{
		return scrollRateAvg;
	}
	public double GetScrollRateMax()
	{
		return scrollRateMax;
	}
}



