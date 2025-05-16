# Introduction
HR/Benefits Team COVID Pay Tracking Application

*** As of 1/10/25 this application is decommissioned ***

## Contacts
- Developers: Shruti Patrabansh & Christina Parrott
- HR/Benefits Contact: Stephanie Alexy

# Environments 
## Test Environment
- http://nshcovidpayhr-test.northside.local/
- Database: nsmvsqlcms02.COVID19HotlineTracker

## Production Enviornment
- http://nshcovidpayhr.northside.local 
- nsmdbfc04-ag01.COVID19HotlineTracker

# Access/Permissions
- Requests should be approved by Stephanie Alexy & Shruti
- Application access: NS\CovidPayHRAppMembers
- Database account (with db_owner role): NS\hrcovidpaytrackapp 

# ETL and Jobs
## ETL
- SSIS Packages on NSMVETLSVR01 in CovidPayTracking folder
    1. AutoDecideNonNumSTD: 
        - Creates a "No" pay decision for employees meeting certain criteria. 
        - Runs every two weeks on the Fri and Sat prior to the end of the pay period. 
    2. GetCovidCasesMadeWhole: 
        - Runs daily at 5 AM. 
    3. GetCOVIDTests:
        - Gets new COVID Tests from the Personnel database every day at 5 AM 
    4. GetHotlineLogData:
        - Gets new Hotline Log entries from Pam Russman-Chamber's EH Tracking database every day at 5 AM 
    5. GetMerits:
        - Gets merit information from Ultipro export every day at 5 AM
    6. GetSTDElection:
        - Gets short-term disability information from Excel sheet every day at 5 AM
- ETL account: NS\CovidPayTrackETL
- See the [SSIS Covid Pay Tracking repo](https://nsmvtfs02.northside.local/DataAndDevelopmentServices/SSIS%20-%20HRCovidPayTracking) for more details

## Jobs
- NSMVETLSVR01 ETL Jobs:
    1. Covid Pay Tracking Application Automate Pay Decisions: biweekly on Fri and Sat
    2. Covid Pay Tracking Application DataFeed: everyday at 5 AM
- NSMDBFC04-AG01 Jobs:
    1. HR Covid Pay Application - Send Daily Email
        - Runs at 5:30 AM every day
        - Executes SP: SendEmailNegCOVIDEH: Sends an email to each employee with a negative COVID test from employee health that was imported on that day
        - Executes SP: SendEmailNegCOVIDHotline: Sends an email to each employee with a negative COVID test from the hotline log that was imported on that day
    2. HR Covid Pay Application - Send Email Notification
        - Runs every two weeks on the Wednesday before pay day at 11 PM
        - Sends emails to everyone who is recieving COVID Pay for the pay period

# Common Issues
1. Failure of Covid Pay Tracking Application DataFeed job: 
    - Generally, the job fails on the "GetHotlineLogData" step because Pam's Access DB is open
    - Reach out to Pam at Pamela.Russman-Chambers@northside.com to inquire if the DB is open, and restart the job at the failed step once closed
2. Complaints of application slowness or failure of the Hotline Log to load
    - These issues usually come in around 9 AM when pay is being processed, which is Monday - Wednesday on pay day weeks
    - Usually, there is high CPU utilization on the DB server during this time that resolves by 9:30 - 10 AM


