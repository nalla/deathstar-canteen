const chai = require('chai');
const moment = require('moment');

const DayAfterTomorrowCommand = require('./DayAfterTomorrowCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('DayAfterTomorrowCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should return the day after tomorrow\'s menu', async () => {
      // Arrange
      await Menu.create({ date: moment().add(2, 'days').format('YYYYMMDD'), meals: ['Foo', 'Bar'] });

      // Act
      const response = await new DayAfterTomorrowCommand(null).handle();

      // Assert
      const expected = `The day after tomorrow is the *${moment().add(2, 'days').format('DD.MM.YYYY')}* and the meals are:\n1. Foo\n2. Bar\n`;
      chai.assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      // Act
      const response = await new DayAfterTomorrowCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served the day after tomorrow!');
    });
  });
});

