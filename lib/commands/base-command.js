'use strict';

class BaseCommand {
  constructor(commandData) {
    this.commandData = commandData;
  }

  handle() {
    return new Promise((resolve, reject) => {
      resolve(null);
    });
  }
}

module.exports = BaseCommand;
