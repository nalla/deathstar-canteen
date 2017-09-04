'use strict';

const assert = require('chai').assert;
const moment = require('moment');

const TomorrowCommand = require('../lib/commands/tomorrow-command');
const Menu = require('../data/models/menu');
const MongoConnect = require('../data/mongo-connect');

describe('TomorrowCommand', () => {

  before(() => {
    MongoConnect.initialize();
  });

  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {

    it('should return tomorrow\'s menu', async () => {
      await Menu.create({ date: moment().add(1, 'days').format('YYYYMMDD'), meals: ['Foo', 'Bar'] });
      const expected = `Tomorrow is the *${moment().add(1, 'days').format('DD.MM.YYYY')}* and the meals are:\n  Foo\n  Bar\n`;
      const response = await new TomorrowCommand(null).handle();
      assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      const response = await new TomorrowCommand(null).handle();
      assert.equal(response, 'I don\'t know which meals are being served tomorrow!');
    });
  });
});

