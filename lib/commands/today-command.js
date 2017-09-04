'use strict';

const BaseCommand = require('./base-command');
const moment = require('moment');
const Menu = require('../../data/models/menu');

class TodayCommand extends BaseCommand {
  async handle() {
    const menu = await Menu.findOne({ date: moment().format('YYYYMMDD') });
    if (!menu)
      return 'I don\'t know which meals are being served today!';
    let message = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n`;
    menu.meals.forEach(meal => {
      message += `  ${meal}\n`;
    });
    return message;
  }
}

module.exports = TodayCommand;
