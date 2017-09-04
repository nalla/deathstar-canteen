'use strict';

const mongoose = require('mongoose');

const menuSchema = mongoose.Schema({
  date: String,
  meals: [String]
});
const Menu = mongoose.model('Menu', menuSchema);

module.exports = Menu;
