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
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foo', 'Bar'] });

      // Act
      const response = await new TodayCommand(null).handle();

      // Assert
      const expected = `Today is the *${moment().format('DD.MM.YYYY')}* and the meals are:\n1. Foo\n2. Bar\n`;
      chai.assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      // Act
      const response = await new TodayCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served today!');
    });
  });
});
