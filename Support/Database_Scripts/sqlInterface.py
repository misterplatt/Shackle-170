import mysql.connector
import getpass
import sys

from mysql.connector import errorcode

#
#	sqlInterface.py is a small single fire script that serves to accept input from game logs and files and push them to a database properly.
# 
#   usage : sqlInterface <database> <type-of-upload> <file>
#

#hardcoded for now
bugDatabase = "shacklegamebugdb.cfnebgo9culq.us-west-1.rds.amazonaws.com"
metricDB = "shacklegamemetricdb.cfnebgo9culq.us-west-1.rds.amazonaws.com"
cnx = None


def displayUsage( ) :
	print "\nData Upload/Download Usage : sqlInterface <operation> <database> <file>"
	print "Query Usage : sqlInterface query <database> <file-containing-queries>\n"
	print "\t\toperation = { upload, download, query }"
	print "\t\tdatabase = { bug, metric}"
	#Fill with reports as created in type of upload
	print "\t\tfile : path to file being manipulated"

#Checks  args and determines flow
#

def connectDB( db ) :
	global cnx
	_host = None
	
	if ( db == 'bug' ) :
		_host = bugDatabase
		_db = 'ShackleGameBugDB'
		_table = 'buglist'
	elif ( db == 'metric' ) :
		_host = metricDB
		
	else :
		print ( "Unknown database was encounter, please use 'metric' or 'bug' " )
		exit()
	#query login
	_username = raw_input( "Enter Username: " )
	_password = getpass.getpass( "Password : " )
	
	config = { 'user' : _username,
					'password' : _password,
					'host' : _host,
					'database' : _db,
					'raise_on_warnings' : True}
	
	try :
	
		cnx = mysql.connector.connect( **config)
											
	except mysql.connector.Error as err :
		if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
			print "Something is wrong with your user name or password"
		elif err.errno == errorcode.ER_BAD_DB_ERROR:
			print "Database does not exist"
		else:
			print err
			
		print "Could not complete Database Connection"
		exit()
	
	return

#accepts name of new table, followed by dictionary of varibles and sql datatypes
def createTable ( name, typedict) :
	global cnx
	
	cursor = cnx.cursor()
	
	command = "CREATE TABLE %s ( " % (name)
	
	for key in typedict :
		command += " %s %s," % ( key, typedict[key] )
	command = command[:-1]
	command += " );"
	
	cursor.execute(command)
	pass
	
def listTables () :
	global cnx
	
	cursor = cnx.cursor()
	cursor.execute("SHOW TABLES")
	print cursor.fetchall()
	
def performQuery( filepath ) :
	#Set active table
	#perform ops
	#return
	pass

def rawQuery( command ) :
	cursor = cnx.cursor()
	cursor.execute(command)
	print cursor.fetchall()
	return
	
#Expected File Format
# Ln 1 : TableName
# Lb 2... : Entries
#
# Ex :
# BugList
# BugName, BugNumber, AssignedTo, Corrected, DateCorrected
def performUpload( filepath ) :
	#Set active table
	#perform ops
	#return	
	pass
	
def performDownload( filepath ) :
	#Set active table
	#perform ops
	#return
	pass
	
def init( ) :	
	#normalize arguments
	sys.argv = [ arg.lower( ) for arg in sys.argv ]
	
	try :
	
		if sys.argv[1] == 'query' :
			connectDB( sys.argv[2] )
			performQuery( sys.argv[3] )
			
		elif sys.argv[1] == 'upload' :
			connectDB( sys.argv[2] )
			performUpload( sys.argv[3] )
			
		elif sys.argv[1] == 'download' :
			performDownload( )
			
		else :
			print "Unknown Operation Specified..."
			raise ValueError( 'Invalid Operation' )
			
	except ValueError : 
		displayUsage( )
		exit( )
		
	
	
init( )
rawQuery ( "show columns from buglist ")
if (cnx != None ) :
	cnx.close()