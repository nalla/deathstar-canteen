'use strict';

const HiCommand = require('./commands/hi-command');
const HelpCommand = require('./commands/help-command');
const TodayCommand = require('./commands/today-command');

class CommandFactory {
  static getCommand(commandName, commandData) {
    let command = null;
    switch (commandName) {
      case 'hi':
        command = new HiCommand(commandData);
        break;
      case 'help':
        command = new HelpCommand(commandData);
        break;
      case 'today':
        command = new TodayCommand(commandData);
        break;
    }
    return command;
  }
}

module.exports = CommandFactory;

