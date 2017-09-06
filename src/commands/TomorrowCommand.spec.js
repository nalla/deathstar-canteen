const chai = require('chai');
const moment = require('moment');

const TomorrowCommand = require('./TomorrowCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('TomorrowCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should return tomorrow\'s menu', async () => {
      // Arrange
      await Menu.create({ date: moment().add(1, 'days').format('YYYYMMDD'), meals: ['Foo', 'Bar'] });

      // Act
      const response = await new TomorrowCommand(null).handle();

      // Assert
      const expected = `Tomorrow is the *${moment().add(1, 'days').format('DD.MM.YYYY')}* and the meals are:\n1. Foo\n2. Bar\n`;
      chai.assert.equal(response, expected);
    });

    it('should return notice when there is no data', async () => {
      // Act
      const response = await new TomorrowCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served tomorrow!');
    });
  });
});
