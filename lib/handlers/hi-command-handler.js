'use strict';

const BaseCommandHandler = require('./base-command-handler');

class HiCommandHandler extends BaseCommandHandler {
  handle() {
    return new Promise((resolve, reject) => {
      resolve('Hi to you too!');
    });
  }
}

module.exports = HiCommandHandler;
