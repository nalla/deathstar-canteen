const HiCommand = require('./commands/HiCommand');
const HelpCommand = require('./commands/HelpCommand');
const TodayCommand = require('./commands/TodayCommand');
const TomorrowCommand = require('./commands/TomorrowCommand');
const AddCommand = require('./commands/AddCommand');
const ClearCommand = require('./commands/ClearCommand');

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
      case 'clear':
        return new ClearCommand(commandData);
      default:
        return null;
    }
  }
}

module.exports = CommandFactory;
