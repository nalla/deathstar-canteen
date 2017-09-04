'use strict';

const mongoose = require('mongoose');

const Config = require('../config');

class MongoConnect {
  static initialize() {
    if(mongoose.connection.db) {
      return;
    }
    mongoose.Promise = global.Promise;
    mongoose.connect(Config.mongo, {useMongoClient: true});
  }
}

module.exports = MongoConnect;
