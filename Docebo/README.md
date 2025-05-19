# Introduction
This code was intended to be a one-time use solution that has been provided here for reference only. Running this code in it's present state will result in duplicate test questions being added to Docebo. This code MUST be edited to be reused.

# Python Files
1. doceboapi.py
    - Python script that automates batch loading from a csv file in the current working directory to the Docebo platform
    - To run, you must do the following:
        - CSV location: update with the new file name. It must be in the same working directory as the code
        - Make sure your CSV is formatted correctly by looking at the Question-Set-Northside-4.csv file
        - Get an API access token via Postman
    - This code spits out a log file into the current working directory with a name that starts with 'Insert-Ques-Log'
2. error-log.py
    - This file contains functions for writing to the error log. 
    - It needs to be present in the current working directory to run doceboapi.py
3. selenium_score_mode.py
    - This file automates some button clicks to change the scoring mode on a set of courses using the Chrome web driver
    - The Chrome web driver (chromedriver.exe) must be present in the working directory
    - To run, you must do the following:
        - CSV location: update with the new file name. It must be in the same working directory as the code
        - Make sure your CSV is formatted correctly by looking at the ScormAicc6.csv file
        - You will be prompted to enter your Docebo super admin credentials upon running this file
4. selenium_test.py
    - This is exactly what it sounds like, and is probably not fit for human consumption

# Other Stuff
- There are lots of CSV files and insert logs that have been included for record keeping. They can be reviewed to see examples of correctly formatted input/output
- Chromedriver.exe: this is required to use selenium_score_mode.py