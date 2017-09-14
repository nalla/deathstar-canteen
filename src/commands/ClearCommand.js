const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class ClearCommand extends BaseCommand {
  async handle() {
    if (!this.commandData) { return 'You need to provide some input.'; }

    const match = this.commandData.match(/(\d{2})\.?(\d{2})\.?(\d{4})/);

    if (!match) { return 'You need to provide some valid input.'; }

    const date = `${match[3]}${match[2]}${match[1]}`;
    const formattedDate = `${match[1]}.${match[2]}.${match[3]}`;
    const menu = await Menu.findOne({ date });

    if (menu) {
      await Menu.remove({ date });
      return `I cleared the menu on *${formattedDate}*.`;
    }

    return `There is no menu on *${formattedDate}*!`;
  }
}

module.exports = ClearCommand;
