import subprocess

unityPath = "C:/Program Files/Unity/Editor/Unity.exe"
projectPath = "D:\work\ShackleClone\Shackle-170\Shackle"
vsPath = "C:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/devenv.exe" 
solutionPath = "D:\work\ShackleClone\Shackle-170\Shackle/Shackle.sln"

print "Launching Unity and VS..."

command = unityPath + "-projectPath " + projectPath
try :
	subprocess.Popen("\"" + vsPath + "\" \"" + solutionPath + "\"")
	subprocess.Popen("\"" + unityPath + "\"" + " -projectPath " + "\"" + projectPath + "\"")
	print "\"" + unityPath + "\"" + " -projectPath " + "\"" + projectPath + "\""
except :
	print "Error : Script Paths are not properly set up."
	
print "process complete"

