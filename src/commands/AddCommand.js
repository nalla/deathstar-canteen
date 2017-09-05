const BaseCommand = require('./BaseCommand');
const Menu = require('../models/Menu');

class AddCommand extends BaseCommand {
  async handle() {
    if (!this.commandData) { return 'You need to provide some input.'; }

    const match = this.commandData.match(/(\d{2})\.?(\d{2})\.?(\d{4})\s(\w.*)/);

    if (!match) { return 'You need to provide some valid input.'; }

    const date = `${match[3]}${match[2]}${match[1]}`;
    const meal = match[4];
    const menu = await Menu.findOne({ date });

    if (menu) {
      if (menu.meals.indexOf(meal) < 0) {
        menu.meals.push(meal);
        await menu.save();
        return `I added _${meal}_ to the menu on *${match[1]}.${match[2]}.${match[3]}*.`;
      }

      return `_${meal}_ is already on the menu on *${match[1]}.${match[2]}.${match[3]}*!`;
    }

    await Menu.create({ date, meals: [meal] });
    return `I added _${meal}_ to the menu on *${match[1]}.${match[2]}.${match[3]}*.`;
  }
}

module.exports = AddCommand;
