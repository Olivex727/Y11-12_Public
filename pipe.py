#Download info.txt and pipe to Desktop
import urllib.request
import os

#Get external inputs
url = input("Enter Url: ")
if url == "": url = "http://127.0.0.1:8080/download/info.txt"
loc = input("Enter location: ")+"/Downloads.txt"
if loc == "/Downloads.txt": loc = "Downloads/Downloads.txt"

#Get/Set Working Directory
if os.getcwd() != "/Users/olivermaxwellwalters":
    print(os.getcwd())
    os.chdir("/Users/olivermaxwellwalters")

#Download File
file = urllib.request.urlretrieve(url, 'Downloads.txt')
print("Sucsess downloading at "+url)

#Move file to new location  #Fun>Desktop>User
os.rename("./Downloads.txt", loc)
