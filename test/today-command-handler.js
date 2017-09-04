'use strict';

const assert = require('chai').assert;
const moment = require('moment');

const TodayCommandHandler = require('../lib/handlers/today-command-handler');
const Menu = require('../data/models/menu');
const MongoConnect = require('../data/mongo-connect');

describe('TodayCommandHandler', () => {
  beforeEach(done => {
    MongoConnect.initialize(() => {
      Menu.remove({}, error => {
        if (error)
          done();
        else
          Menu.create({date: moment().format('YYYYMMDD'), meals: ['Foo', 'Bar']}, (error, menu) => {
            done();
          });
      });
    });
  });
  describe('#handle()', () => {
    it('should return today\'s menu', () => {
      const expected = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n  Foo\n  Bar\n`;
      return new TodayCommandHandler(null).handle().then(response => {
        assert.equal(response, expected);
      });
    });
  });
});

