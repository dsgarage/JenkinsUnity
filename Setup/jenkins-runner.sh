#!/bin/bash
#
# Startup script used by Jenkins launchd job.
# Mac OS X launchd process calls this script to customize
# the java process command line used to run Jenkins.
#-      
# Customizable parameters are found in
# ~/Library/Preferences/jenkins.plist
#       
# You can manipulate it using the "defaults" utility.
# See "man defaults" for details.
        
defaults="defaults read ~/Library/Preferences/jenkins"
        
war=`$defaults war` || war="${HOME}/lib/java/jenkins.war"
        
javaArgs=""
heapSize=`$defaults heapSize` && javaArgs="$javaArgs -Xmx${heapSize}"
            
home=`$defaults JENKINS_HOME` && export JENKINS_HOME="$home"
        
add_to_args() {
    val=`$defaults $1` && args="$args --${1}=${val}"
}       
    
args=""
add_to_args prefix
add_to_args httpPort
add_to_args httpListenAddress
add_to_args httpsPort
add_to_args httpsListenAddress
add_to_args ajp13Port
add_to_args ajp13ListenAddress

echo "JENKINS_HOME=$JENKINS_HOME"
echo "Jenkins command line for execution:"
echo /usr/bin/java $javaArgs -jar "$war" $args
exec /usr/bin/java $javaArgs -jar "$war" $args