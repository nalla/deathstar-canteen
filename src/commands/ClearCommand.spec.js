const chai = require('chai');
const moment = require('moment');

const ClearCommand = require('./ClearCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('ClearCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should clear today\'s menu using the no-dot notation', async () => {
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foobar'] });

      // Act
      const response = await new ClearCommand(moment().format('DDMMYYYY')).handle();
      const menu = await Menu.findOne({ date: moment().format('YYYYMMDD') });

      // Assert
      chai.assert.equal(response, `I cleared the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.isNull(menu);
    });

    it('should clear today\'s menu using the dot notation', async () => {
      // Arrange
      await Menu.create({ date: moment().format('YYYYMMDD'), meals: ['Foobar'] });

      // Act
      const response = await new ClearCommand(moment().format('DD.MM.YYYY')).handle();
      const menu = await Menu.findOne({ date: moment().format('YYYYMMDD') });

      // Assert
      chai.assert.equal(response, `I cleared the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.isNull(menu);
    });

    it('should only clear given menu', async () => {
      // Arrange
      await Menu.create({ date: '20010101', meals: ['Foobar'] });
      await Menu.create({ date: '20010102', meals: ['Foobar'] });
      // Act
      const response = await new ClearCommand('01012001').handle();
      const menu1 = await Menu.findOne({ date: '20010101' });
      const menu2 = await Menu.findOne({ date: '20010102' });

      // Assert
      chai.assert.equal(response, 'I cleared the menu on *01.01.2001*.');
      chai.assert.isNull(menu1);
      chai.assert.isOk(menu2);
    });

    it('should return notice about missing command data', async () => {
      // Act
      const response = await new ClearCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some input.');
    });

    it('should return notice about missing invalid data', async () => {
      // Act
      const response = await new ClearCommand('Foobar').handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some valid input.');
    });
  });
});
