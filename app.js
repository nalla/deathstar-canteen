'use strict';

const SlackBot = require('slackbots');
const CommandFactory = require('./lib/command-factory');
const Config = require('./config');
const MongoConnect = require('./data/mongo-connect');

MongoConnect.initialize();

const bot = new SlackBot({
  token: Config.token,
  name: Config.name
});
let id = null;

bot.on('start', async (data) => {
  const user = await bot.getUser(bot.name);
  id = user.id;
});

bot.on('message', async (data) => {
  const params = {
    icon_emoji: Config.emoji
  };
  const regex = new RegExp(`\\<\\@${id}\\>\\s(\\w+)\\s?(.*)`);
  if (data.channel && data.text) {
    const match = data.text.match(regex);
    if (match) {
      const commandName = match[1].toLowerCase();
      const commandData = match[2];
      const command = CommandFactory.getCommand(commandName, commandData);
      if (command) {
        const response = await command.handle();
        bot.postMessage(data.channel, response, params);
      }
    }
  }
});
