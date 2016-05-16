#StatCrunch is a down and dirty python script that calculates statistics of accumulated
#player metrics from playtesting
#it then outputs this as a file to be used by DDA

import sys

def crunchStat(levelName) :
	metricFile = open( levelName + "_metrics.txt", 'r')
	
	eventTimes = {}
	numOfRecords = {}
	timesFailedToComplete = {}
	
	fileData = metricFile.read().splitlines()
	metricFile.close()
	
	#Read in statistics from metric file, store number of occurrences and running sum
	for line in fileData :
		thisData = line.split(',')
		print thisData[0]
		if thisData[0] == 'PuzzleEvent' :
			continue
		if float(thisData[1]) == -1.0 :
			if thisData[0] in timesFailedToComplete :
				timesFailedToComplete[thisData[0]] = timesFailedToComplete[thisData[0]] + 1.0
			else :
				timesFailedToComplete[thisData[0]] = 1.0
			continue
			
		
		if  thisData[0] in numOfRecords :
			numOfRecords[thisData[0]] = numOfRecords[thisData[0]] + 1.0
			eventTimes[thisData[0]] = eventTimes[thisData[0]] + float(thisData[1])		
		else :
			numOfRecords[thisData[0]] = 1.0
			eventTimes[thisData[0]] = float(thisData[1])
			
	#Calculate Avgs
	for eventName in eventTimes :
		eventTimes[eventName] = eventTimes[eventName] / numOfRecords[eventName]
	
	outFile = open( "./Output/" + levelName + ".txt", 'w')
	
	#output file
	outFile.write(levelName + "\r\n")
	outFile.write("Average Event Completion Times\r\n")
	
	for key in eventTimes :
		outFile.write( key +"," + str(eventTimes[key]) + "\r\n")
	
	outFile.write("Event Non-Completions\r\n")
	
	for key in timesFailedToComplete:
		outFile.write(key + ",")
		outFile.write(str(timesFailedToComplete[key]))
		outFile.write("\r\n")
		
	outFile.close()
	
for arg in sys.argv[1:]:
	print "Crunching Stats for : " + arg
	crunchStat(arg)