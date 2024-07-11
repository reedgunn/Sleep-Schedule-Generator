using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class testing : MonoBehaviour {

    public GameObject MonthOptions;
    public GameObject DayOptions;
    public GameObject YearOptions;
    public GameObject FallAsleepDuration;
    public GameObject WakeUpTimeOptions;
    public GameObject resultsDisplay;
    public string usersBedtime;
    public string usersWakeUpTime;
    public float usersAgeYrs;
    public static int currentYear = Int32.Parse(DateTime.Now.ToString("MM/dd/yyyy").Substring(6, 4));
    public static int usersBirthdayMonth = 1;
    public static int usersBirthdayDay = 1;
    public static int usersBirthdayYear = Int32.Parse(DateTime.Now.ToString("MM/dd/yyyy").Substring(6, 4));
    public static int usersFallingAsleepDuration = 10;
    public static int usersWakeUpTimeMinutes = 0;
    public int usersBedtimeMinutes;

    bool isLeapYear(int year) { return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0; }

    public void updateusersAgeYrs() {
        string curDate = DateTime.Now.ToString("MM/dd/yyyy");
        int curMonth = Int32.Parse(curDate.Substring(0, 2));
        int curDay = Int32.Parse(curDate.Substring(3, 2));
        List<int> curDate_daysPerMonth = new List<int>{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        if (isLeapYear(currentYear)) curDate_daysPerMonth[1] = 29;
        int curDate_daysFromMonths = 0;
        for (int i = 0; i <= curMonth - 2; i++) curDate_daysFromMonths += curDate_daysPerMonth[i];
        List<int> birthday_daysPerMonth = new List<int>{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        if (isLeapYear(currentlySelectedYear())) birthday_daysPerMonth[1] = 29;
        int birthday_daysFromMonths = 0;
        for (int i = 0; i <= currentlySelectedMonth() - 2; i++) birthday_daysFromMonths += birthday_daysPerMonth[i];
        int daysPassedFromMonths = curDate_daysFromMonths - birthday_daysFromMonths;
        int daysPassedFromYears = 0;
        for (int i = currentlySelectedYear(); i < currentYear; i++) {
            if (isLeapYear(i)) daysPassedFromYears += 366;
            else daysPassedFromYears += 365;
        }
        int daysPassedFromDays = curDay - currentlySelectedDay();
        int ageDays = daysPassedFromMonths + daysPassedFromYears + daysPassedFromDays;
        float ageInYears = ageDays / 365.2422f;
        usersAgeYrs = ageInYears;
    }

    int currentlySelectedYear() { return currentYear - YearOptions.GetComponent<TMPro.TMP_Dropdown>().value; }
    int currentlySelectedDay() { return DayOptions.GetComponent<TMPro.TMP_Dropdown>().value + 1; }
    int currentlySelectedMonth() { return MonthOptions.GetComponent<TMPro.TMP_Dropdown>().value + 1; }
    int currentlySelectedFallingAsleepDuration() { return 10 + FallAsleepDuration.GetComponent<TMPro.TMP_Dropdown>().value * 5; }
    int currentlySelectedWakeUpTime() { return WakeUpTimeOptions.GetComponent<TMPro.TMP_Dropdown>().value * 5; }

    public void userEditsBirthdayMonth() {
        // Change day dropdown options if number of days in months differ, reselect same day option if possible,
        // update users age, and update displayed results.
        List<int> daysPerMonth = new List<int>{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        if (daysPerMonth[usersBirthdayMonth - 1] != daysPerMonth[currentlySelectedMonth() - 1]) {
            if (isLeapYear(currentlySelectedYear())) daysPerMonth[1] = 29;
            DayOptions.GetComponent<TMPro.TMP_Dropdown>().ClearOptions();
            List<string> dayOptions = new List<string>();
            for (int i = 1; i <= daysPerMonth[currentlySelectedMonth() - 1]; i++) dayOptions.Add(i.ToString());
            DayOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(dayOptions);
            if (usersBirthdayDay <= dayOptions.Count) DayOptions.GetComponent<TMPro.TMP_Dropdown>().value = usersBirthdayDay - 1;
            else usersBirthdayDay = 1;
        }
        usersBirthdayMonth = currentlySelectedMonth();
        updateusersAgeYrs();
        updateDisplayedResults();
    }

    public void goToExplanationPage() { SceneManager.LoadScene(1); }

    string timeInMinToString(int timeInMin) {
        if (timeInMin < 0) timeInMin += 24 * 60;
        string timeAsString;
        if (timeInMin / 60 < 1) timeAsString = "12:" + (timeInMin % 60).ToString("00") + "am";
        else if (timeInMin / 60 >= 12 && timeInMin / 60 < 13) timeAsString = "12:" + (timeInMin % 60).ToString("00") + "pm";
        else if (timeInMin / 60 >= 13) timeAsString = (timeInMin / 60 - 12).ToString() + ":" + (timeInMin % 60).ToString("00") + "pm";
        else timeAsString = (timeInMin / 60).ToString() + ":" + (timeInMin % 60).ToString("00") + "am";
        return timeAsString;
    }

    public void updateDisplayedResults() {
        usersWakeUpTime = timeInMinToString(usersWakeUpTimeMinutes);
        usersBedtimeMinutes = usersWakeUpTimeMinutes - sleepNeededMinutes() - usersFallingAsleepDuration;
        usersBedtime = timeInMinToString(usersBedtimeMinutes);
        resultsDisplay.GetComponent<TMPro.TMP_Text>().SetText("Your daily sleep schedule:\nBedtime = " + usersBedtime + "\nWake-up time = " + usersWakeUpTime);
    }

    int sleepNeededMinutes() {
        float sleepNeededHrs = -1.00000f;
        if      (usersAgeYrs >= 0.00000f  && usersAgeYrs < 0.66667f)   sleepNeededHrs = (-4.00000f * usersAgeYrs) + 16.16667f;
        else if (usersAgeYrs >= 0.66667f  && usersAgeYrs < 2.00000f)   sleepNeededHrs = (-0.75000f * usersAgeYrs) + 14.00000f;
        else if (usersAgeYrs >= 2.00000f  && usersAgeYrs < 4.50000f)   sleepNeededHrs = (-0.40000f * usersAgeYrs) + 13.33333f;
        else if (usersAgeYrs >= 4.50000f  && usersAgeYrs < 10.00000f)  sleepNeededHrs = (-0.27273f * usersAgeYrs) + 12.72727f;
        else if (usersAgeYrs >= 10.00000f && usersAgeYrs < 16.00000f)  sleepNeededHrs = (-0.16667f * usersAgeYrs) + 11.66667f;
        else if (usersAgeYrs >= 16.00000f && usersAgeYrs < 22.00000f)  sleepNeededHrs = (-0.16667f * usersAgeYrs) + 11.66667f;
        else if (usersAgeYrs >= 22.00000f && usersAgeYrs < 45.50000f)  sleepNeededHrs = (-0.00000f * usersAgeYrs) + 8.00000f;
        else if (usersAgeYrs >= 45.50000f && usersAgeYrs < 122.44901f) sleepNeededHrs = (-0.01923f * usersAgeYrs) + 8.87500f;
        int sleepNeededMins = (int) (MathF.Round((sleepNeededHrs * 60.00000f) / 5.00000f) * 5.00000f);
        return sleepNeededMins;
    }

    public void userEditsBirthdayYear() {
        // Change days dropdown and reselect same day if possible if month is February and we go from leap year to non-leap year
        // or vice versa, update users age, and update displayed results.
        if (currentlySelectedMonth() == 2 && isLeapYear(usersBirthdayYear) != isLeapYear(currentlySelectedYear())) {
            DayOptions.GetComponent<TMPro.TMP_Dropdown>().ClearOptions();
            List<string> dayOptions = new List<string>();
            int numDaysFeb = 28;
            if (isLeapYear(currentlySelectedYear())) numDaysFeb++;
            for (int i = 1; i <= numDaysFeb; i++) dayOptions.Add(i.ToString());
            DayOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(dayOptions);
            if (usersBirthdayDay <= numDaysFeb) DayOptions.GetComponent<TMPro.TMP_Dropdown>().value = usersBirthdayDay - 1;
            else usersBirthdayDay = 1;
        }
        usersBirthdayYear = currentlySelectedYear();
        updateusersAgeYrs();
        updateDisplayedResults();
    }

    public void userEditsBirthdayDay() {
        usersBirthdayDay = currentlySelectedDay();
        updateusersAgeYrs();
        updateDisplayedResults();
    }

    public void userEditsFallingAsleepDuration() {
        usersFallingAsleepDuration = usersFallingAsleepDuration = 10 + FallAsleepDuration.GetComponent<TMPro.TMP_Dropdown>().value * 5;
        updateDisplayedResults();
    }

    public void userEditsWakeupTime() {
        usersWakeUpTimeMinutes = WakeUpTimeOptions.GetComponent<TMPro.TMP_Dropdown>().value * 5;
        updateDisplayedResults();
    }

    // Start is called before the first frame update
    void Start() {
        List<string> yearOptions = new List<string>();
        for (int i = currentYear; i >= currentYear - 122; i--) yearOptions.Add(i.ToString());
        YearOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(yearOptions);
        List<string> monthOptions = new List<string>();
        for (int i = 1; i <= 12; i++) monthOptions.Add(i.ToString());
        MonthOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(monthOptions);
        List<string> dayOptions = new List<string>();
        for (int i = 1; i <= 31; i++) dayOptions.Add(i.ToString());
        DayOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(dayOptions);
        List<string> fallingAsleepDurationOptions = new List<string>();
        for (int i = 10; i <= 20; i += 5) fallingAsleepDurationOptions.Add(i.ToString() + " min");
        FallAsleepDuration.GetComponent<TMPro.TMP_Dropdown>().AddOptions(fallingAsleepDurationOptions);
        List<string> wakeUpTimeOptions = new List<string>();
        for (int i = 0; i <= 55; i += 5) wakeUpTimeOptions.Add("12:" + i.ToString("00") + "am");
        for (int i = 1; i <= 11; i++) for (int j = 0; j <= 55; j += 5) wakeUpTimeOptions.Add(i.ToString() + ":" + j.ToString("00") + "am");
        for (int i = 0; i <= 55; i += 5) wakeUpTimeOptions.Add("12:" + i.ToString("00") + "pm");
        for (int i = 1; i <= 11; i++) for (int j = 0; j <= 55; j += 5) wakeUpTimeOptions.Add(i.ToString() + ":" + j.ToString("00") + "pm");
        WakeUpTimeOptions.GetComponent<TMPro.TMP_Dropdown>().AddOptions(wakeUpTimeOptions);
        MonthOptions.GetComponent<TMPro.TMP_Dropdown>().value = usersBirthdayMonth - 1;
        DayOptions.GetComponent<TMPro.TMP_Dropdown>().value = usersBirthdayDay - 1;
        YearOptions.GetComponent<TMPro.TMP_Dropdown>().value = currentYear - usersBirthdayYear;
        FallAsleepDuration.GetComponent<TMPro.TMP_Dropdown>().value = (usersFallingAsleepDuration - 10) / 5;
        WakeUpTimeOptions.GetComponent<TMPro.TMP_Dropdown>().value = usersWakeUpTimeMinutes / 5;
        updateDisplayedResults();
    }

}