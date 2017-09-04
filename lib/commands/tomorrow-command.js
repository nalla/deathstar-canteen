'use strict';

const BaseCommand = require('./base-command');
const moment = require('moment');
const Menu = require('../../data/models/menu');

class TomorrowCommand extends BaseCommand {
  async handle() {
    const menu = await Menu.findOne({ date: moment().add(1, 'days').format('YYYYMMDD') });
    if (!menu)
      return 'I don\'t know which meals are being served tomorrow!';
    let message = `Tomorrow is the *${moment().add(1, 'days').format('DD.MM.YYYY')}* and the meals are:\n`;
    menu.meals.forEach(meal => {
      message += `  ${meal}\n`;
    });
    return message;
  }
}

module.exports = TomorrowCommand;
