import requests
import json
import os
import csv
import getpass
import time
import error_log as el

def getToken():
  # API Endpoints TEST
  # auth_endpoint = "https://mynorthsidelearningsandbox.docebosaas.com/oauth2/authorize"
  # API Endpoints PROD
  auth_endpoint = "https://mynorthsidelearning.docebosaas.com/oauth2/authorize"

  # Token Expiration
  token_expire = 3600

  # Get API Credentials
  # CLIENT_ID = input("What's the Client ID? ")
  # CLIENT_SECRET = getpass.getpass("What's the Client Secret? ")
  # REDIRECT_URI = input("What's the Redirect URI? ")

  #API Credentials TEST
  #CLIENT_ID = "Python-Test"
  #CLIENT_SECRET = "e7a5d26543091c9368bd051a9d2c9f60e298552d"
  #REDIRECT_URI = "https://httpbin.org/anything" - this is not a secure redirect URI and should not be used for PROD

  # Authorize via OAuth2 Implicit Authentication
  # auth_data = {'client_id': CLIENT_ID,
  #            'client_secret': CLIENT_SECRET,
  #            "redirect_uri": REDIRECT_URI,
  #            "response_type": "token"}

  # auth_response = requests.get(auth_endpoint, params=auth_data)

  # Note: You MUST get the token from the redirect URL to proceed
  # print(f"Go to this URL to give permission for API & get your token: \
  #{auth_response.url}")
  # Let's make this secure and just use a Postman token
  token = input("What is your token? ")    
  token_start = time.time()
  token_end = token_start + token_expire - 300 # Give ourselves 5 minutes of wiggle
  headers = {"Authorization": "Bearer " + token}
  return(headers, token_start, token_end)

# API Endpoints TEST
# insert_questions_endpoint = "https://mynorthsidelearningsandbox.docebosaas.com/learn/v1/lo/tests/batch"
# course_info_endpoint = "https://mynorthsidelearningsandbox.docebosaas.com/learn/v1/courses/" # Insert course_id here

# API Endpoints PROD
insert_questions_endpoint = "https://mynorthsidelearning.docebosaas.com/learn/v1/lo/tests/batch"
course_info_endpoint = "https://mynorthsidelearning.docebosaas.com/learn/v1/courses/" # Insert course_id here

# Working directory
wd = os.path.dirname(__file__)

# File names for prod insert:
# 'Question-Set-Gwinnett-1.csv' 43 lines / all success- done
# 'Question-Set-Gwinnett-2.csv' 357 lines / all success- done
# 'Question-Set-Gwinnett-3.csv' 787 lines / all success- done
# 'Question-Set-Gwinnett-4.csv' 9987 lines /  9971 inserted (16 duplicates)- done
# 'Question-Set-Gwinnett-5.csv' 10793 lines / 10787 inserted (5 duplicates)- done
# 'Question-Set-Gwinnett-6.csv' 13755 lines / all success - done

# 'Question-Set-Northside-1.csv' 10106 lines / 315 inserted, rest duplicate- done
# 'Question-Set-Northside-2.csv' 10003 lines / 244 inserted, rest duplicate- done
# 'Question-Set-Northside-3.csv' 10057 lines / 210 inserted, rest duplicate- done
# 'Question-Set-Northside-4.csv' 6570 lines / 264 inserted, rest duplicate- done 

# Course, test, and question data
csv_location = os.path.join(wd, 'Question-Set-Northside-4.csv')

course_test_list = []
test_ques_dict = {}
output_dict = {}
field_names = ["course_id","course_code", "docebo_code","course_name_input",
  "course_name","test_id", "test_name","type","question_title","answer_title",
  "is_correct", "score_if_correct", "status", "message","updated"]

# Initialize output CSV
csv_out = el.initCSV('Insert-Ques-Log', wd, field_names)
# csv_edit_log = el.initCSV('List-Edit', wd, field_names)  

# Get course numbers, test names, and questions from CSV file
with open(csv_location, encoding="utf-8", errors='replace') as csv_file:
    csv_reader = csv.reader(csv_file, delimiter=',')
    line_count = 0
    for row in csv_reader:
        # row 0 is the header, skip it
        if line_count > 0:
            if row[10] == '1':
              is_correct = True
              score = 100
            else:
              is_correct = False
              score = 0
            test_ques_dict = {
              "course_id" : row[0],
              "course_code" : "",
              "docebo_code": row[1], 
              "test_id": row[3],
              "test_name": "",
              "question_title": row[6],
              "type": "choice",
              "answer_title": row[9],
              "course_name_input": row[2],
              "course_name" : "",
              "is_correct": is_correct,
              "score_if_correct": score,
              "status": "",
              "message": "",
              "updated": False
            }
            course_test_list.append(test_ques_dict)
        line_count += 1

# Verify that initial types and point values are correct
# This doesn't work well and I don't have time to debug- use with caution

# ans_count = 1
# correct_count = 0
# prev_course = ""
# prev_test = ""
# prev_ques = ""
# for i in range(0, len(course_test_list)):
#  item = course_test_list[i]
#  this_course = item["course_id"]
#  this_test = item["test_id"]
#  this_ques = item["question_title"]
  
  # Same question
#  if this_course == prev_course and this_test == prev_test \
#   and this_ques == prev_ques:
#      ans_count += 1
#      if item["is_correct"]:
#        correct_count += 1
      # If more than one correct answer  
