'use strict';

const BaseCommand = require('./base-command');
const moment = require('moment');
const Menu = require('../../data/models/menu');

class TodayCommand extends BaseCommand {
  handle() {
    return new Promise((resolve, reject) => {
      Menu.findOne({date: moment().format('YYYYMMDD')}, (error, result) => {
        if (error)
          resolve('Error getting menu data.');
        else {
          if(!result)
          {
            resolve('I don\'t know which meals are being served today.!');
            return;
          }
          let message = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n`;
          result.meals.forEach(meal => {
            message += `  ${meal}\n`;
          });
          resolve(message);
        }
      });
    });
  }
}

module.exports = TodayCommand;
