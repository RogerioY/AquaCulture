using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calender : MonoBehaviour {
    public int monthNumber;
    public int date;
    public Button surfaceButton;
    public string monthName;
    public Text calenderDisplay;
	// Use this for initialization
	void Start () {
        monthNumber = 1;
        date = 1;
        Button advanceDayButton = surfaceButton.GetComponent<Button>();
        advanceDayButton.onClick.AddListener(advanceDay);
        monthName = "JAN"; calenderDisplay.text = "" + date + monthName;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void advanceDay()
    {
        date++;
        if (date >= 31)
        {
            date = 1;
            monthNumber++;
        }
        monthNumber %= 12;
        switch (monthNumber)
        {
            case 1: monthName = "JAN"; calenderDisplay.text = "" + date + monthName;  break;
            case 2: monthName = "FEB"; calenderDisplay.text = "" + date + monthName;  break;
            case 3: monthName = "MAR"; calenderDisplay.text = "" + date + monthName;  break;
            case 4: monthName = "APR"; calenderDisplay.text = "" + date + monthName;  break;
            case 5: monthName = "MAY"; calenderDisplay.text = "" + date + monthName;  break;
            case 6: monthName = "JUN"; calenderDisplay.text = "" + date + monthName;  break;
            case 7: monthName = "JUL"; calenderDisplay.text = "" + date + monthName;  break;
            case 8: monthName = "AUG"; calenderDisplay.text = "" + date + monthName;  break;
            case 9: monthName = "SEP"; calenderDisplay.text = "" + date + monthName;  break;
            case 10: monthName = "OCT"; calenderDisplay.text = "" + date + monthName; break;
            case 11: monthName = "NOV"; calenderDisplay.text = "" + date + monthName; break;
            case 12: monthName = "DEC"; calenderDisplay.text = "" + date + monthName; break;
            default: monthName = "JAN"; calenderDisplay.text = "" + date + monthName; break;



        }


    }

}

