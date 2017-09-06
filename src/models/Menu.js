const mongoose = require('mongoose');

const MenuSchema = mongoose.Schema({
  date: { type: String, unique: true },
  meals: [String],
});
MenuSchema.methods.print = function print() {
  let i = 0;
  let result = '';
  this.meals.forEach((meal) => {
    i += 1;
    result += `${i}. ${meal}\n`;
  });
  return result;
};
const Menu = mongoose.model('Menu', MenuSchema);

module.exports = Menu;
