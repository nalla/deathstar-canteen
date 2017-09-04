'use strict';

const assert = require('chai').assert;
const moment = require('moment');

const TodayCommand = require('../lib/commands/today-command');
const Menu = require('../data/models/menu');
const MongoConnect = require('../data/mongo-connect');

describe('TodayCommand', () => {

  before(() => {
    MongoConnect.initialize();
  });

  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {

    it('should return today\'s menu', async () => {
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foo', 'Bar'] });
      const expected = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n  Foo\n  Bar\n`;
      const response = await new TodayCommand(null).handle();
      assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      const response = await new TodayCommand(null).handle();
      assert.equal(response, 'I don\'t know which meals are being served today!');
    });
  });
});

