#!/bin/sh

APP_PATH="$HOME/deathstar-canteen/src/Deathstar.Canteen"
OUTPUT_PATH="$APP_PATH/bin/Release/netcoreapp2.0/publish"
DEPLOY_PATH=/var/cantina

echo "Building latest version.."
cd $APP_PATH
git fetch && git reset --hard origin/master
dotnet publish -c Release

echo "Closing cantina.."
sudo systemctl stop cantina.service

echo "Cleaning deployment.."
sudo rm -rf $DEPLOY_PATH/*

echo "Copying latest version.."
sudo cp -r $OUTPUT_PATH/* $DEPLOY_PATH

echo "Copying appsettings.."
sudo cp $HOME/appsettings.json $DEPLOY_PATH

echo "Updating ownership.."
sudo chown -R cantina:cantina $DEPLOY_PATH

echo "Opening cantina.."
sudo systemctl start cantina.service
