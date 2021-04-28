#!/bin/bash

GAME_NAME=${PWD##*/}
MAIN_PATH=~/Desktop
UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
FINAL_PATH=$MAIN_PATH/$GAME_NAME

CURRENT_PATH=$(pwd)

rm -rf $FINAL_PATH
mkdir $FINAL_PATH

cd $FINAL_PATH

#mkdir web
mkdir linux
mkdir win32
mkdir osx

cd $CURRENT_PATH

#$UNITY_PATH -quit -batchmode -buildWebPlayer $FINAL_PATH/web/$GAME_NAME
cp -f ./ProjectSettings/InputManager-linux.asset ./ProjectSettings/InputManager.asset
$UNITY_PATH -quit -batchmode -buildLinux32Player $FINAL_PATH/linux/$GAME_NAME.x86
cp -f ./ProjectSettings/InputManager-win32.asset ./ProjectSettings/InputManager.asset
$UNITY_PATH -quit -batchmode -buildWindowsPlayer $FINAL_PATH/win32/$GAME_NAME.exe
cp -f ./ProjectSettings/InputManager-osx.asset ./ProjectSettings/InputManager.asset
$UNITY_PATH -quit -batchmode -buildOSXPlayer $FINAL_PATH/osx/$GAME_NAME.app

cd $FINAL_PATH

#cp -f web/$GAME_NAME/*.unity3d ./
rm -rf win32/*.pdb

cd $FINAL_PATH/linux
zip -r -X -q ../$GAME_NAME-linux *
cd $FINAL_PATH/win32
zip -r -X -q ../$GAME_NAME-win32 *
cd $FINAL_PATH/osx
zip -r -X -q ../$GAME_NAME-osx *

cd $FINAL_PATH

#rm -rf web
rm -rf linux
rm -rf win32
rm -rf osx