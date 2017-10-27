const moment = require('moment');

const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class DayAfterTomorrowCommand extends BaseCommand {
  async handle() {
    const menu = await Menu.findOne({ date: moment().add(2, 'days').format('YYYYMMDD') });

    if (!menu) { return 'I don\'t know which meals are being served the day after tomorrow!'; }

    return `The day after tomorrow is the *${moment().add(2, 'days').format('DD.MM.YYYY')}* and the meals are:\n${menu.print()}`;
  }
}

module.exports = DayAfterTomorrowCommand;
