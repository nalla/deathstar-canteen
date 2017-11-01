# deathstar-canteen

Jeff Vader's famous canteen. Located somewhere in the center of the deathstar. The `canteen-bot` provides access to the current menus inside the canteen. When integrating it with you _Slack_ team you can write `<botname> help` to get more information about the commands at hand.

## Travis-CI

[![Build Status](https://travis-ci.org/nalla/deathstar-canteen.svg?branch=master)](https://travis-ci.org/nalla/deathstar-canteen)

## Environment

Just add an `appsettings.json` file to the `Deathstar.Canteen` project providing the following information.

~~~json
{
  "token": "slack api token",
  "username": "botname"
}
~~~

With this data Jeff has everything that he needs to run the canteen.

## Test

~~~bash
> ./build
~~~

## Run

~~~bash
> ./build -target run
~~~
