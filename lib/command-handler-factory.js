'use strict';

const HiCommandHandler = require('./handlers/hi-command-handler');
const HelpCommandHandler = require('./handlers/help-command-handler');

class CommandHandlerFactory {
  static getHandler(commandName, commandData) {
    let handler = null;
    switch (commandName) {
      case 'hi':
        handler = new HiCommandHandler(commandData);
        break;
      case 'help':
        handler = new HelpCommandHandler(commandData);
        break;
    }
    return handler;
  }
}

module.exports = CommandHandlerFactory;

