import getpass
import time
import csv
import os
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select

import error_log as el

# URLS from Docebo TEST
# URL = "https://mynorthsidelearningsandbox.docebosaas.com/learn"
# HOME_URL = "https://mynorthsidelearningsandbox.docebosaas.com/pages/17/nshhomelist"
# COURSE_EDIT_URL = "https://mynorthsidelearningsandbox.docebosaas.com/course/edit/" # Insert course number here

# URLS from Docebo PROD
URL = "https://mynorthsidelearning.docebosaas.com/learn"
HOME_URL = "https://mynorthsidelearning.docebosaas.com/pages/15/home"
COURSE_EDIT_URL = "https://mynorthsidelearning.docebosaas.com/course/edit/" # Insert course number here

# Element IDs from Docebo site
user_name_id = "ui-input-text-0"
password_id = "ui-input-password-0"
i_frame_id = "legacy-wrapper-iframe"
score_ddl_id = "LearningCourse_initial_object"

# XPATH from Docebo site
score_btn_xpath = '''//*[@id="coursesettings-menu"]/li[7]/a'''
calc_btn_xpath = '''//*[@id="final_score"]/div[1]/div/div/div/label[2]'''
save_btn_xpath = '''//*[@id="learning-course-form"]/div/div[3]/div/input'''

course_test_dict = {}
output_dict = {}
field_names = ["course_id", "course_name", "status"]

# Working directory
wd = os.path.dirname(__file__)

# Course data
csv_location = os.path.join(wd, 'ScormAicc6.csv')

# Chrome Driver location
cd_location = os.path.join(wd, 'chromedriver.exe')

# Initialize output CSV
csv_out = el.initCSV('Edit-Course-Log', wd, field_names)

# Get course numbers and test names from CSV file
with open(csv_location) as csv_file:
    csv_reader = csv.reader(csv_file, delimiter=',')
    line_count = 0
    for row in csv_reader:
        # row 0 is the header, skip it
        if line_count > 0:
            # row[0] is course id, row[2] is course name
            if row[0] not in course_test_dict.keys():
                course_test_dict[row[0]] = row[2]
        line_count += 1

# Get username and password
user_name_input = input("What is your Docebo superadmin username? ")
password_input = getpass.getpass("What is your password? ")

# Start webdriver
service = Service(cd_location)
browser = webdriver.Chrome(service = service)

# Log in
browser.get(URL)
try:
    element = WebDriverWait(browser, 10).until(
        EC.presence_of_element_located((By.ID, user_name_id))
    )
finally:
    user_name_txtbox = browser.find_element_by_id(user_name_id)
    password_txtbox = browser.find_element_by_id(password_id)
    user_name_txtbox.send_keys(user_name_input)
    password_txtbox.send_keys(password_input)
    password_txtbox.submit()

# Wait while login occurs
time.sleep(5)

# Verify successful
if browser.current_url == HOME_URL:
    # Loop through courses
    for course in course_test_dict.keys():
        course_name = course_test_dict[course]
        output_dict.clear()        
        output_dict["course_id"] = course
        output_dict["course_name"] = course_name
        course_url = COURSE_EDIT_URL + course + ';tab=advanced_settings'
        browser.get(course_url)

        # Wait while URL loads
        time.sleep(2)

        # Verify successful
        if browser.current_url == course_url:
            # Switch to frame to interact with pane
            try:
                element = WebDriverWait(browser, 10).until(
                    EC.presence_of_element_located((By.ID, i_frame_id)))
            finally:
                browser.switch_to.frame(browser.find_element_by_id(i_frame_id))
                browser.find_element_by_xpath(score_btn_xpath).click()
                browser.find_element_by_xpath(calc_btn_xpath).click()  
                item_count = len(Select(browser.find_element_by_id(score_ddl_id)).options)   
                if item_count > 0:
                    browser.find_element_by_xpath(save_btn_xpath).click()
                if item_count == 1:                
                    output_dict["status"] = "SUCCESS"
                elif item_count > 1:
                    output_dict["status"] = "REVIEW- More than one item"
                else:
                    output_dict["status"] = "FAIL- No item to select"
                el.writeCSV(csv_out, output_dict, field_names)    

        else:
            output_dict["status"] = "Fail- Course URL load failure"
            el.writeCSV(csv_out, output_dict, field_names)
else:
    output_dict["course_name"] = "N/A"
    output_dict["course_id"] = "N/A"
    output_dict["status"] = "Fail- Login failure"
    el.writeCSV(csv_out, output_dict, field_names)

browser.implicitly_wait(5)
browser.quit()

