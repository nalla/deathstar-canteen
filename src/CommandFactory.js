const HiCommand = require('./commands/HiCommand');
const HelpCommand = require('./commands/HelpCommand');
const TodayCommand = require('./commands/TodayCommand');
const TomorrowCommand = require('./commands/TomorrowCommand');
const AddCommand = require('./commands/AddCommand');

class CommandFactory {
  static getCommand(commandName, commandData) {
    switch (commandName) {
      case 'hi':
        return new HiCommand(commandData);
      case 'help':
        return new HelpCommand(commandData);
      case 'today':
        return new TodayCommand(commandData);
      case 'tomorrow':
        return new TomorrowCommand(commandData);
      case 'add':
        return new AddCommand(commandData);
      default:
        return null;
    }
  }
}

module.exports = CommandFactory;
