const BaseCommand = require('./BaseCommand');

class HiCommand extends BaseCommand {
  handle() {
    return new Promise((resolve) => {
      resolve('Hi to you too!');
    });
  }
}

module.exports = HiCommand;
