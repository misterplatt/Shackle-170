
#autobuild.py a script to automatically build Shackle in Unity and provide slack integration.
#Written by Ryan Connors Winter 2016

import os
import subprocess
import zipfile
import time

def zipdir(path, ziph) :
	for root, dirs, files in os.walk(path) :
		for file in files :
			ziph.write(os.path.join(root,file))

#Set this to where you want builds to be created.
buildDirectory = "C:\\Builds"
unityDirectory = "C:\\Program Files\\Unity\\Editor\\"

#sync code to head
os.chdir("..")
subprocess.call("echo \"Syncing to most recent commit...\" | python ./Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)
subprocess.call("git pull")
subprocess.call("echo \"Sync Complete.\" | python ./Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)

#Get Project Directory
os.chdir("Shackle")
projDirectory = subprocess.check_output("cd",shell=True).rstrip()

#perform build by calling supplementary unity script on execution of Unity. 
subprocess.call("echo \"Executing Build...\" | python ../Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)
subprocess.call("\"" + unityDirectory + "unity.exe \" -projectPath " + projDirectory + " -executeMethod spt_AutomatedBuild.Start -quit", shell=True)

#update buildNumber
buildNumber = open("../Support/buildNum", 'r').read()
subprocess.call("echo \"Shackle Build #%s Complete.\" | python ../Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"" % buildNumber, shell=True)

#Zip application and data resources
subprocess.call("echo \"Compressing Build...\" | python ../Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)
zipf = zipfile.ZipFile( "%s/ShackleBuild_%s.zip" % ( buildDirectory, buildNumber) , 'w' )
zipf.write("%s/ShackleBuild_%s.exe" % ( buildDirectory, buildNumber) )
zipdir("%s/ShackleBuild_%s_Data/" % ( buildDirectory, buildNumber), zipf )
zipf.close()
subprocess.call("echo \"Compression Complete.\" | python ../Support/slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)

#get ftp username and password from environment variable
_username = os.environ['FTP_USER']
_password = os.environ['FTP_PASS']
_FTP_IP = os.environ['FTP_IP']

os.chdir(buildDirectory)

subprocess.call("cd", shell=True)

#update batch file
batch = open("ftpbatch",'w')
batch.write("open %s\n" % _FTP_IP)
batch.write("cd /ShackleGame/Builds\n")
batch.write("put ShackleBuild_%s.zip\n" % buildNumber)
batch.close()

time.sleep(2)
subprocess.call("echo \"Uploading Build...\" | python slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)
subprocess.call("psftp -l %FTP_USER% -pw %FTP_PASS% -b ftpbatch", shell=True)
subprocess.call("echo \"Build Uploaded, Give me a Cookie or feel my wraith.\" | python slackcmd.py -c \"buildnotes\" -n \"Chester_TheContinuousBuilderBot\"", shell=True)