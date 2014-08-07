#!/bin/sh

echo "PK Del ================"

# パッケージJenkins削除
sh /Library/Application\ Support/Jenkins/Uninstall.command

echo "Get Pass ================"

JEN_INST=$PWD
echo $JEN_INST

echo "mkdir ================"
#ディレクトリ作成

mkdir -p ~/Library/LaunchAgents/
mkdir -p ~/Jenkins/
mkdir -p ~/bin/
mkdir -p ~/lib/java/

echo "File cp ================"

#ファイル配置
mv ${JEN_INST}/Documents/JenkinsUnity/jenkins.plist ~/Library/LaunchAgents/
mv ${JEN_INST}/Documents/JenkinsUnity/jenkins-runner.sh ~/bin/
mv ${JEN_INST}/Documents/JenkinsUnity/jenkins.war ~/lib/java/

echo "Launchd add ================"

#jenkins.plist の ${USER} と ${HOME} を書き換える
#${HOME} -> /Users/User名
#${USER} -> User名
#vim ~/Library/LaunchAgents/jenkins.plist

#launchd登録
launchctl load -wF -D user  ~/Library/LaunchAgents/jenkins.plist
launchctl start jenkins

echo "END ================"
