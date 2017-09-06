const moment = require('moment');

const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class TodayCommand extends BaseCommand {
  async handle() {
    const menu = await Menu.findOne({ date: moment().format('YYYYMMDD') });
    if (!menu) { return 'I don\'t know which meals are being served today!'; }
    return `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n${menu.print()}`;
  }
}

module.exports = TodayCommand;
