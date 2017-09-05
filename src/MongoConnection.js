const mongoose = require('mongoose');

const Config = require('../config');

class MongoConnection {
  static connect() {
    if (mongoose.connection.db) {
      return;
    }
    mongoose.Promise = global.Promise;
    mongoose.connect(Config.mongo, { useMongoClient: true });
  }
}

module.exports = MongoConnection.connect();
