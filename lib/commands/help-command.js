'use strict';

const BaseCommand = require('./base-command');

class HelpCommand extends BaseCommand {
  constructor(commandData) {
    super(commandData);
    this.supportedCommands = ['hi', 'help'];
  }

  handle() {
    return new Promise((resolve, reject) => {
      resolve(this.commandData ? this.getDetailedHelpMessage() : this.getGeneralHelpMessage());
    });
  }

  getGeneralHelpMessage() {
    let message = 'The following commands are available:\n';
    this.supportedCommands.forEach(command => {
      message += `  *${command}*\n`;
    });
    message += 'Use *help command* for more information about each command.';
    return message;
  }

  getDetailedHelpMessage() {
    switch (this.commandData) {
      case 'hi':
        return 'The *hi* command will return a friendly hello.';
      case 'help':
        return 'The *help* command will return a list of supported commands.';
      default:
        return this.getGeneralHelpMessage();
    }
  }
}

module.exports = HelpCommand;
