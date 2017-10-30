const moment = require('moment');

const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class NextCommand extends BaseCommand {
  async handle() {
    if (!this.commandData) { return 'You need to provide some input.'; }

    const match = this.commandData.match(/(\d+)/);

    if (!match) { return 'You need to provide some valid input.'; }

    const days = Math.min(Math.max(parseInt(match[1], 10), 1), 7);
    const menus = await Menu.find({ date: { $gte: moment().format('YYYYMMDD'), $lt: moment().add(days, 'days').format('YYYYMMDD') } });

    if (!menus || menus.length === 0) { return `I don't know which meals are being served the next ${days} days!`; }

    let response = '';

    menus.forEach((menu) => {
      response += `On *${moment(menu.date).format('DD.MM.YYYY')}* the meals are:\n${menu.print()}\n`;
    });

    return response;
  }
}

module.exports = NextCommand;
