using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using System.Text;

public class HeartRateManager : MonoBehaviour {
	private txtStreamReader stream;
	private double[] biometrics = new double[4]; 
	private ArrayList hrArrList = new ArrayList(); 
	private ArrayList avgHrArrList = new ArrayList(); 
	private ArrayList maxHrArrList = new ArrayList(); 
	private ArrayList gsrArrList = new ArrayList();
	private ArrayList timeList = new ArrayList(); 

	private ArrayList hrCSArrList = new ArrayList(); 
	private ArrayList avgHrCSArrList = new ArrayList(); 
	private ArrayList maxHrCSArrList = new ArrayList(); 
	private ArrayList gsrCSArrList = new ArrayList();
	private ArrayList timeCSList = new ArrayList(); 

	private ArrayList hrPDArrList = new ArrayList(); 
	private ArrayList avgHrPDArrList = new ArrayList(); 
	private ArrayList maxHrPDArrList = new ArrayList(); 
	private ArrayList gsrPDArrList = new ArrayList();

	private ArrayList triggers = new ArrayList();
	private ArrayList triggersWithBaseLine = new ArrayList();
	private float time = 0.0f;
	private UDPDoubleArrSend udpSender;
	private UDPSend triggerSender;
	private int participantID = 1; 
	private string directoryPath;
	private double hrBase = 0.0;
	private double avghrBase = 0.0;
	private double maxhrBase = 0.0;
	private double gsrBase = 0.0;
	private bool captureBaseline = true;
	private double baselineTime = 10.0; 
	private bool printTxts = true; 
	private bool triggerEnabled = true; 
	// Use this for initialization
	void Start () {
		udpSender = GetComponent<UDPDoubleArrSend> ();
		triggerSender = GetComponent<UDPSend> ();
		stream = GetComponent<txtStreamReader> ();

		while(Directory.Exists("E:/HrTestData/" + participantID))
		{
			participantID += 1;
		}
		Directory.CreateDirectory ("E:/HrTestData/" + participantID);
		directoryPath = "E:/HrTestData/" + participantID + "/";
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime; 
		stream.StartStream ();
		biometrics [0] = stream.GetLiveHr ();
		biometrics [1] = stream.GetAvgHr ();
		biometrics [2] = stream.GetMaxHr ();
		biometrics [3] = stream.GetLivGsr ();
		udpSender.sendDoubleArr (biometrics); 

		timeList.Add (time); 
		hrArrList.Add (stream.GetLiveHr ());
		avgHrArrList.Add (stream.GetAvgHr ());
		maxHrArrList.Add (stream.GetMaxHr ());
		gsrArrList.Add (stream.GetLivGsr ());
		triggers.Add (0);

		if (time > 2.0 && time < 2.1 && triggerEnabled) {
			SetTrigger (2);
			triggerEnabled = false;
		}
		if (time > 2.1 && time < 2.2) {
			triggerEnabled = true;
		}

		if (time > baselineTime && captureBaseline == true) {
			captureBaseline = false;
			hrBase = MeasureBaseLine (hrArrList);
			avghrBase = MeasureBaseLine (avgHrArrList);
			maxhrBase = MeasureBaseLine (maxHrArrList);
			gsrBase = MeasureBaseLine (gsrArrList);
			time = 0.0f;
			Debug.Log ("Baseline Found");
		}
		if (hrBase != 0.0) {
			hrCSArrList.Add(LiveChangeScore(stream.GetLiveHr(),hrBase));
			avgHrCSArrList.Add (LiveChangeScore (stream.GetAvgHr (), avghrBase));
			maxHrCSArrList.Add (LiveChangeScore (stream.GetMaxHr (), maxhrBase));
			gsrCSArrList.Add (LiveChangeScore (stream.GetLivGsr (), gsrBase));
			timeCSList.Add (time); 

			hrPDArrList.Add (LivePercentageDiffFromBase (stream.GetLiveHr (), hrBase));
			avgHrPDArrList.Add (LivePercentageDiffFromBase (stream.GetAvgHr (), avghrBase));
			maxHrPDArrList.Add (LivePercentageDiffFromBase (stream.GetMaxHr (), maxhrBase));
			gsrPDArrList.Add (LivePercentageDiffFromBase (stream.GetLivGsr (), gsrBase));

			if (time > 3.0 && time < 3.1 && triggerEnabled) {
				SetTrigger (3);
				triggerEnabled = false;
			}
			if (time > 3.1 && time < 3.2) {
				triggerEnabled = true;
			}
			if (time > 4.0 && time < 4.1 && triggerEnabled) {
				SetTrigger (4);
				triggerEnabled = false;
			}
			if (time > 4.1 && time < 4.2) {
				triggerEnabled = true;
			}

			if (time > 20 && printTxts == true) {
				printTxts = false; 
				string filenameRaw = "part" + participantID + "bioMeasure.txt"; 
				string filenameCS = "part" + participantID + "ChangeScore.txt"; 
				string filenamePD = "part" + participantID + "PercentageDifferenceFromBase.txt"; 
				CreateTxtContent (timeList, hrArrList, avgHrArrList, maxHrArrList, gsrArrList, filenameRaw);
				CreateTxtContent (timeCSList, hrCSArrList, avgHrCSArrList, maxHrCSArrList, gsrCSArrList, filenameCS); 
				CreateTxtContent (timeCSList, hrPDArrList, avgHrPDArrList, maxHrPDArrList, gsrPDArrList, filenamePD); 
				Debug.Log ("Text Files Printed"); 
			}

		}

	}
	private void CreateTxtContent(ArrayList time, ArrayList hr, ArrayList avghr, ArrayList maxhr, ArrayList gsr, string filename)
	{
		string labels = "Time,HeartRate,AverageHeartRate,MaximumHeartRate,GSR \r\n";
		string info = labels; 
		for (int i = 0; i < hr.Count; i++) {
			info += (time[i]+","+hr[i]+","+avghr[i]+","+maxhr[i]+","+gsr[i]+"\r\n");
		}
		CreateTxtFile (info, filename);
	}
	private void CreateTxtFile(string txt, string fileName)
	{
		ASCIIEncoding asciiCoding = new ASCIIEncoding ();
		UnicodeEncoding uniencoding = new UnicodeEncoding();


		byte[] result = asciiCoding.GetBytes(txt);

		using (FileStream SourceStream = File.Open(directoryPath+fileName, FileMode.OpenOrCreate))
		{
			SourceStream.Seek(0, SeekOrigin.End);
			//SourceStream.WriteAsync(result, 0, result.Length);
			SourceStream.Write(result, 0, result.Length);
			SourceStream.Close();
		}
	}
	public double MeasureBaseLine(ArrayList bioMeasure)
	{
		double baseline = 0.0;
		double sum = 0.0;
		for (int i = 0; i < bioMeasure.Count; i++) {
			string biom = bioMeasure [i].ToString ();
			sum += double.Parse(biom);
		}
		baseline = sum / bioMeasure.Count; 
		return baseline;  
	}
	public double LiveChangeScore(double bioMeasure, double baseline)
	{
		double changeScore = bioMeasure - baseline; 
		return changeScore; 
	}
	public double LivePercentageDiffFromBase(double bioMeasure, double baseline)
	{
		double percentageDiff = (bioMeasure - baseline)/(baseline/100); 
		return percentageDiff; 
	}
	public void SetTrigger(int triggerNo )
	{
		triggers.Add (triggerNo);
		triggerSender.sendInt (triggerNo);
		if (hrBase != 0.0) {
			triggersWithBaseLine.Add (triggerNo); 
		}
	}
}
