import csv
import os
import time

def initCSV(name, directory, headers):
    file_name = name + '-'  + time.strftime("%Y%m%d-%H%M%S") + '.csv'
    csv_location = os.path.join(directory, file_name)
    with open(csv_location, 'w', newline='', encoding='utf-8') as f:
        writer = csv.DictWriter(f, fieldnames=headers)
        writer.writeheader()
        f.close()
    return csv_location

def writeCSV(csv_location, output_dict, headers):
    with open(csv_location, 'a', newline='', encoding='utf-8') as f:
        writer = csv.DictWriter(f, fieldnames=headers)
        writer.writerow(output_dict)
        f.close()