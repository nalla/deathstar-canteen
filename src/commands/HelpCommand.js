const BaseCommand = require('./BaseCommand');

class HelpCommand extends BaseCommand {
  constructor(commandData) {
    super(commandData);
    this.supportedCommands = ['hi', 'help', 'today', 'tomorrow', 'dayaftertomorrow', 'add', 'clear'];
  }

  handle() {
    return new Promise((resolve) => {
      resolve(this.commandData ? this.getDetailedHelpMessage() : this.getGeneralHelpMessage());
    });
  }

  getGeneralHelpMessage() {
    let message = 'The following commands are available:\n';

    this.supportedCommands.forEach((command) => {
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
      case 'today':
        return 'The *today* command will return a list of today\'s meals.';
      case 'tomorrow':
        return 'The *tomorrow* command will return a list of tomorrow\'s meals.';
      case 'dayaftertomorrow':
        return 'The *dayaftertomorrow* command will return a list of the day after tomorrow\'s meals.';
      case 'add':
        return 'The *add* command can be used to add something to the menu.\n\nExample: `add 01012017 Foobar`';
      case 'clear':
        return 'The *clear* command can be used to clear the menu on a given date.\n\nExample: `clear 01012017`';
      default:
        return this.getGeneralHelpMessage();
    }
  }
}

module.exports = HelpCommand;
