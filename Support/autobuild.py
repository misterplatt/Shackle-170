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

#Get Project Directory
os.chdir("../Shackle/")
projDirectory = subprocess.check_output("cd",shell=True).rstrip()

#perform build by calling supplementary unity script on execution of Unity. 
subprocess.call("\"" + unityDirectory + "unity.exe \" -projectPath " + projDirectory + " -executeMethod spt_AutomatedBuild.Start -quit", shell=True)

#update buildNumber
buildNumber = open("../Support/buildNum", 'r').read()


#Zip application and data resources
zipf = zipfile.ZipFile( "%s/ShackleBuild_%s.zip" % ( buildDirectory, buildNumber) , 'w' )
zipf.write("%s/ShackleBuild_%s.exe" % ( buildDirectory, buildNumber) )
zipdir("%s/ShackleBuild_%s_Data/" % ( buildDirectory, buildNumber), zipf )
zipf.close()

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

subprocess.call("psftp -l %FTP_USER% -pw %FTP_PASS% -b ftpbatch", shell=True)