const moment = require('moment');

const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class TomorrowCommand extends BaseCommand {
  async handle() {
    const menu = await Menu.findOne({ date: moment().add(1, 'days').format('YYYYMMDD') });
    if (!menu) { return 'I don\'t know which meals are being served tomorrow!'; }
    return `Tomorrow is the *${moment().add(1, 'days').format('DD.MM.YYYY')}* and the meals are:\n${menu.print()}`;
  }
}

module.exports = TomorrowCommand;
