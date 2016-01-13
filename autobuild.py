import subprocess

#Check for env variable and terminate if not found

subprocess.call("Echo Beginning Build Process. Initiating Git Pull... | slacker -c buildnotes", shell=True)
subprocess.call("git pull", shell=True)
subprocess.call("Echo Git Pull Complete. | slacker -c buildnotes", shell=True)
subprocess.call("Echo Launching Virtual Machine | slacker -c buildnotes", shell=True)
# Open VM and Performing Build with seperate script on startup
subprocess.call("Virtual Machine Closed. Build Complete. | slacker -c buildnotes", shell=True)