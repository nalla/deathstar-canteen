const chai = require('chai');
const moment = require('moment');

const NextCommand = require('./NextCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('NextCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should displays today\'s menu using 1 as parameter', async () => {
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foobar'] });

      // Act
      const response = await new NextCommand('1').handle();

      // Assert
      chai.assert.equal(response, `On *${moment().format('DD.MM.YYYY')}* the meals are:\n1. Foobar\n`);
    });

    it('should display today\'s and tomorrow\'s menu using 2 as parameter', async () => {
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foobar'] });
      await Menu.create({ date: moment().add(1, 'days').format('YYYYMMDD'), meals: ['Raboof'] });

      // Act
      const response = await new NextCommand('2').handle();

      // Assert
      chai.assert.equal(response, `On *${moment().format('DD.MM.YYYY')}* the meals are:\n1. Foobar\n` +
        `On *${moment().add(1, 'days').format('DD.MM.YYYY')}* the meals are:\n1. Raboof\n`);
    });

    it('should only display menus in queried range', async () => {
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foobar'] });
      await Menu.create({ date: moment().add(1, 'days').format('YYYYMMDD'), meals: ['Raboof'] });

      // Act
      const response = await new NextCommand('1').handle();

      // Assert
      chai.assert.equal(response, `On *${moment().format('DD.MM.YYYY')}* the meals are:\n1. Foobar\n`);
    });

    it('should return notice about missing command data', async () => {
      // Act
      const response = await new NextCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some input.');
    });

    it('should return notice about missing invalid data', async () => {
      // Act
      const response = await new NextCommand('Foobar').handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some valid input.');
    });

    it('should return notice when there is no data', async () => {
      // Act
      const response = await new NextCommand('1').handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served the next 1 days!');
    });

    it('should return a maximum of 7 days', async () => {
      // Act
      const response = await new NextCommand('10').handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served the next 7 days!');
    });

    it('should return a minimum of 1 days', async () => {
      // Act
      const response = await new NextCommand('0').handle();

      // Assert
      chai.assert.equal(response, 'I don\'t know which meals are being served the next 1 days!');
    });
  });
});
