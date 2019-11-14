# deathstar-canteen

Jeff Vader's famous canteen. Located somewhere in the center of the deathstar. The `canteen-bot` provides access to the current menus inside the canteen. When integrating it with you _Slack_ team you can write `<botname> help` to get more information about the commands at hand.

## Environment

Just add an `docker-compose.prod.yml` file to the project providing the following information.

~~~yml
version: '3'

services:

  deathstar-canteen:
    environment:
      - Slackbot__Token=slacktoken
      - Slackbot__Username=botusername
~~~

With this data Jeff has everything that he needs to run the canteen.

## Docker

Just run the following command to setup the deathstar canteen.

~~~bash
$> docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
~~~

You can redeploy the deathstar canteen after that with the following command.

~~~bash
$> ./redeploy.sh
~~~
