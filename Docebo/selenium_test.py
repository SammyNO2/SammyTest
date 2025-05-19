import getpass
import time
import csv
import os
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.common.exceptions import NoSuchElementException

import error_log as el

# URLS from Docebo site
URL = "https://mynorthsidelearningsandbox.docebosaas.com/learn"
HOME_URL = "https://mynorthsidelearningsandbox.docebosaas.com/pages/17/nshhomelist"
COURSE_EDIT_URL = "https://mynorthsidelearningsandbox.docebosaas.com/course/edit/" # Insert course number here

# Element IDs from Docebo site
user_name_id = "ui-input-text-0"
password_id = "ui-input-password-0"
i_frame_id = "legacy-wrapper-iframe"
test_title_id = "title"
new_test_title_id = "test-title"

# XPATH from Docebo site
test_btn_xpath = '''//*[@id="player-manage-add-lo-button"]/div/ul/li[15]/span[2]/a'''
training_btn_xpath = '''//*[@id="player-manage-add-lo-button"]/div/a'''

course_test_dict = {}
output_dict = {}
field_names = ["course_name", "test_name", "status"]

# Working directory
wd = os.path.dirname(__file__)

# Course and test shell data
csv_location = os.path.join(wd, 'TestExport.csv')

# Chrome Driver location
cd_location = os.path.join(wd, 'chromedriver.exe')

# Initialize output CSV
csv_out = el.initCSV('Insert-Test-Log', wd, field_names)

# Get course numbers and test names from CSV file
with open(csv_location) as csv_file:
    csv_reader = csv.reader(csv_file, delimiter=',')
    line_count = 0
    for row in csv_reader:
        # row 0 is the header, skip it
        if line_count > 0:
            # row[0] is course number, row[1] is test name
            if row[0] not in course_test_dict.keys():
                course_test_dict[row[0]] = [row[1]]
            else:
                course_test_dict[row[0]].append(row[1])
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
        output_dict.clear()        
        output_dict["course_name"] = course
        test_list = course_test_dict[course]

        # Loop through test list for course
        for test in test_list:
            output_dict["test_name"] = test
            course_url = COURSE_EDIT_URL + course + ';tab=training_materials'
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
                    browser.find_element_by_xpath(training_btn_xpath).click()
                    browser.find_element_by_xpath(test_btn_xpath).click()
                
                # Wait for add test menu to load
                try:
                    element = WebDriverWait(browser, 10).until(
                        EC.presence_of_element_located((By.ID, test_title_id)))

                # Insert test name and submit
                finally:
                    title_txtbox = browser.find_element_by_id(test_title_id)
                    title_txtbox.send_keys(test)
                    title_txtbox.submit()

                # Wait while test page loads
                time.sleep(2)
                # Verify successful
                try:
                    test_name = browser.find_element_by_id(new_test_title_id)
                    if test_name.text != test:
                        output_dict["status"] = "Fail- Test name mismatch"
                        el.writeCSV(csv_out, output_dict, field_names)
                        #print(f"Name mismatch error on course {course} and test {test}")
                    else:
                        output_dict["status"] = "Success"
                        el.writeCSV(csv_out, output_dict, field_names)
                except NoSuchElementException:
                    output_dict["status"] = "Fail- Test insertion failure"
                    el.writeCSV(csv_out, output_dict, field_names)
                    #print(f"Course {course} and test {test} failed to insert.")               
            else:
                output_dict["status"] = "Fail- Course URL load failure"
                el.writeCSV(csv_out, output_dict, field_names)
                #print(f"Course {course} and test {test} failed to insert.")
        time.sleep(2)
else:
    output_dict["course_name"] = "N/A"
    output_dict["test_name"] = "N/A"
    output_dict["status"] = "Fail- Login failure"
    el.writeCSV(csv_out, output_dict, field_names)
    #print("Login failed. Please try again.")

browser.implicitly_wait(5)
browser.quit()

