'use strict';

class BaseCommandHandler {
  constructor(commandData) {
    this.commandData = commandData;
  }

  handle() {
    return new Promise((resolve, reject) => {
      resolve(null);
    });
  }
}

module.exports = BaseCommandHandler;
