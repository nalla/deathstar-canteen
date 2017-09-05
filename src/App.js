const SlackBot = require('slackbots');
const CommandFactory = require('./CommandFactory');
const Config = require('../config');
require('./MongoConnection');

class App {
  constructor() {
    this.getBot();
    this.getId();
    this.startBot();
  }

  getBot() {
    this.bot = new SlackBot({
      token: Config.token,
      name: Config.name,
    });
  }

  getId() {
    this.bot.on('start', async () => {
      const user = await this.bot.getUser(this.bot.name);
      this.id = user.id;
    });
  }

  startBot() {
    this.bot.on('message', async (data) => {
      const params = {
        icon_emoji: Config.emoji,
      };
      const regex = new RegExp(`\\<\\@${this.id}\\>\\s(\\w+)\\s?(.*)`);
      if (data.channel && data.text) {
        const match = data.text.match(regex);
        if (match) {
          const commandName = match[1].toLowerCase();
          const commandData = match[2];
          const command = CommandFactory.getCommand(commandName, commandData);
          if (command) {
            const response = await command.handle();
            this.bot.postMessage(data.channel, response, params);
          }
        }
      }
    });
  }
}

module.exports = new App();
