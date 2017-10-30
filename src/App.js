const SlackBot = require('slackbots');
const CommandFactory = require('./CommandFactory');
const Config = require('../config');
require('./MongoConnection');

class App {
  constructor() {
    this.getBot();
    this.getRegex();
    this.getParams();
    this.startBot();
  }

  getBot() {
    this.bot = new SlackBot({
      token: Config.token,
      name: Config.name,
    });
  }

  getRegex() {
    this.bot.on('start', async () => {
      const user = await this.bot.getUser(this.bot.name);
      this.regex = new RegExp(`\\<\\@${user.id}\\>\\s(\\w+)\\s?(.*)`);
    });
  }

  getParams() {
    this.params = {
      icon_emoji: Config.emoji,
    };
  }

  startBot() {
    this.bot.on('message', async (data) => {
      console.log('Parsing..');
      if (data.channel && data.text) {
        const match = data.text.match(this.regex);

        if (match) {
          const commandName = match[1].toLowerCase();
          const commandData = match[2];
          const command = CommandFactory.getCommand(commandName, commandData);

          if (command) {
            console.log(`Received: ${commandName} ${commandData}`);
            const response = await command.handle();

            this.bot.postMessage(data.channel, response, this.params);
          }

          console.log('Done.');
        }
      }
    });
  }
}

module.exports = new App();
