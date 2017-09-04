'use strict';

const mongoose = require('mongoose');

const Config = require('../config');

class MongoConnect {
  static initialize(callback) {
    if(mongoose.connection.db) {
      callback();
      return;
    }

    mongoose.connect(Config.mongo, {useMongoClient: true});
    mongoose.connection.once('open', () => {
      callback();
    });
  }
}

module.exports = MongoConnect;
