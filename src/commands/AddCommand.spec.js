const chai = require('chai');
const moment = require('moment');

const AddCommand = require('./AddCommand');
const Menu = require('../models/Menu');
require('../MongoConnection');

describe('AddCommand', () => {
  afterEach(async () => {
    await Menu.remove({});
  });

  describe('#handle()', () => {
    it('should add meal to today\'s menu using the no-dot notation', async () => {
      // Act
      const response = await new AddCommand(`${moment().format('DDMMYYYY')} Foobar`).handle();
      const menu = await Menu.findOne({ date: `${moment().format('YYYYMMDD')}` });

      // Assert
      chai.assert.equal(response, `I added _Foobar_ to the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.include(menu.meals, 'Foobar');
    });

    it('should add meal to today\'s menu using the dot notation', async () => {
      // Act
      const response = await new AddCommand(`${moment().format('DD.MM.YYYY')} Foobar`).handle();
      const menu = await Menu.findOne({ date: `${moment().format('YYYYMMDD')}` });

      // Assert
      chai.assert.equal(response, `I added _Foobar_ to the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.include(menu.meals, 'Foobar');
    });

    it('should not add the meal to today\'s menu twice', async () => {
      // Act
      const response1 = await new AddCommand(`${moment().format('DD.MM.YYYY')} Foobar`).handle();
      const response2 = await new AddCommand(`${moment().format('DD.MM.YYYY')} Foobar`).handle();
      const menu = await Menu.findOne({ date: `${moment().format('YYYYMMDD')}` });

      // Assert
      chai.assert.equal(response1, `I added _Foobar_ to the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.equal(response2, `_Foobar_ is already on the menu on *${moment().format('DD.MM.YYYY')}*!`);
      chai.assert.include(menu.meals, 'Foobar');
      chai.assert.equal(menu.meals.length, 1);
    });

    it('should add two different meals to today\'s menu', async () => {
      // Act
      const response1 = await new AddCommand(`${moment().format('DD.MM.YYYY')} Foo`).handle();
      const response2 = await new AddCommand(`${moment().format('DD.MM.YYYY')} Bar`).handle();
      const menu = await Menu.findOne({ date: `${moment().format('YYYYMMDD')}` });

      // Assert
      chai.assert.equal(response1, `I added _Foo_ to the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.equal(response2, `I added _Bar_ to the menu on *${moment().format('DD.MM.YYYY')}*.`);
      chai.assert.include(menu.meals, 'Foo');
      chai.assert.include(menu.meals, 'Bar');
    });

    it('should return notice about missing command data', async () => {
      // Act
      const response = await new AddCommand(null).handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some input.');
    });

    it('should return notice about missing invalid data', async () => {
      // Act
      const response = await new AddCommand(`${moment().format('DD.MM.YYYY')}`).handle();

      // Assert
      chai.assert.equal(response, 'You need to provide some valid input.');
    });
  });
});
