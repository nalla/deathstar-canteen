'use strict';

const BaseCommand = require('./base-command');

class HiCommand extends BaseCommand {
  handle() {
    return new Promise((resolve, reject) => {
      resolve('Hi to you too!');
    });
  }
}

module.exports = HiCommand;