#      if correct_count > 1:  
#        point_val = 100.0 / correct_count
#        for j in range(0, ans_count):
#          corrected_item = course_test_list[i - j]
#          corrected_item["type"] = "choice_multiple"
#          if corrected_item["is_correct"]:
#            corrected_item["score_if_correct"] = point_val  
          
          # Log this
#          el.writeCSV(csv_edit_log, corrected_item, field_names)
#  else:
#    ans_count = 1
#    correct_count = 0
      
#  prev_course = this_course
#  prev_test = this_test
#  prev_ques = this_ques

headers, token_start, token_end = getToken()

# Get course names to construct test names and course code for batch insert call
course_info_dict = {}
for item in course_test_list:
  # Make sure our token hasn't expired
  if time.time() >= token_end:
    headers, token_start, token_end = getToken()

  # Do we already have this course's info? If no, go get it:
  if item["course_id"] not in course_info_dict.keys():

    response = requests.get(course_info_endpoint + item["course_id"], headers=headers)  

    # Successful API call
    if response.status_code == 200:
      duplicate_flag = False
      response_data = response.json()['data']
      response_tree = response_data['tree']
      course_name = response_data["name"]
      course_code = response_data["code"]

      # Is this test already in the course?
      for course_item in response_tree:
        if course_item["type"] == "test":
          exist_test = course_item["name"]
          if item["test_id"][1:] in exist_test \
             and item["course_name_input"] in exist_test:
              duplicate_flag = True
              output_dict = item.copy()
              output_dict["message"] = "Duplicate Test. Existing test is " + \
                f"{exist_test}"
              output_dict["status"] = "FAIL DUPLICATE TEST: " \
              + str(response.status_code)
              el.writeCSV(csv_out, output_dict, field_names)

      # If the course names don't match then log it. Do not insert the test
      if course_name != item["course_name_input"]:
        output_dict = item.copy()
        output_dict["message"] = "Course name mismatch. Input name: " + \
          f'''{item["course_name_input"]}. Docebo name: ''' + \
          f"{course_name}"
        output_dict["status"] = "FAIL COURSE NAME MISMATCH: " \
          + str(response.status_code)
        el.writeCSV(csv_out, output_dict, field_names)

      # If test doesn't already exist, then insert it
      else:
        course_info_dict[item["course_id"]] = [course_name, course_code, duplicate_flag]

    # API call error
    else:
      output_dict = item.copy()
      try:
        output_dict["message"] = response.json()['message']
      except:
        output_dict["message"] = response.text
      output_dict["status"] = "FAIL ON GET COURSE NAME: " + str(response.status_code)
      el.writeCSV(csv_out, output_dict, field_names)
  
  # Do we already have this course's info? If yes, get it from dictionary:
  else:
    course_name = course_info_dict[item["course_id"]][0]
    course_code = course_info_dict[item["course_id"]][1]
    duplicate_flag = course_info_dict[item["course_id"]][2]
          
  if course_name != "" and course_code != "" and not duplicate_flag:
    test_name = course_name + '- ' + item["test_id"]
    item["test_name"] = test_name
    item["course_name"] = course_name
    item["course_code"] = course_code
    item["updated"] = True
  
  # Error: Duplicate test from successful API call
  elif duplicate_flag:
      output_dict = item.copy()
      output_dict["message"] = "Duplicate test"
      output_dict["status"] = "FAIL DUPLICATE TEST: " + str(response.status_code)
      el.writeCSV(csv_out, output_dict, field_names)
  # Error: Empty course name or course code from successful API call
  else:
      output_dict = item.copy()
      output_dict["message"] = "Empty course name or course code"
      output_dict["status"] = "FAIL ON GET COURSE NAME: " + str(response.status_code)
      el.writeCSV(csv_out, output_dict, field_names)

# Do the API insert one question at a time
for item in course_test_list:

  # Check for expired token
  if time.time() >= token_end:
    headers, token_start, token_end = getToken()

  # Only add items for which update was successful
  if item["updated"]:
    question = {
      "course_code": item["course_code"],
      "object_title": item["test_name"],
      "type": item["type"],
      "question_title": item["question_title"],
      "answer_title" : item["answer_title"],
      "is_correct": item["is_correct"],
      "score_if_correct": item["score_if_correct"]
    }
    questions_json = json.dumps({"items":[question]}, indent=4)
    response = requests.post(insert_questions_endpoint, headers=headers, 
      data=questions_json)

    # If API call was successful
    if response.status_code == 200:
      response_data = response.json()['data'][0]
      output_dict = item.copy()
      output_dict["message"] = response_data['message']

      # If insert was successful
      if response_data['success']:   
        output_dict["status"] = "SUCCESS: " + str(response.status_code)
        el.writeCSV(csv_out, output_dict, field_names) 

      # If insert failed (Will still be 200 in some cases)
      else:
        output_dict["status"] = "FAIL ON INSERT: " + str(response.status_code)
        el.writeCSV(csv_out, output_dict, field_names)

    # If API call failed (status codes 400-500)
    else:
      try:
        output_dict["message"] = response.json()['message']
      except:
        output_dict["message"] = response.text
      output_dict["status"] = "FAIL ON INSERT: " + str(response.status_code)
      el.writeCSV(csv_out, output_dict, field_names)
