const chai = require('chai');
const moment = require('moment');

const TodayCommand = require('./TodayCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('TodayCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should return today\'s menu', async () => {
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foo', 'Bar'] });
      const expected = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n  Foo\n  Bar\n`;
      const response = await new TodayCommand(null).handle();
      chai.assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      const response = await new TodayCommand(null).handle();
      chai.assert.equal(response, 'I don\'t know which meals are being served today!');
    });
  });
});
